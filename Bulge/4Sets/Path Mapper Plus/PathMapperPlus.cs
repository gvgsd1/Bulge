
using Bulge;
using GH_IO.Serialization;
using Grasshopper.GUI;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Grasshopper.Kernel.Special
{

    public class PathMapper : GH_Param<IGH_Goo>
    {
        public PathMapper()
          : base(new GH_InstanceDescription("First Last Tree Branch At Level", "First Last Tree Branch At Level", "First Last Tree Branch At Level", "Bulge",
                SubCategoryName.SETS))
        {
            Lexers = new List<GH_LexerCombo>();
            Access = GH_ParamAccess.tree;
            Optional = true;
        }

        public override void CreateAttributes() => m_attributes = new PathMapperAttributes(this);

        protected override IGH_Goo InstantiateT() => new GH_ObjectWrapper();

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("{6726600a-6ef5-4172-80e5-0f95bd9f5622}");

        public List<GH_LexerCombo> Lexers { get; }

        public override string InstanceDescription => VolatileDataDescription();

        public override GH_ParamKind Kind => GH_ParamKind.floating;


        private List<GH_Path> GetSourcePaths()
        {
            SortedList<GH_Path, bool> sortedList = new SortedList<GH_Path, bool>();

            foreach (IGH_Param source in (IEnumerable<IGH_Param>)Sources)
            {

                foreach (GH_Path path in (IEnumerable<GH_Path>)source.VolatileData.Paths)
                {
                    if (!sortedList.ContainsKey(path))
                        sortedList.Add(path, true);
                }

            }

            return new List<GH_Path>(sortedList.Keys);
        }

        private List<string> GetInputMapping(bool includeItemIndices)
        {
            List<string> stringList = new List<string>();
            List<GH_Path> sourcePaths = GetSourcePaths();
            List<string> inputMapping;
            if (sourcePaths.Count == 0)
            {
                inputMapping = stringList;
            }
            else
            {
                GH_StringPattern ghStringPattern = new GH_StringPattern("ABCDEFGHJKLMNOPQRSTUVWXYZ", 3);
                List<int> intList = new List<int>();

                foreach (GH_Path ghPath in sourcePaths)
                {
                    if (!intList.Contains(ghPath.Length))
                    {
                        intList.Add(ghPath.Length);
                        ghStringPattern.Reset();
                        List<string> values = new List<string>();
                        int length = ghPath.Length;
                        for (int index = 1; index <= length; ++index)
                            values.Add(ghStringPattern.Next());
                        string str = "{" + string.Join(";", values) + "}";
                        if (includeItemIndices)
                            str += "(i)";
                        stringList.Add(str);
                    }
                }

                inputMapping = stringList;
            }
            return inputMapping;
        }

        public void CreateDefaultMapping()
        {
            Lexers.Clear();
            List<string> inputMapping = GetInputMapping(false);
            if (inputMapping.Count == 0)
                return;

            foreach (string str in inputMapping)
                Lexers.Add(new GH_LexerCombo(str, str));

        }

        public void CreateFlattenMapping()
        {
            Lexers.Clear();
            List<string> inputMapping = GetInputMapping(false);
            if (inputMapping.Count == 0)
                return;

            foreach (string n_source in inputMapping)
                Lexers.Add(new GH_LexerCombo(n_source, "{0}"));

        }

        public void CreateGraftMapping()
        {
            Lexers.Clear();
            List<string> inputMapping = GetInputMapping(false);
            if (inputMapping.Count == 0)
                return;

            foreach (string str in inputMapping)
                Lexers.Add(new GH_LexerCombo(str + "(i)", str.Replace("}", ";i}")));

        }

        public void CreateReverseMapping()
        {
            Lexers.Clear();
            List<string> inputMapping = GetInputMapping(false);
            if (inputMapping.Count == 0)
                return;

            foreach (string str in inputMapping)
                Lexers.Add(new GH_LexerCombo(str + "(i)", str + "(item_count - 1 - i)"));

        }

        public void CreateRenumberMapping()
        {
            Lexers.Clear();
            List<string> inputMapping = GetInputMapping(false);
            if (inputMapping.Count == 0)
                return;

            foreach (string n_source in inputMapping)
                Lexers.Add(new GH_LexerCombo(n_source, "{path_index}"));

        }

        public void CreateTrimMapping()
        {
            Lexers.Clear();
            List<string> inputMapping = GetInputMapping(false);
            if (inputMapping.Count == 0)
                return;

            foreach (string n_source in inputMapping)
            {
                string[] array = n_source.Replace("{", string.Empty).Replace("}", string.Empty).Split(new string[1]
                {
            ";"
                }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length >= 2)
                {
                    Array.Resize(ref array, array.Length - 1);
                    string n_target = "{" + string.Join(";", array) + "}";
                    Lexers.Add(new GH_LexerCombo(n_source, n_target));
                }
            }

        }

        protected override void CollectVolatileData_FromSources()
        {
            Optional = true;
            if (Lexers.Count == 0)
            {
                base.CollectVolatileData_FromSources();
            }
            else
            {
                VolatileData.Clear();

                foreach (IGH_Param source in (IEnumerable<IGH_Param>)Sources)
                {
                    if ((uint)source.Phase < 2U)
                        source.CollectData();
                    if (!source.VolatileData.IsEmpty)
                    {

                        foreach (GH_LexerCombo lexer in Lexers)
                            GH_Lexer.PerformLexicalReplace(lexer.Source, lexer.Target, source.VolatileData, m_data);

                    }
                }

                Phase = GH_SolutionPhase.Collected;
            }
        }

        public override bool AppendMenuItems(ToolStripDropDown menu)
        {
            ToolStripMenuItem toolStripMenuItem1 = Menu_AppendItem(menu, "Mapping Editor...", new EventHandler(Menu_MappingEditorClicked));
            toolStripMenuItem1.ToolTipText = "Open the mapping editor";
            toolStripMenuItem1.Font = GH_FontServer.NewFont(toolStripMenuItem1.Font, FontStyle.Bold);
            bool enabled = GetSourcePaths().Count > 0;
            ToolStripMenuItem toolStripMenuItem2 = Menu_AppendItem(menu, "Create Null Mapping", new EventHandler(Menu_CreateDefaultClicked), enabled);
            ToolStripMenuItem toolStripMenuItem3 = Menu_AppendItem(menu, "Create Flatten Mapping", new EventHandler(Menu_CreateFlattenClicked), enabled);
            ToolStripMenuItem toolStripMenuItem4 = Menu_AppendItem(menu, "Create Graft Mapping", new EventHandler(Menu_CreateGraftClicked), enabled);
            ToolStripMenuItem toolStripMenuItem5 = Menu_AppendItem(menu, "Create Trim Mapping", new EventHandler(Menu_CreateTrimClicked), enabled);
            ToolStripMenuItem toolStripMenuItem6 = Menu_AppendItem(menu, "Create Reverse Mapping", new EventHandler(Menu_CreateReverseClicked), enabled);
            ToolStripMenuItem toolStripMenuItem7 = Menu_AppendItem(menu, "Create Renumber Mapping", new EventHandler(Menu_CreateRenumberClicked), enabled);
            toolStripMenuItem2.ToolTipText = "Create a mapping which doesn't change the tree";
            toolStripMenuItem3.ToolTipText = "Create a mapping which flattens the entire tree";
            toolStripMenuItem4.ToolTipText = "Create a mapping which grafts all the items in the tree";
            toolStripMenuItem5.ToolTipText = "Create a mapping which trims the last elements in all paths";
            toolStripMenuItem6.ToolTipText = "Create a mapping which reverses the item order in all branches";
            toolStripMenuItem7.ToolTipText = "Create a mapping which renumbers all branches";
            Menu_AppendSeparator(menu);
            Menu_AppendEnableItem(menu);
            Menu_AppendSeparator(menu);
            Menu_AppendRuntimeMessages(menu);
            Menu_AppendSeparator(menu);
            Menu_AppendObjectHelp(menu);
            return true;
        }

        private void Menu_MappingEditorClicked(object sender, EventArgs e) => DisplayLexerEditor();

        private void Menu_CreateDefaultClicked(object sender, EventArgs e)
        {
            RecordUndoEvent("Default Mapping");
            CreateDefaultMapping();
            ExpireSolution(true);
        }

        private void Menu_CreateFlattenClicked(object sender, EventArgs e)
        {
            RecordUndoEvent("Flatten Mapping");
            CreateFlattenMapping();
            ExpireSolution(true);
        }

        private void Menu_CreateGraftClicked(object sender, EventArgs e)
        {
            RecordUndoEvent("Graft Mapping");
            CreateGraftMapping();
            ExpireSolution(true);
        }

        private void Menu_CreateTrimClicked(object sender, EventArgs e)
        {
            RecordUndoEvent("Trim Mapping");
            CreateTrimMapping();
            ExpireSolution(true);
        }

        private void Menu_CreateReverseClicked(object sender, EventArgs e)
        {
            RecordUndoEvent("Reverse Mapping");
            CreateReverseMapping();
            ExpireSolution(true);
        }

        private void Menu_CreateRenumberClicked(object sender, EventArgs e)
        {
            RecordUndoEvent("Renumber Mapping");
            CreateRenumberMapping();
            ExpireSolution(true);
        }

        public void DisplayLexerEditor()
        {
            GH_LexerComboEditor F = new GH_LexerComboEditor();
            GH_WindowsFormUtil.CenterFormOnCursor(F, true);
            F.Lexers = Lexers;
            if (F.ShowDialog(Instances.DocumentEditor) != DialogResult.OK)
                return;
            TriggerAutoSave();
            RecordUndoEvent("Lexical Change");
            try
            {
                Lexers.Clear();
                Lexers.AddRange(F.Lexers);
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                Tracing.Assert(new Guid("{965882bd-9fc1-45bd-a2fb-1fec8baad7c8}"), "Lexers could not be parsed");
                ProjectData.ClearProjectError();
            }
            ExpireSolution(true);
        }

        protected override string VolatileDataDescription()
        {
            string str;
            if (Lexers == null)
                str = null;
            else if (Lexers.Count == 0)
            {
                str = null;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (GH_LexerCombo lexer in Lexers)
                    stringBuilder.AppendLine(string.Format("{0} → {1}", lexer.Source.Mask, lexer.Target.Mask));

                str = stringBuilder.ToString();
            }
            return str;
        }

        public override bool Write(GH_IWriter writer)
        {
            GH_IWriter chunk = writer.CreateChunk("lexers");
            chunk.SetInt32("lexer_count", Lexers.Count);
            int num = Lexers.Count - 1;
            for (int index = 0; index <= num; ++index)
            {
                chunk.SetString("source", index, Lexers[index].Source.Mask);
                chunk.SetString("target", index, Lexers[index].Target.Mask);
            }
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            Lexers.Clear();
            GH_IReader chunk = reader.FindChunk("lexers");
            if (chunk != null)
            {
                int num = chunk.GetInt32("lexer_count") - 1;
                for (int item_index = 0; item_index <= num; ++item_index)
                    Lexers.Add(new GH_LexerCombo(chunk.GetString("source", item_index), chunk.GetString("target", item_index)));
            }
            return base.Read(reader);
        }
    }
}
