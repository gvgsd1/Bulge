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

namespace Bulge
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
          : base(new GH_InstanceDescription("Button 11", "Button", "Button object with two values", "Bulge", "Input"))
        {
            this.m_buttonDown = false;
            this.m_expNormal = "False";
            this.m_expPressed = "True";
            this.m_valNormal = new GH_Boolean(false);
            this.m_valPressed = new GH_Boolean(true);
            this.m_exceptions = new List<string>();
        }

        public override void CreateAttributes() => this.m_attributes = (IGH_Attributes)new GH_ButtonAttributes(this);

        public bool ButtonDown
        {
            get => this.m_buttonDown;
            set => this.m_buttonDown = value;
        }

        public string ExpressionNormal
        {
            get => this.m_expNormal;
            set
            {
                this.m_expNormal = value;
                this.m_valNormal = null;
            }
        }

        public string ExpressionPressed
        {
            get => this.m_expPressed;
            set
            {
                this.m_expPressed = value;
                this.m_valPressed = null;
            }
        }

        public void EvaluateExpressions()
        {
            this.m_exceptions.Clear();
            this.m_valNormal = GH_Convert.ParseExpression(this.ExpressionNormal, true).ToGoo();
            this.m_valPressed = GH_Convert.ParseExpression(this.ExpressionPressed, true).ToGoo();
            if (this.m_valNormal == null)
                this.m_exceptions.Add(string.Format("Expression: \"{0}\" could not be evaluated", (object)this.ExpressionNormal));
            if (this.m_valPressed != null)
                return;
            this.m_exceptions.Add(string.Format("Expression: \"{0}\" could not be evaluated", (object)this.ExpressionPressed));
        }

        protected override void CollectVolatileData_Custom()
        {
            try
            {
                foreach (string exception in this.m_exceptions)
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, exception);
            }
            finally
            {
                //List<string>.Enumerator enumerator;
                //enumerator.Dispose();
            }
            this.m_data.Clear();
            if (this.m_buttonDown)
                this.m_data.Append(this.m_valPressed, new GH_Path(0));
            else
                this.m_data.Append(this.m_valNormal, new GH_Path(0));
        }

        public override GH_ParamKind Kind => GH_ParamKind.floating;

        protected override Bitmap Icon => Properties.Resources.S44316481;

        public override GH_ParamData DataType => GH_ParamData.local;

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        public override Guid ComponentGuid => new Guid("{c701104f-0ef7-4ab6-9b52-9fde37c40b32}");

        public override bool IconCapableUI => false;

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            ToolStripMenuItem toolStripMenuItem1 = GH_DocumentObject.Menu_AppendItem((ToolStrip)menu, "Normal state");
            ToolStripMenuItem toolStripMenuItem2 = GH_DocumentObject.Menu_AppendItem((ToolStrip)menu, "Pressed state");
            toolStripMenuItem1.ToolTipText = "Represents the value of the button in the unpressed state";
            toolStripMenuItem2.ToolTipText = "Represents the value of the button in the depressed state";
            GH_DocumentObject.Menu_AppendTextItem(toolStripMenuItem1.DropDown, this.ExpressionNormal, new GH_MenuTextBox.KeyDownEventHandler(this.Menu_ExpressionNormalKeyDown), new GH_MenuTextBox.TextChangedEventHandler(this.Menu_ExpressionTextChanged), true);
            GH_DocumentObject.Menu_AppendTextItem(toolStripMenuItem2.DropDown, this.ExpressionPressed, new GH_MenuTextBox.KeyDownEventHandler(this.Menu_ExpressionPressedKeyDown), new GH_MenuTextBox.TextChangedEventHandler(this.Menu_ExpressionTextChanged), true);
        }

        private void Menu_ExpressionNormalKeyDown(GH_MenuTextBox sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
                return;
            this.RecordUndoEvent("Button Expression");
            this.ExpressionNormal = sender.Text;
            this.EvaluateExpressions();
            this.ExpireSolution(true);
        }

        private void Menu_ExpressionPressedKeyDown(GH_MenuTextBox sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
                return;
            this.RecordUndoEvent("Button Expression");
            this.ExpressionPressed = sender.Text;
            this.EvaluateExpressions();
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
            writer.SetString("ExpressionNormal", this.ExpressionNormal);
            writer.SetString("ExpressionPressed", this.ExpressionPressed);
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            this.ButtonDown = false;
            this.ExpressionNormal = reader.GetString("ExpressionNormal");
            this.ExpressionPressed = reader.GetString("ExpressionPressed");
            this.EvaluateExpressions();
            return base.Read(reader);
        }


    }
}
