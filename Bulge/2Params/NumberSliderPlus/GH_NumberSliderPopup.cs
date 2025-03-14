using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.GUI.Base;
using Grasshopper.GUI.HTML;

using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

#nullable disable
namespace Bulge.Params.NumberSliderPlus
{
    /// <summary>Window for editing number sliders</summary>
    /// <exclude />
    [DesignerGenerated]
    public class GH_NumberSliderPopup : Form
    {
        private IContainer components;
        private GH_NumberSlider m_owner;
        private bool m_changed;
        private decimal cache_lower;
        private decimal cache_upper;
        private decimal cache_value;
        private int m_rounding;
        private Bitmap m_iconR;
        private Bitmap m_iconN;
        private Bitmap m_iconE;
        private Bitmap m_iconO;
        private Button _btnOK;
        private GH_Slider _sldDigits;
        private GH_Slider _sldValue;
        private GH_DigitScroller _numLower;
        private GH_DigitScroller _numUpper;
        private TextBox _txtName;
        private GH_DoubleBufferedPanel _pnlOddRounding;
        private GH_DigitScroller _numRange;
        private GH_DigitScroller _numValue;
        private TextBox _txtExpression;
        private ComboBox _cmbGripStyle;
        private GH_DoubleBufferedPanel _pnlEvenRounding;
        private GH_DoubleBufferedPanel _pnlIntegerRounding;
        private GH_DoubleBufferedPanel _pnlFloatRounding;

        public GH_NumberSliderPopup()
        {
            Load += new EventHandler(GH_DoubleSliderPopup_Load);
            HelpButtonClicked += new CancelEventHandler(GH_DoubleSliderPopup_HelpButtonClicked);
            KeyDown += new KeyEventHandler(GH_DoubleSliderPopup_KeyDown);
            m_rounding = 0;
            //m_iconR = Res_GUI.Slider_Float_20x20;
            //m_iconN = Res_GUI.Slider_Integer_20x20;
            //m_iconE = Res_GUI.Slider_Even_20x20;
            //m_iconO = Res_GUI.Slider_Odd_20x20;
            m_iconR = null;
            m_iconN = null;
            m_iconE = null;
            m_iconO = null;
            InitializeComponent();
        }

        [DebuggerNonUserCode]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!disposing || components == null)
                    return;
                components.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            components = new Container();
            btnOK = new Button();
            Panel2 = new Panel();
            btnCancel = new Button();
            lblAccuracy = new Label();
            lblDigits = new Label();
            grpType = new GroupBox();
            tblType = new TableLayoutPanel();
            sldDigits = new GH_Slider();
            tblRounding = new TableLayoutPanel();
            pnlOddRounding = new GH_DoubleBufferedPanel();
            pnlEvenRounding = new GH_DoubleBufferedPanel();
            pnlIntegerRounding = new GH_DoubleBufferedPanel();
            pnlFloatRounding = new GH_DoubleBufferedPanel();
            grpDomain = new GroupBox();
            tblDomain = new TableLayoutPanel();
            numRange = new GH_DigitScroller();
            numUpper = new GH_DigitScroller();
            numLower = new GH_DigitScroller();
            grpValue = new GroupBox();
            tblValue = new TableLayoutPanel();
            numValue = new GH_DigitScroller();
            sldValue = new GH_Slider();
            pnSep1 = new Panel();
            Panel1 = new Panel();
            grpProp = new GroupBox();
            tblPropLayout = new TableLayoutPanel();
            cmbGripStyle = new ComboBox();
            labelGripStyle = new Label();
            txtExpression = new TextBox();
            lblExpression = new Label();
            lblName = new Label();
            txtName = new TextBox();
            Panel3 = new Panel();
            ToolTip = new ToolTip(components);
            Panel2.SuspendLayout();
            grpType.SuspendLayout();
            tblType.SuspendLayout();
            tblRounding.SuspendLayout();
            grpDomain.SuspendLayout();
            tblDomain.SuspendLayout();
            grpValue.SuspendLayout();
            tblValue.SuspendLayout();
            grpProp.SuspendLayout();
            tblPropLayout.SuspendLayout();
            SuspendLayout();
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Dock = DockStyle.Right;
            btnOK.Location = new Point(103, 8);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(80, 24);
            btnOK.TabIndex = 10;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            Panel2.Controls.Add((Control)btnOK);
            Panel2.Controls.Add((Control)btnCancel);
            Panel2.Dock = DockStyle.Bottom;
            Panel2.Location = new Point(5, 363);
            Panel2.Name = "Panel2";
            Panel2.Padding = new Padding(0, 8, 0, 0);
            Panel2.Size = new Size(263, 32);
            Panel2.TabIndex = 9;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Dock = DockStyle.Right;
            btnCancel.Location = new Point(183, 8);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 24);
            btnCancel.TabIndex = 11;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            lblAccuracy.Dock = DockStyle.Fill;
            lblAccuracy.Location = new Point(1, 1);
            lblAccuracy.Margin = new Padding(1);
            lblAccuracy.Name = "lblAccuracy";
            lblAccuracy.Size = new Size(86, 24);
            lblAccuracy.TabIndex = 4;
            lblAccuracy.Text = "Rounding";
            lblAccuracy.TextAlign = ContentAlignment.MiddleLeft;
            lblDigits.Dock = DockStyle.Fill;
            lblDigits.Location = new Point(1, 27);
            lblDigits.Margin = new Padding(1);
            lblDigits.Name = "lblDigits";
            lblDigits.Size = new Size(86, 26);
            lblDigits.TabIndex = 12;
            lblDigits.Text = "Digits";
            lblDigits.TextAlign = ContentAlignment.MiddleLeft;
            grpType.Controls.Add(tblType);
            grpType.Dock = DockStyle.Top;
            grpType.Location = new Point(5, 103);
            grpType.Name = "grpType";
            grpType.Padding = new Padding(5);
            grpType.Size = new Size(263, 80);
            grpType.TabIndex = 100;
            grpType.TabStop = false;
            grpType.Text = "Slider accuracy";
            tblType.ColumnCount = 2;
            tblType.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35f));
            tblType.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65f));
            tblType.Controls.Add(lblDigits, 0, 1);
            tblType.Controls.Add(lblAccuracy, 0, 0);
            tblType.Controls.Add((Control)sldDigits, 1, 1);
            tblType.Controls.Add(tblRounding, 1, 0);
            tblType.Dock = DockStyle.Top;
            tblType.Location = new Point(5, 18);
            tblType.Name = "tblType";
            tblType.RowCount = 2;
            tblType.RowStyles.Add(new RowStyle(SizeType.Absolute, 26f));
            tblType.RowStyles.Add(new RowStyle(SizeType.Absolute, 16f));
            tblType.Size = new Size(253, 54);
            tblType.TabIndex = 100;
            sldDigits.ControlEdgeColour = SystemColors.ControlText;
            sldDigits.ControlShadowColour = Color.FromArgb(30, 0, 0, 0);
            sldDigits.DecimalPlaces = 2;
            sldDigits.Dock = DockStyle.Fill;
            sldDigits.DrawControlBorder = false;
            sldDigits.DrawControlShadows = false;
            sldDigits.FormatMask = "{0}";
            sldDigits.GripBottomColour = SystemColors.Window;
            sldDigits.GripDisplay = GH_SliderGripDisplay.Numeric;
            sldDigits.GripEdgeColour = SystemColors.ControlText;
            sldDigits.GripTopColour = SystemColors.Window;
            sldDigits.Location = new Point(91, 29);
            sldDigits.Maximum = new decimal(new int[4]
            {
        6,
        0,
        0,
        0
            });
            sldDigits.Minimum = new decimal(new int[4]);
            sldDigits.Name = "sldDigits";
            sldDigits.Padding = new Padding(0, 2, 1, 2);
            sldDigits.RailBrightColour = SystemColors.ControlLightLight;
            sldDigits.RailDarkColour = SystemColors.ControlDark;
            sldDigits.RailDisplay = GH_SliderRailDisplay.Simple;
            sldDigits.RailEmptyColour = SystemColors.Control;
            sldDigits.RailFullColour = SystemColors.Highlight;
            sldDigits.ShadowSize = new Padding(5, 5, 0, 0);
            sldDigits.ShowTextInputOnDoubleClick = true;
            sldDigits.ShowTextInputOnKeyDown = true;
            sldDigits.Size = new Size(159, 22);
            sldDigits.TabIndex = 4;
            sldDigits.TextColour = SystemColors.ControlText;
            sldDigits.TickCount = 7;
            sldDigits.TickDisplay = GH_SliderTickDisplay.Simple;
            sldDigits.TickFrequency = 5;
            ToolTip.SetToolTip((Control)sldDigits, "Number of decimal places for floating point sliders");
            sldDigits.Type = GH_SliderAccuracy.Integer;
            sldDigits.Value = new decimal(new int[4]
            {
        2,
        0,
        0,
        0
            });
            tblRounding.ColumnCount = 4;
            tblRounding.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tblRounding.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tblRounding.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tblRounding.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            tblRounding.Controls.Add((Control)pnlOddRounding, 3, 0);
            tblRounding.Controls.Add((Control)pnlEvenRounding, 2, 0);
            tblRounding.Controls.Add((Control)pnlIntegerRounding, 1, 0);
            tblRounding.Controls.Add((Control)pnlFloatRounding, 0, 0);
            tblRounding.Location = new Point(88, 0);
            tblRounding.Margin = new Padding(0);
            tblRounding.Name = "tblRounding";
            tblRounding.RowCount = 1;
            tblRounding.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            tblRounding.RowStyles.Add(new RowStyle(SizeType.Absolute, 26f));
            tblRounding.Size = new Size(165, 26);
            tblRounding.TabIndex = 16;
            pnlOddRounding.Location = new Point(123, 0);
            pnlOddRounding.Margin = new Padding(0);
            pnlOddRounding.Name = "pnlOddRounding";
            pnlOddRounding.Size = new Size(41, 26);
            pnlOddRounding.TabIndex = 3;
            ToolTip.SetToolTip((Control)pnlOddRounding, "Odd numbers");
            pnlEvenRounding.Location = new Point(82, 0);
            pnlEvenRounding.Margin = new Padding(0);
            pnlEvenRounding.Name = "pnlEvenRounding";
            pnlEvenRounding.Size = new Size(41, 26);
            pnlEvenRounding.TabIndex = 2;
            ToolTip.SetToolTip((Control)pnlEvenRounding, "Even numbers");
            pnlIntegerRounding.Location = new Point(41, 0);
            pnlIntegerRounding.Margin = new Padding(0);
            pnlIntegerRounding.Name = "pnlIntegerRounding";
            pnlIntegerRounding.Size = new Size(41, 26);
            pnlIntegerRounding.TabIndex = 1;
            ToolTip.SetToolTip((Control)pnlIntegerRounding, "Integer numbers");
            pnlFloatRounding.Location = new Point(0, 0);
            pnlFloatRounding.Margin = new Padding(0);
            pnlFloatRounding.Name = "pnlFloatRounding";
            pnlFloatRounding.Size = new Size(41, 26);
            pnlFloatRounding.TabIndex = 0;
            ToolTip.SetToolTip((Control)pnlFloatRounding, "Floating Point numbers");
            grpDomain.Controls.Add(tblDomain);
            grpDomain.Dock = DockStyle.Top;
            grpDomain.Location = new Point(5, 193);
            grpDomain.Name = "grpDomain";
            grpDomain.Padding = new Padding(5);
            grpDomain.Size = new Size(263, 89);
            grpDomain.TabIndex = 12;
            grpDomain.TabStop = false;
            grpDomain.Text = "Numeric domain";
            tblDomain.ColumnCount = 1;
            tblDomain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            tblDomain.Controls.Add((Control)numRange, 0, 2);
            tblDomain.Controls.Add((Control)numUpper, 0, 1);
            tblDomain.Controls.Add((Control)numLower, 0, 0);
            tblDomain.Dock = DockStyle.Top;
            tblDomain.Location = new Point(5, 18);
            tblDomain.Name = "tblDomain";
            tblDomain.RowCount = 3;
            tblDomain.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            tblDomain.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            tblDomain.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            tblDomain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            tblDomain.Size = new Size(253, 66);
            tblDomain.TabIndex = 0;
            numRange.AllowRadixDrag = false;
            numRange.AllowTextInput = true;
            numRange.BackColor = SystemColors.Control;
            numRange.DecimalPlaces = 2;
            numRange.DigitAlign = GH_DigitAlign.Right;
            numRange.Digits = 12;
            numRange.Dock = DockStyle.Fill;
            numRange.Location = new Point(0, 44);
            numRange.Margin = new Padding(0, 0, 0, 2);
            numRange.MaximumValue = new decimal(new int[4]
            {
        -1,
        -1,
        -1,
        0
            });
            numRange.MinimumValue = new decimal(new int[4]
            {
        -1,
        -1,
        -1,
        int.MinValue
            });
            numRange.Name = "numRange";
            numRange.Prefix = "Range";
            numRange.Radix = 10;
            numRange.ShowTextInputOnDoubleClick = true;
            numRange.ShowTextInputOnKeyDown = true;
            numRange.Size = new Size(253, 20);
            numRange.Suffix = (string)null;
            numRange.TabIndex = 7;
            ToolTip.SetToolTip((Control)numRange, "Length of slider range (Max - Min)");
            numRange.Value = new decimal(new int[4]
            {
        0,
        0,
        0,
        131072
            });
            numUpper.AllowRadixDrag = false;
            numUpper.AllowTextInput = true;
            numUpper.BackColor = SystemColors.Control;
            numUpper.DecimalPlaces = 2;
            numUpper.DigitAlign = GH_DigitAlign.Right;
            numUpper.Digits = 12;
            numUpper.Dock = DockStyle.Fill;
            numUpper.Location = new Point(0, 22);
            numUpper.Margin = new Padding(0, 0, 0, 2);
            numUpper.MaximumValue = new decimal(new int[4]
            {
        1000000,
        0,
        0,
        0
            });
            numUpper.MinimumValue = new decimal(new int[4]
            {
        999999,
        0,
        0,
        int.MinValue
            });
            numUpper.Name = "numUpper";
            numUpper.Prefix = "Max";
            numUpper.Radix = 10;
            numUpper.ShowTextInputOnDoubleClick = true;
            numUpper.ShowTextInputOnKeyDown = true;
            numUpper.Size = new Size(253, 20);
            numUpper.Suffix = (string)null;
            numUpper.TabIndex = 6;
            ToolTip.SetToolTip((Control)numUpper, "Maximum limit of slider range");
            numUpper.Value = new decimal(new int[4]
            {
        0,
        0,
        0,
        131072
            });
            numLower.AllowRadixDrag = false;
            numLower.AllowTextInput = true;
            numLower.BackColor = SystemColors.Control;
            numLower.DecimalPlaces = 2;
            numLower.DigitAlign = GH_DigitAlign.Right;
            numLower.Digits = 12;
            numLower.Dock = DockStyle.Fill;
            numLower.Location = new Point(0, 0);
            numLower.Margin = new Padding(0, 0, 0, 2);
            numLower.MaximumValue = new decimal(new int[4]
            {
        999999,
        0,
        0,
        0
            });
            numLower.MinimumValue = new decimal(new int[4]
            {
        1000000,
        0,
        0,
        int.MinValue
            });
            numLower.Name = "numLower";
            numLower.Prefix = "Min";
            numLower.Radix = 10;
            numLower.ShowTextInputOnDoubleClick = true;
            numLower.ShowTextInputOnKeyDown = true;
            numLower.Size = new Size(253, 20);
            numLower.Suffix = (string)null;
            numLower.TabIndex = 5;
            ToolTip.SetToolTip((Control)numLower, "Minimum limit of slider range");
            numLower.Value = new decimal(new int[4]
            {
        0,
        0,
        0,
        131072
            });
            grpValue.Controls.Add(tblValue);
            grpValue.Dock = DockStyle.Top;
            grpValue.Location = new Point(5, 292);
            grpValue.Name = "grpValue";
            grpValue.Padding = new Padding(5);
            grpValue.Size = new Size(263, 71);
            grpValue.TabIndex = 13;
            grpValue.TabStop = false;
            grpValue.Text = "Numeric value";
            tblValue.ColumnCount = 1;
            tblValue.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            tblValue.Controls.Add((Control)numValue, 0, 0);
            tblValue.Controls.Add((Control)sldValue, 0, 1);
            tblValue.Dock = DockStyle.Top;
            tblValue.Location = new Point(5, 18);
            tblValue.Name = "tblValue";
            tblValue.RowCount = 2;
            tblValue.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            tblValue.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            tblValue.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            tblValue.Size = new Size(253, 44);
            tblValue.TabIndex = 0;
            numValue.AllowRadixDrag = false;
            numValue.AllowTextInput = true;
            numValue.BackColor = SystemColors.Control;
            numValue.DecimalPlaces = 2;
            numValue.DigitAlign = GH_DigitAlign.Right;
            numValue.Digits = 12;
            numValue.Dock = DockStyle.Fill;
            numValue.Location = new Point(0, 0);
            numValue.Margin = new Padding(0, 0, 0, 2);
            numValue.MaximumValue = new decimal(new int[4]
            {
        -1,
        -1,
        -1,
        0
            });
            numValue.MinimumValue = new decimal(new int[4]
            {
        -1,
        -1,
        -1,
        int.MinValue
            });
            numValue.Name = "numValue";
            numValue.Prefix = "";
            numValue.Radix = 10;
            numValue.ShowTextInputOnDoubleClick = true;
            numValue.ShowTextInputOnKeyDown = true;
            numValue.Size = new Size(253, 20);
            numValue.Suffix = (string)null;
            numValue.TabIndex = 8;
            numValue.Value = new decimal(new int[4]
            {
        0,
        0,
        0,
        131072
            });
            sldValue.ControlEdgeColour = SystemColors.ControlText;
            sldValue.ControlShadowColour = Color.FromArgb(30, 0, 0, 0);
            sldValue.DecimalPlaces = 2;
            sldValue.Dock = DockStyle.Fill;
            sldValue.DrawControlBorder = true;
            sldValue.DrawControlShadows = true;
            sldValue.FormatMask = "{0}";
            sldValue.GripBottomColour = SystemColors.Window;
            sldValue.GripDisplay = GH_SliderGripDisplay.Numeric;
            sldValue.GripEdgeColour = SystemColors.ControlText;
            sldValue.GripTopColour = SystemColors.Window;
            sldValue.Location = new Point(0, 22);
            sldValue.Margin = new Padding(0);
            sldValue.Maximum = new decimal(new int[4]
            {
        1,
        0,
        0,
        0
            });
            sldValue.Minimum = new decimal(new int[4]);
            sldValue.Name = "sldValue";
            sldValue.Padding = new Padding(2);
            sldValue.RailBrightColour = SystemColors.ControlLightLight;
            sldValue.RailDarkColour = SystemColors.ControlDark;
            sldValue.RailDisplay = GH_SliderRailDisplay.Simple;
            sldValue.RailEmptyColour = SystemColors.Control;
            sldValue.RailFullColour = SystemColors.Highlight;
            sldValue.ShadowSize = new Padding(5, 5, 0, 0);
            sldValue.ShowTextInputOnDoubleClick = true;
            sldValue.ShowTextInputOnKeyDown = true;
            sldValue.Size = new Size(253, 22);
            sldValue.TabIndex = 9;
            sldValue.TextColour = SystemColors.ControlText;
            sldValue.TickCount = 21;
            sldValue.TickDisplay = GH_SliderTickDisplay.Simple;
            sldValue.TickFrequency = 5;
            sldValue.Type = GH_SliderAccuracy.Float;
            sldValue.Value = new decimal(new int[4]
            {
        25,
        0,
        0,
        131072
            });
            pnSep1.Dock = DockStyle.Top;
            pnSep1.Location = new Point(5, 183);
            pnSep1.Name = "pnSep1";
            pnSep1.Size = new Size(263, 10);
            pnSep1.TabIndex = 14;
            Panel1.Dock = DockStyle.Top;
            Panel1.Location = new Point(5, 282);
            Panel1.Name = "Panel1";
            Panel1.Size = new Size(263, 10);
            Panel1.TabIndex = 15;
            grpProp.Controls.Add(tblPropLayout);
            grpProp.Dock = DockStyle.Top;
            grpProp.Location = new Point(5, 5);
            grpProp.Name = "grpProp";
            grpProp.Size = new Size(263, 88);
            grpProp.TabIndex = 100;
            grpProp.TabStop = false;
            grpProp.Text = "Properties";
            tblPropLayout.ColumnCount = 2;
            tblPropLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35f));
            tblPropLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65f));
            tblPropLayout.Controls.Add(cmbGripStyle, 1, 2);
            tblPropLayout.Controls.Add(labelGripStyle, 0, 2);
            tblPropLayout.Controls.Add(txtExpression, 1, 1);
            tblPropLayout.Controls.Add(lblExpression, 0, 1);
            tblPropLayout.Controls.Add(lblName, 0, 0);
            tblPropLayout.Controls.Add(txtName, 1, 0);
            tblPropLayout.Dock = DockStyle.Top;
            tblPropLayout.Location = new Point(3, 16);
            tblPropLayout.Name = "tblPropLayout";
            tblPropLayout.RowCount = 3;
            tblPropLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            tblPropLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            tblPropLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            tblPropLayout.Size = new Size(257, 66);
            tblPropLayout.TabIndex = 100;
            cmbGripStyle.Dock = DockStyle.Fill;
            cmbGripStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGripStyle.FormattingEnabled = true;
            cmbGripStyle.Items.AddRange(new object[3]
            {
         "Shape",
         "Shape & Text",
         "Box & Text"
            });
            cmbGripStyle.Location = new Point(89, 44);
            cmbGripStyle.Margin = new Padding(0);
            cmbGripStyle.Name = "cmbGripStyle";
            cmbGripStyle.Size = new Size(168, 21);
            cmbGripStyle.TabIndex = 3;
            labelGripStyle.Dock = DockStyle.Fill;
            labelGripStyle.Location = new Point(1, 45);
            labelGripStyle.Margin = new Padding(1);
            labelGripStyle.Name = "labelGripStyle";
            labelGripStyle.Size = new Size(87, 20);
            labelGripStyle.TabIndex = 15;
            labelGripStyle.Text = "Grip Style";
            labelGripStyle.TextAlign = ContentAlignment.MiddleLeft;
            txtExpression.Dock = DockStyle.Fill;
            txtExpression.Location = new Point(89, 22);
            txtExpression.Margin = new Padding(0);
            txtExpression.Name = "txtExpression";
            txtExpression.Size = new Size(168, 20);
            txtExpression.TabIndex = 2;
            txtExpression.TextAlign = HorizontalAlignment.Center;
            ToolTip.SetToolTip(txtExpression, "Expression to post-process slider value");
            lblExpression.Dock = DockStyle.Fill;
            lblExpression.Location = new Point(1, 23);
            lblExpression.Margin = new Padding(1);
            lblExpression.Name = "lblExpression";
            lblExpression.Size = new Size(87, 20);
            lblExpression.TabIndex = 12;
            lblExpression.Text = "Expression";
            lblExpression.TextAlign = ContentAlignment.MiddleLeft;
            lblName.Dock = DockStyle.Fill;
            lblName.Location = new Point(1, 1);
            lblName.Margin = new Padding(1);
            lblName.Name = "lblName";
            lblName.Size = new Size(87, 20);
            lblName.TabIndex = 4;
            lblName.Text = "Name";
            lblName.TextAlign = ContentAlignment.MiddleLeft;
            txtName.Dock = DockStyle.Fill;
            txtName.Location = new Point(89, 0);
            txtName.Margin = new Padding(0);
            txtName.Name = "txtName";
            txtName.Size = new Size(168, 20);
            txtName.TabIndex = 1;
            ToolTip.SetToolTip(txtName, "Name of slider");
            Panel3.Dock = DockStyle.Top;
            Panel3.Location = new Point(5, 93);
            Panel3.Name = "Panel3";
            Panel3.Size = new Size(263, 10);
            Panel3.TabIndex = 100;
            ToolTip.AutoPopDelay = 32000;
            ToolTip.InitialDelay = 500;
            ToolTip.ReshowDelay = 100;
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(273, 400);
            Controls.Add(grpValue);
            Controls.Add(Panel1);
            Controls.Add(grpDomain);
            Controls.Add(Panel2);
            Controls.Add(pnSep1);
            Controls.Add(grpType);
            Controls.Add(Panel3);
            Controls.Add(grpProp);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            HelpButton = true;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(260, 130);
            Name = nameof(GH_NumberSliderPopup);
            Padding = new Padding(5);
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.Manual;
            Text = "Slider:";
            Panel2.ResumeLayout(false);
            grpType.ResumeLayout(false);
            tblType.ResumeLayout(false);
            tblRounding.ResumeLayout(false);
            grpDomain.ResumeLayout(false);
            tblDomain.ResumeLayout(false);
            grpValue.ResumeLayout(false);
            tblValue.ResumeLayout(false);
            grpProp.ResumeLayout(false);
            tblPropLayout.ResumeLayout(false);
            tblPropLayout.PerformLayout();
            ResumeLayout(false);
        }

        internal virtual Button btnOK
        {
            get => this._btnOK;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = new EventHandler(btnOK_Click);
                Button btnOk1 = this._btnOK;
                if (btnOk1 != null)
                    btnOk1.Click -= eventHandler;
                this._btnOK = value;
                Button btnOk2 = this._btnOK;
                if (btnOk2 == null)
                    return;
                btnOk2.Click += eventHandler;
            }
        }

        [field: AccessedThroughProperty("Panel2")]
        internal virtual Panel Panel2 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("btnCancel")]
        internal virtual Button btnCancel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("lblAccuracy")]
        internal virtual Label lblAccuracy { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("lblDigits")]
        internal virtual Label lblDigits { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("grpType")]
        internal virtual GroupBox grpType { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("tblType")]
        internal virtual TableLayoutPanel tblType { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("grpDomain")]
        internal virtual GroupBox grpDomain { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("tblDomain")]
        internal virtual TableLayoutPanel tblDomain { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("grpValue")]
        internal virtual GroupBox grpValue { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("tblValue")]
        internal virtual TableLayoutPanel tblValue { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("pnSep1")]
        internal virtual Panel pnSep1 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("Panel1")]
        internal virtual Panel Panel1 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        internal virtual GH_Slider sldDigits
        {
            get => this._sldDigits;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                GH_Slider.ValueChangedEventHandler changedEventHandler = new GH_Slider.ValueChangedEventHandler(this.sldDigits_ValueChanged);
                GH_Slider sldDigits1 = this._sldDigits;
                if (sldDigits1 != null)
                    sldDigits1.ValueChanged -= changedEventHandler;
                this._sldDigits = value;
                GH_Slider sldDigits2 = this._sldDigits;
                if (sldDigits2 == null)
                    return;
                sldDigits2.ValueChanged += changedEventHandler;
            }
        }

        internal virtual GH_Slider sldValue
        {
            get => this._sldValue;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                GH_Slider.ValueChangedEventHandler changedEventHandler = new GH_Slider.ValueChangedEventHandler(this.sldValue_ValueChanged);
                GH_Slider sldValue1 = this._sldValue;
                if (sldValue1 != null)
                    sldValue1.ValueChanged -= changedEventHandler;
                this._sldValue = value;
                GH_Slider sldValue2 = this._sldValue;
                if (sldValue2 == null)
                    return;
                sldValue2.ValueChanged += changedEventHandler;
            }
        }

        internal virtual GH_DigitScroller numLower
        {
            get => this._numLower;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                GH_DigitScroller.ValueChangedEventHandler changedEventHandler = new GH_DigitScroller.ValueChangedEventHandler(this.numLower_ValueChanged);
                GH_DigitScroller numLower1 = this._numLower;
                if (numLower1 != null)
                    numLower1.ValueChanged -= changedEventHandler;
                this._numLower = value;
                GH_DigitScroller numLower2 = this._numLower;
                if (numLower2 == null)
                    return;
                numLower2.ValueChanged += changedEventHandler;
            }
        }

        internal virtual GH_DigitScroller numUpper
        {
            get => this._numUpper;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                GH_DigitScroller.ValueChangedEventHandler changedEventHandler = new GH_DigitScroller.ValueChangedEventHandler(this.numUpper_ValueChanged);
                GH_DigitScroller numUpper1 = this._numUpper;
                if (numUpper1 != null)
                    numUpper1.ValueChanged -= changedEventHandler;
                this._numUpper = value;
                GH_DigitScroller numUpper2 = this._numUpper;
                if (numUpper2 == null)
                    return;
                numUpper2.ValueChanged += changedEventHandler;
            }
        }

        internal virtual GH_DigitScroller numRange
        {
            get => this._numRange;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                GH_DigitScroller.ValueChangedEventHandler changedEventHandler = new GH_DigitScroller.ValueChangedEventHandler(this.numRange_ValueChanged);
                GH_DigitScroller numRange1 = this._numRange;
                if (numRange1 != null)
                    numRange1.ValueChanged -= changedEventHandler;
                this._numRange = value;
                GH_DigitScroller numRange2 = this._numRange;
                if (numRange2 == null)
                    return;
                numRange2.ValueChanged += changedEventHandler;
            }
        }

        internal virtual GH_DigitScroller numValue
        {
            get => this._numValue;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                GH_DigitScroller.ValueChangedEventHandler changedEventHandler = new GH_DigitScroller.ValueChangedEventHandler(this.numValue_ValueChanged);
                GH_DigitScroller numValue1 = this._numValue;
                if (numValue1 != null)
                    numValue1.ValueChanged -= changedEventHandler;
                this._numValue = value;
                GH_DigitScroller numValue2 = this._numValue;
                if (numValue2 == null)
                    return;
                numValue2.ValueChanged += changedEventHandler;
            }
        }

        [field: AccessedThroughProperty("grpProp")]
        internal virtual GroupBox grpProp { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("Panel3")]
        internal virtual Panel Panel3 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("tblPropLayout")]
        internal virtual TableLayoutPanel tblPropLayout { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("lblExpression")]
        internal virtual Label lblExpression { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("lblName")]
        internal virtual Label lblName { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        internal virtual TextBox txtName
        {
            get => this._txtName;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = new EventHandler(txtName_TextChanged);
                TextBox txtName1 = this._txtName;
                if (txtName1 != null)
                    txtName1.TextChanged -= eventHandler;
                this._txtName = value;
                TextBox txtName2 = this._txtName;
                if (txtName2 == null)
                    return;
                txtName2.TextChanged += eventHandler;
            }
        }

        internal virtual TextBox txtExpression
        {
            get => this._txtExpression;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = new EventHandler(txtExpression_TextChanged);
                TextBox txtExpression1 = this._txtExpression;
                if (txtExpression1 != null)
                    txtExpression1.TextChanged -= eventHandler;
                this._txtExpression = value;
                TextBox txtExpression2 = this._txtExpression;
                if (txtExpression2 == null)
                    return;
                txtExpression2.TextChanged += eventHandler;
            }
        }

        [field: AccessedThroughProperty("labelGripStyle")]
        internal virtual Label labelGripStyle { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        internal virtual ComboBox cmbGripStyle
        {
            get => this._cmbGripStyle;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = new EventHandler(cmbGripStyle_SelectedIndexChanged);
                ComboBox cmbGripStyle1 = this._cmbGripStyle;
                if (cmbGripStyle1 != null)
                    cmbGripStyle1.SelectedIndexChanged -= eventHandler;
                this._cmbGripStyle = value;
                ComboBox cmbGripStyle2 = this._cmbGripStyle;
                if (cmbGripStyle2 == null)
                    return;
                cmbGripStyle2.SelectedIndexChanged += eventHandler;
            }
        }

        [field: AccessedThroughProperty("tblRounding")]
        internal virtual TableLayoutPanel tblRounding { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        internal virtual GH_DoubleBufferedPanel pnlOddRounding
        {
            get => this._pnlOddRounding;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler1 = new EventHandler(pnlRounding_MouseEnter);
                EventHandler eventHandler2 = new EventHandler(pnlRounding_MouseLeave);
                PaintEventHandler paintEventHandler = new PaintEventHandler(pnlOddRounding_Paint);
                MouseEventHandler mouseEventHandler = new MouseEventHandler(pnlOddRounding_MouseDown);
                GH_DoubleBufferedPanel pnlOddRounding1 = this._pnlOddRounding;
                if (pnlOddRounding1 != null)
                {
                    pnlOddRounding1.MouseEnter -= eventHandler1;
                    pnlOddRounding1.MouseLeave -= eventHandler2;
                    pnlOddRounding1.Paint -= paintEventHandler;
                    pnlOddRounding1.MouseDown -= mouseEventHandler;
                }
                this._pnlOddRounding = value;
                GH_DoubleBufferedPanel pnlOddRounding2 = this._pnlOddRounding;
                if (pnlOddRounding2 == null)
                    return;
                pnlOddRounding2.MouseEnter += eventHandler1;
                pnlOddRounding2.MouseLeave += eventHandler2;
                pnlOddRounding2.Paint += paintEventHandler;
                pnlOddRounding2.MouseDown += mouseEventHandler;
            }
        }

        internal virtual GH_DoubleBufferedPanel pnlEvenRounding
        {
            get => this._pnlEvenRounding;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler1 = new EventHandler(pnlRounding_MouseEnter);
                EventHandler eventHandler2 = new EventHandler(pnlRounding_MouseLeave);
                PaintEventHandler paintEventHandler = new PaintEventHandler(pnlEvenRounding_Paint);
                MouseEventHandler mouseEventHandler = new MouseEventHandler(pnlEvenRounding_MouseDown);
                GH_DoubleBufferedPanel pnlEvenRounding1 = this._pnlEvenRounding;
                if (pnlEvenRounding1 != null)
                {
                    pnlEvenRounding1.MouseEnter -= eventHandler1;
                    pnlEvenRounding1.MouseLeave -= eventHandler2;
                    pnlEvenRounding1.Paint -= paintEventHandler;
                    pnlEvenRounding1.MouseDown -= mouseEventHandler;
                }
                this._pnlEvenRounding = value;
                GH_DoubleBufferedPanel pnlEvenRounding2 = this._pnlEvenRounding;
                if (pnlEvenRounding2 == null)
                    return;
                pnlEvenRounding2.MouseEnter += eventHandler1;
                pnlEvenRounding2.MouseLeave += eventHandler2;
                pnlEvenRounding2.Paint += paintEventHandler;
                pnlEvenRounding2.MouseDown += mouseEventHandler;
            }
        }

        internal virtual GH_DoubleBufferedPanel pnlIntegerRounding
        {
            get => this._pnlIntegerRounding;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler1 = new EventHandler(pnlRounding_MouseEnter);
                EventHandler eventHandler2 = new EventHandler(pnlRounding_MouseLeave);
                PaintEventHandler paintEventHandler = new PaintEventHandler(pnlIntegerRounding_Paint);
                MouseEventHandler mouseEventHandler = new MouseEventHandler(pnlIntegerRounding_MouseDown);
                GH_DoubleBufferedPanel pnlIntegerRounding1 = this._pnlIntegerRounding;
                if (pnlIntegerRounding1 != null)
                {
                    pnlIntegerRounding1.MouseEnter -= eventHandler1;
                    pnlIntegerRounding1.MouseLeave -= eventHandler2;
                    pnlIntegerRounding1.Paint -= paintEventHandler;
                    pnlIntegerRounding1.MouseDown -= mouseEventHandler;
                }
                this._pnlIntegerRounding = value;
                GH_DoubleBufferedPanel pnlIntegerRounding2 = this._pnlIntegerRounding;
                if (pnlIntegerRounding2 == null)
                    return;
                pnlIntegerRounding2.MouseEnter += eventHandler1;
                pnlIntegerRounding2.MouseLeave += eventHandler2;
                pnlIntegerRounding2.Paint += paintEventHandler;
                pnlIntegerRounding2.MouseDown += mouseEventHandler;
            }
        }

        internal virtual GH_DoubleBufferedPanel pnlFloatRounding
        {
            get => this._pnlFloatRounding;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler1 = new EventHandler(pnlRounding_MouseEnter);
                EventHandler eventHandler2 = new EventHandler(pnlRounding_MouseLeave);
                PaintEventHandler paintEventHandler = new PaintEventHandler(pnlFloatRounding_Paint);
                MouseEventHandler mouseEventHandler = new MouseEventHandler(pnlFloatRounding_MouseDown);
                GH_DoubleBufferedPanel pnlFloatRounding1 = this._pnlFloatRounding;
                if (pnlFloatRounding1 != null)
                {
                    pnlFloatRounding1.MouseEnter -= eventHandler1;
                    pnlFloatRounding1.MouseLeave -= eventHandler2;
                    pnlFloatRounding1.Paint -= paintEventHandler;
                    pnlFloatRounding1.MouseDown -= mouseEventHandler;
                }
                this._pnlFloatRounding = value;
                GH_DoubleBufferedPanel pnlFloatRounding2 = this._pnlFloatRounding;
                if (pnlFloatRounding2 == null)
                    return;
                pnlFloatRounding2.MouseEnter += eventHandler1;
                pnlFloatRounding2.MouseLeave += eventHandler2;
                pnlFloatRounding2.Paint += paintEventHandler;
                pnlFloatRounding2.MouseDown += mouseEventHandler;
            }
        }

        [field: AccessedThroughProperty("ToolTip")]
        internal virtual ToolTip ToolTip { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        private void GH_DoubleSliderPopup_Load(object sender, EventArgs e)
        {
            GH_WindowsControlUtil.FixTextRenderingDefault(Controls);
        }

        private void GH_DoubleSliderPopup_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            GH_HtmlFormatter syntax = new GH_HtmlFormatter();
            syntax.Title = "Number slider popup";
            syntax.Description = "This dialog provides access to all the number slider properties. Note that all of these properties are also available through the slider menu. <BR> <BR>" + Environment.NewLine + "<dl>" + Environment.NewLine + "  <dt><b> Name </b></dt>" + Environment.NewLine + "  <dd> The custom name of the slider object. </dd><BR>" + Environment.NewLine + "  <dt><b> Limits </b></dt>" + Environment.NewLine + "  <dd> The numeric limits (extremes) of the numeric domain. Note that the number of decimals in the limit scrollers depends on their magnitude. </dd><BR>" + Environment.NewLine + "  <dt><b> Accuracy </b></dt>" + Environment.NewLine + "  <dd> Sliders can either be floating point accurate or integer accurate. </dd><BR>" + Environment.NewLine + "  <dt><b> Value </b></dt>" + Environment.NewLine + "  <dd> The actual numeric value of the slider. This value is always contrained to the limits. </dd><BR> " + Environment.NewLine + "</dl>";
            syntax.ContactURI = "https://discourse.mcneel.com/";
            syntax.AddRemark("This dialog can be opened through the slider menu or by double-clicking the slider object.");
            GH_HtmlHelpPopup ghHtmlHelpPopup = new GH_HtmlHelpPopup();
            ghHtmlHelpPopup.LoadHTML(syntax);
            ghHtmlHelpPopup.SetLocation(Cursor.Position);
            ghHtmlHelpPopup.Show((IWin32Window)Instances.DocumentEditor);
        }

        private void GH_DoubleSliderPopup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
                return;
            DialogResult = DialogResult.Cancel;
        }

        public void Setup(GH_NumberSlider owner)
        {
            Text = "Slider: " + owner.NickName;
            txtName.Text = owner.NickName;
            txtExpression.Text = owner.Expression;
            cache_lower = owner.Slider.Minimum;
            cache_upper = owner.Slider.Maximum;
            cache_value = owner.Slider.Value;
            switch (owner.Slider.GripDisplay)
            {
                case GH_SliderGripDisplay.Numeric:
                    cmbGripStyle.SelectedIndex = 2;
                    break;
                case GH_SliderGripDisplay.Shape:
                    cmbGripStyle.SelectedIndex = 0;
                    break;
                case GH_SliderGripDisplay.ShapeAndText:
                    cmbGripStyle.SelectedIndex = 1;
                    break;
            }
            switch (owner.Slider.Type)
            {
                case GH_SliderAccuracy.Float:
                    m_rounding = 0;
                    break;
                case GH_SliderAccuracy.Integer:
                    m_rounding = 1;
                    break;
                case GH_SliderAccuracy.Even:
                    m_rounding = 2;
                    break;
                case GH_SliderAccuracy.Odd:
                    m_rounding = 3;
                    break;
            }
            InvalidateRoundingPanels();
            if (owner.Slider.Type == GH_SliderAccuracy.Float)
            {
                numLower.DecimalPlaces = owner.Slider.DecimalPlaces;
                numUpper.DecimalPlaces = owner.Slider.DecimalPlaces;
                numRange.DecimalPlaces = owner.Slider.DecimalPlaces;
                numValue.DecimalPlaces = owner.Slider.DecimalPlaces;
                sldValue.DecimalPlaces = owner.Slider.DecimalPlaces;
            }
            else
            {
                numLower.DecimalPlaces = 0;
                numUpper.DecimalPlaces = 0;
                numRange.DecimalPlaces = 0;
                numValue.DecimalPlaces = 0;
                sldValue.DecimalPlaces = 0;
            }
            numLower.Value = owner.Slider.Minimum;
            numUpper.Value = owner.Slider.Maximum;
            numValue.Value = owner.Slider.Value;
            sldDigits.Value = new decimal(owner.Slider.DecimalPlaces);
            sldValue.Minimum = owner.Slider.Minimum;
            sldValue.Maximum = owner.Slider.Maximum;
            sldValue.Value = owner.Slider.Value;
            m_owner = owner;
            FixUI();
            m_changed = false;
        }

        //public void Setup(GH_NumberSliderBackup owner)
        //{
        //    Text = "Slider: " + owner.NickName;
        //    txtName.Text = owner.NickName;
        //    txtExpression.Text = owner.Expression;
        //    cache_lower = owner.Slider.Minimum;
        //    cache_upper = owner.Slider.Maximum;
        //    cache_value = owner.Slider.Value;
        //    switch (owner.Slider.GripDisplay)
        //    {
        //        case GH_SliderGripDisplay.Numeric:
        //            cmbGripStyle.SelectedIndex = 2;
        //            break;
        //        case GH_SliderGripDisplay.Shape:
        //            cmbGripStyle.SelectedIndex = 0;
        //            break;
        //        case GH_SliderGripDisplay.ShapeAndText:
        //            cmbGripStyle.SelectedIndex = 1;
        //            break;
        //    }
        //    switch (owner.Slider.Type)
        //    {
        //        case GH_SliderAccuracy.Float:
        //            m_rounding = 0;
        //            break;
        //        case GH_SliderAccuracy.Integer:
        //            m_rounding = 1;
        //            break;
        //        case GH_SliderAccuracy.Even:
        //            m_rounding = 2;
        //            break;
        //        case GH_SliderAccuracy.Odd:
        //            m_rounding = 3;
        //            break;
        //    }
        //    InvalidateRoundingPanels();
        //    if (owner.Slider.Type == GH_SliderAccuracy.Float)
        //    {
        //        numLower.DecimalPlaces = owner.Slider.DecimalPlaces;
        //        numUpper.DecimalPlaces = owner.Slider.DecimalPlaces;
        //        numRange.DecimalPlaces = owner.Slider.DecimalPlaces;
        //        numValue.DecimalPlaces = owner.Slider.DecimalPlaces;
        //        sldValue.DecimalPlaces = owner.Slider.DecimalPlaces;
        //    }
        //    else
        //    {
        //        numLower.DecimalPlaces = 0;
        //        numUpper.DecimalPlaces = 0;
        //        numRange.DecimalPlaces = 0;
        //        numValue.DecimalPlaces = 0;
        //        sldValue.DecimalPlaces = 0;
        //    }
        //    numLower.Value = owner.Slider.Minimum;
        //    numUpper.Value = owner.Slider.Maximum;
        //    numValue.Value = owner.Slider.Value;
        //    sldDigits.Value = new decimal(owner.Slider.DecimalPlaces);
        //    sldValue.Minimum = owner.Slider.Minimum;
        //    sldValue.Maximum = owner.Slider.Maximum;
        //    sldValue.Value = owner.Slider.Value;
        //    m_owner = owner;
        //    FixUI();
        //    m_changed = false;
        //}

        private void FixUI()
        {
            if (m_owner == null)
                return;
            switch (m_rounding)
            {
                case 1:
                case 2:
                case 3:
                    sldDigits.Enabled = false;
                    numLower.DecimalPlaces = 0;
                    numUpper.DecimalPlaces = 0;
                    numRange.DecimalPlaces = 0;
                    numValue.DecimalPlaces = 0;
                    break;
                default:
                    sldDigits.Enabled = true;
                    numLower.DecimalPlaces = Convert.ToInt32(sldDigits.Value);
                    numUpper.DecimalPlaces = Convert.ToInt32(sldDigits.Value);
                    numRange.DecimalPlaces = Convert.ToInt32(sldDigits.Value);
                    numValue.DecimalPlaces = Convert.ToInt32(sldDigits.Value);
                    sldValue.DecimalPlaces = Convert.ToInt32(sldDigits.Value);
                    break;
            }
            switch (m_rounding)
            {
                case 1:
                    sldValue.Type = GH_SliderAccuracy.Integer;
                    numLower.Value = GH_SliderBase.ProcessNumber(numLower.Value, GH_SliderAccuracy.Integer, 0);
                    numUpper.Value = GH_SliderBase.ProcessNumber(numUpper.Value, GH_SliderAccuracy.Integer, 0);
                    numValue.Value = GH_SliderBase.ProcessNumber(numValue.Value, GH_SliderAccuracy.Integer, 0);
                    numRange.Value = decimal.Subtract(numUpper.Value, numLower.Value);
                    break;
                case 2:
                    sldValue.Type = GH_SliderAccuracy.Even;
                    numLower.Value = GH_SliderBase.ProcessNumber(numLower.Value, GH_SliderAccuracy.Even, 0);
                    numUpper.Value = GH_SliderBase.ProcessNumber(numUpper.Value, GH_SliderAccuracy.Even, 0);
                    numValue.Value = GH_SliderBase.ProcessNumber(numValue.Value, GH_SliderAccuracy.Even, 0);
                    numRange.Value = decimal.Subtract(numUpper.Value, numLower.Value);
                    break;
                case 3:
                    sldValue.Type = GH_SliderAccuracy.Odd;
                    numLower.Value = GH_SliderBase.ProcessNumber(numLower.Value, GH_SliderAccuracy.Odd, 0);
                    numUpper.Value = GH_SliderBase.ProcessNumber(numUpper.Value, GH_SliderAccuracy.Odd, 0);
                    numValue.Value = GH_SliderBase.ProcessNumber(numValue.Value, GH_SliderAccuracy.Odd, 0);
                    numRange.Value = decimal.Subtract(numUpper.Value, numLower.Value);
                    break;
                default:
                    sldValue.Type = GH_SliderAccuracy.Float;
                    numLower.Value = GH_SliderBase.ProcessNumber(numLower.Value, GH_SliderAccuracy.Float, Convert.ToInt32(sldDigits.Value));
                    numUpper.Value = GH_SliderBase.ProcessNumber(numUpper.Value, GH_SliderAccuracy.Float, Convert.ToInt32(sldDigits.Value));
                    numValue.Value = GH_SliderBase.ProcessNumber(numValue.Value, GH_SliderAccuracy.Float, Convert.ToInt32(sldDigits.Value));
                    numRange.Value = decimal.Subtract(numUpper.Value, numLower.Value);
                    break;
            }
            numRange.MinimumValue = numLower.Epsilon;
            numRange.MaximumValue = Math.Abs(decimal.Subtract(numUpper.MaximumValue, numLower.MinimumValue));
            numValue.MinimumValue = numLower.Value;
            numValue.MaximumValue = numUpper.Value;
            sldValue.Minimum = numLower.Value;
            sldValue.Maximum = numUpper.Value;
            if (decimal.Compare(Math.Abs(decimal.Subtract(sldValue.Value, numValue.Value)), numValue.Epsilon) >= 0)
                sldValue.Value = numValue.Value;
            InvalidateRoundingPanels();
        }

        private void sldDigits_ValueChanged(object sender, EventArgs e)
        {
            m_changed = true;
            FixUI();
        }

        private void numLower_ValueChanged(object sender, GH_DigitScrollerEventArgs e)
        {
            m_changed = true;
            numUpper.Value = Math.Max(numUpper.Value, decimal.Add(e.Value, numLower.Epsilon));
            FixUI();
        }

        private void numUpper_ValueChanged(object sender, GH_DigitScrollerEventArgs e)
        {
            m_changed = true;
            numLower.Value = Math.Min(numLower.Value, decimal.Subtract(e.Value, numLower.Epsilon));
            FixUI();
        }

        private void numRange_ValueChanged(object sender, GH_DigitScrollerEventArgs e)
        {
            numUpper.Value = decimal.Add(numLower.Value, numRange.Value);
            numLower.Value = decimal.Subtract(numUpper.Value, numRange.Value);
            m_changed = true;
            FixUI();
        }

        private void numValue_ValueChanged(object sender, GH_DigitScrollerEventArgs e)
        {
            sldValue.Value = numValue.Value;
            m_changed = true;
        }

        private void sldValue_ValueChanged(object sender, GH_SliderEventArgs e)
        {
            numValue.Value = sldValue.Value;
            m_changed = true;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            m_changed = true;
            Text = "Slider: " + txtName.Text;
        }

        private void txtExpression_TextChanged(object sender, EventArgs e) => m_changed = true;

        private void cmbGripStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_changed = true;
        }

        private void pnlRounding_MouseEnter(object sender, EventArgs e)
        {
            ((Control)sender).Invalidate();
        }

        private void pnlRounding_MouseLeave(object sender, EventArgs e)
        {
            ((Control)sender).Invalidate();
        }

        private void pnlFloatRounding_Paint(object sender, PaintEventArgs e)
        {
            bool hover = pnlFloatRounding.ClientRectangle.Contains(pnlFloatRounding.PointToClient(Cursor.Position));
            PaintRoundingPanel(e.Graphics, m_iconR, m_rounding == 0, hover);
        }

        private void pnlIntegerRounding_Paint(object sender, PaintEventArgs e)
        {
            bool hover = pnlIntegerRounding.ClientRectangle.Contains(pnlIntegerRounding.PointToClient(Cursor.Position));
            PaintRoundingPanel(e.Graphics, m_iconN, m_rounding == 1, hover);
        }

        private void pnlEvenRounding_Paint(object sender, PaintEventArgs e)
        {
            bool hover = pnlEvenRounding.ClientRectangle.Contains(pnlEvenRounding.PointToClient(Cursor.Position));
            PaintRoundingPanel(e.Graphics, m_iconE, m_rounding == 2, hover);
        }

        private void pnlOddRounding_Paint(object sender, PaintEventArgs e)
        {
            bool hover = pnlOddRounding.ClientRectangle.Contains(pnlOddRounding.PointToClient(Cursor.Position));
            PaintRoundingPanel(e.Graphics, m_iconO, m_rounding == 3, hover);
        }

        private void PaintRoundingPanel(Graphics G, Bitmap icon, bool active, bool hover)
        {
            G.Clear(BackColor);
            Rectangle rectangle = new Rectangle(0, 0, pnlFloatRounding.Width, pnlFloatRounding.Height);
            rectangle.Inflate(-4, -1);
            --rectangle.Width;
            --rectangle.Height;
            if (active)
            {
                Brush brush = GH_GraphicsUtil.HighlightBrush();
                Pen pen = GH_GraphicsUtil.HighlightPen();
                G.FillRectangle(brush, rectangle);
                G.DrawRectangle(pen, rectangle);
                brush.Dispose();
                pen.Dispose();
            }
            if (hover)
            {
                Brush brush = GH_GraphicsUtil.HighlightBrush();
                Pen pen = GH_GraphicsUtil.HighlightPen();
                G.FillRectangle(brush, rectangle);
                G.DrawRectangle(pen, rectangle);
                brush.Dispose();
                pen.Dispose();
            }
            GH_GraphicsUtil.RenderIcon(G, (RectangleF)rectangle, (Image)icon);
        }

        private void InvalidateRoundingPanels()
        {
            pnlFloatRounding.Invalidate();
            pnlIntegerRounding.Invalidate();
            pnlEvenRounding.Invalidate();
            pnlOddRounding.Invalidate();
        }

        private void pnlFloatRounding_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_rounding == 0 || e.Button != MouseButtons.Left)
                return;
            m_rounding = 0;
            m_changed = true;
            FixUI();
        }

        private void pnlIntegerRounding_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_rounding == 1 || e.Button != MouseButtons.Left)
                return;
            m_rounding = 1;
            m_changed = true;
            FixUI();
        }

        private void pnlEvenRounding_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_rounding == 2 || e.Button != MouseButtons.Left)
                return;
            m_rounding = 2;
            m_changed = true;
            FixUI();
        }

        private void pnlOddRounding_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_rounding == 3 || e.Button != MouseButtons.Left)
                return;
            m_rounding = 3;
            m_changed = true;
            FixUI();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (m_owner == null)
                return;
            if (!m_changed)
                return;
            try
            {
                m_owner.RecordUndoEvent("Slider settings");
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                ProjectData.ClearProjectError();
            }
            m_owner.NickName = txtName.Text;
            m_owner.Expression = txtExpression.Text;
            switch (m_rounding)
            {
                case 1:
                    m_owner.Slider.Type = GH_SliderAccuracy.Integer;
                    break;
                case 2:
                    m_owner.Slider.Type = GH_SliderAccuracy.Even;
                    break;
                case 3:
                    m_owner.Slider.Type = GH_SliderAccuracy.Odd;
                    break;
                default:
                    m_owner.Slider.Type = GH_SliderAccuracy.Float;
                    break;
            }
            switch (cmbGripStyle.SelectedIndex)
            {
                case 0:
                    m_owner.Slider.GripDisplay = GH_SliderGripDisplay.Shape;
                    break;
                case 1:
                    m_owner.Slider.GripDisplay = GH_SliderGripDisplay.ShapeAndText;
                    break;
                case 2:
                    m_owner.Slider.GripDisplay = GH_SliderGripDisplay.Numeric;
                    break;
                default:
                    m_owner.Slider.GripDisplay = GH_SliderGripDisplay.ShapeAndText;
                    break;
            }
            m_owner.Slider.Minimum = numLower.Value;
            m_owner.Slider.Maximum = numUpper.Value;
            m_owner.Slider.Value = sldValue.Value;
            m_owner.Slider.DecimalPlaces = Convert.ToInt32(sldDigits.Value);
            m_owner.Slider.FixDomain();
            m_owner.Slider.FixValue();
        }
    }
}
