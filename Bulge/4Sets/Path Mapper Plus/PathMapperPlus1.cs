using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Bulge
{
    public class PathMapperPlus1 : GH_Component
    {
        public PathMapperPlus1()
          : base("Path Mapper Plus", "Path Mapper Plus", "Path Mapper Plus", "Bulge",
                SubCategoryName.SETS)
        {
            Lexers = new List<GH_LexerCombo>();
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree", "T", "Data Tree", GH_ParamAccess.tree);
            pManager.AddTextParameter("P1", "P1", "Splitting masks", GH_ParamAccess.item);
            pManager.AddTextParameter("P2", "P2", "Splitting masks", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree", "T", " Tree", GH_ParamAccess.tree);
        }

        public List<GH_LexerCombo> Lexers { get; }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            GH_Structure<IGH_Goo> inTree = null;
            if (!DA.GetDataTree(0, out inTree))
                return;

            string p1 = null;
            string p2 = null;
            if (!DA.GetData(1, ref p1))
                return;
            if (!DA.GetData(2, ref p2))
                return;

            GH_Structure<IGH_Goo> outTree = new GH_Structure<IGH_Goo>();

            GH_LexerCombo lexer = new GH_LexerCombo(p1, p2);

            GH_Lexer.PerformLexicalReplace(lexer.Source, lexer.Target, inTree, outTree);

            DA.SetDataTree(0, outTree);

        }


        //public void CreateDefaultMapping()
        //{
        //    Lexers.Clear();
        //    List<string> inputMapping = GetInputMapping(false);
        //    if (inputMapping.Count == 0)
        //        return;

        //    foreach (string str in inputMapping)
        //        Lexers.Add(new GH_LexerCombo(str, str));

        //}

        //private void Menu_CreateDefaultClicked(object sender, EventArgs e)
        //{
        //    RecordUndoEvent("Default Mapping");
        //    CreateDefaultMapping();
        //    ExpireSolution(true);
        //}

        //public override bool AppendMenuItems(ToolStripDropDown menu)
        //{

        //    //bool enabled = GetSourcePaths().Count > 0;
        //    ToolStripMenuItem toolStripMenuItem2 = Menu_AppendItem(menu, "Create Null Mapping", new EventHandler(Menu_CreateDefaultClicked), true);

        //    return true;
        //}

        public override Guid ComponentGuid => new Guid("{d0a2fbae-8207-4744-a902-0520b302441a}");

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconFirstLastTreeBranch);
    }

}