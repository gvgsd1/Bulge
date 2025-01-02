
using GH_IO.Serialization;
using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.GUI.RemotePanel;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Expressions;
using Grasshopper.Kernel.Types;
using Grasshopper.My.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Bulge.Params.ButtonPlus
{
    /// <exclude />
    public class GH_Button : GH_Param<IGH_Goo>
    {
        private bool m_buttonDown;
        private string m_expNormal;
        private string m_expPressed;
        private IGH_Goo m_valNormal;
        private IGH_Goo m_valPressed;
        private List<string> m_exceptions;

        public GH_Button()
          : base(new GH_InstanceDescription("Button +", "Button +", "Button object with two values", "Bulge", "Params"))
        {
            m_buttonDown = false;
            m_expNormal = "False";
            m_expPressed = "True";
            m_valNormal = new GH_Boolean(false);
            m_valPressed = new GH_Boolean(true);
            m_exceptions = new List<string>();
        }

        public override void CreateAttributes() => m_attributes = new GH_ButtonAttributes(this);

        public bool ButtonDown
        {
            get => m_buttonDown;
            set => m_buttonDown = value;
        }

        public string ExpressionNormal
        {
            get => m_expNormal;
            set
            {
                m_expNormal = value;
                m_valNormal = null;
            }
        }

        public string ExpressionPressed
        {
            get => m_expPressed;
            set
            {
                m_expPressed = value;
                m_valPressed = null;
            }
        }

        public void EvaluateExpressions()
        {
            m_exceptions.Clear();
            m_valNormal = GH_Convert.ParseExpression(ExpressionNormal, true).ToGoo();
            m_valPressed = GH_Convert.ParseExpression(ExpressionPressed, true).ToGoo();
            if (m_valNormal == null)
                m_exceptions.Add(string.Format("Expression: \"{0}\" could not be evaluated", ExpressionNormal));
            if (m_valPressed != null)
                return;
            m_exceptions.Add(string.Format("Expression: \"{0}\" could not be evaluated", ExpressionPressed));
        }

        protected override void CollectVolatileData_Custom()
        {
            try
            {
                foreach (string exception in m_exceptions)
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, exception);
            }
            finally
            {
                //List<string>.Enumerator enumerator;
                //enumerator.Dispose();
            }
            m_data.Clear();
            if (m_buttonDown)
                m_data.Append(m_valPressed, new GH_Path(0));
            else
                m_data.Append(m_valNormal, new GH_Path(0));
        }

        public override GH_ParamKind Kind => GH_ParamKind.floating;

        protected override Bitmap Icon => Properties.Resources.S44316481;

        public override GH_ParamData DataType => GH_ParamData.local;

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        public override Guid ComponentGuid => new Guid("{c701104f-0ef7-4ab6-9b52-9fde37c40b32}");

        public override bool IconCapableUI => false;

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            ToolStripMenuItem toolStripMenuItem1 = Menu_AppendItem(menu, "Normal state");
            ToolStripMenuItem toolStripMenuItem2 = Menu_AppendItem(menu, "Pressed state");
            toolStripMenuItem1.ToolTipText = "Represents the value of the button in the unpressed state";
            toolStripMenuItem2.ToolTipText = "Represents the value of the button in the depressed state";
            Menu_AppendTextItem(toolStripMenuItem1.DropDown, ExpressionNormal, new GH_MenuTextBox.KeyDownEventHandler(Menu_ExpressionNormalKeyDown), new GH_MenuTextBox.TextChangedEventHandler(Menu_ExpressionTextChanged), true);
            Menu_AppendTextItem(toolStripMenuItem2.DropDown, ExpressionPressed, new GH_MenuTextBox.KeyDownEventHandler(Menu_ExpressionPressedKeyDown), new GH_MenuTextBox.TextChangedEventHandler(Menu_ExpressionTextChanged), true);
        }

        private void Menu_ExpressionNormalKeyDown(GH_MenuTextBox sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
                return;
            RecordUndoEvent("Button Expression");
            ExpressionNormal = sender.Text;
            EvaluateExpressions();
            ExpireSolution(true);
        }

        private void Menu_ExpressionPressedKeyDown(GH_MenuTextBox sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
                return;
            RecordUndoEvent("Button Expression");
            ExpressionPressed = sender.Text;
            EvaluateExpressions();
        }

        private void Menu_ExpressionTextChanged(GH_MenuTextBox sender, string text)
        {
            if (GH_Convert.ParseExpression(text, true).Type == GH_VariantType.@null)
                sender.TextBoxItem.ForeColor = Color.Red;
            else
                sender.TextBoxItem.ForeColor = SystemColors.ControlText;
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetString("ExpressionNormal", ExpressionNormal);
            writer.SetString("ExpressionPressed", ExpressionPressed);
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            ButtonDown = false;
            ExpressionNormal = reader.GetString("ExpressionNormal");
            ExpressionPressed = reader.GetString("ExpressionPressed");
            EvaluateExpressions();
            return base.Read(reader);
        }


    }
}
