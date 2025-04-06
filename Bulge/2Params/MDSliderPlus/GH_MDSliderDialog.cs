using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Microsoft.VisualBasic.CompilerServices;
using Rhino.Geometry;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

#nullable disable
namespace Bulge.Params.MDSliderPlus
{
    /// <exclude />
    [DesignerGenerated]
    public class GH_MDSliderDialog : Form
    {
        private IContainer components = null;
        private TextBox _txtXDomain;
        private TextBox _txtZDomain;
        private TextBox _txtYDomain;

        public GH_MDSliderDialog() => InitializeComponent();

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
            pnlButtons = new Panel();
            btnOK = new Button();
            btnCancel = new Button();
            tblControls = new TableLayoutPanel();
            txtZDomain = new TextBox();
            lblZDomain = new Label();
            txtYDomain = new TextBox();
            lblYDomain = new Label();
            lblXDomain = new Label();
            txtXDomain = new TextBox();
            pnlButtons.SuspendLayout();
            tblControls.SuspendLayout();
            SuspendLayout();
            pnlButtons.Controls.Add((Control)btnOK);
            pnlButtons.Controls.Add((Control)btnCancel);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new System.Drawing.Point(3, 138);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new Padding(0, 8, 0, 0);
            pnlButtons.Size = new Size(242, 32);
            pnlButtons.TabIndex = 0;
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Dock = DockStyle.Right;
            btnOK.Location = new System.Drawing.Point(82, 8);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(80, 24);
            btnOK.TabIndex = 1;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Dock = DockStyle.Right;
            btnCancel.Location = new System.Drawing.Point(162, 8);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(80, 24);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            tblControls.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            tblControls.ColumnCount = 2;
            tblControls.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80f));
            tblControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            tblControls.Controls.Add(txtZDomain, 1, 3);
            tblControls.Controls.Add(lblZDomain, 0, 3);
            tblControls.Controls.Add(txtYDomain, 1, 2);
            tblControls.Controls.Add(lblYDomain, 0, 2);
            tblControls.Controls.Add(lblXDomain, 0, 1);
            tblControls.Controls.Add(txtXDomain, 1, 1);
            tblControls.Dock = DockStyle.Fill;
            tblControls.Location = new System.Drawing.Point(3, 3);
            tblControls.Name = "tblControls";
            tblControls.RowCount = 5;
            tblControls.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            tblControls.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            tblControls.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            tblControls.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            tblControls.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            tblControls.RowStyles.Add(new RowStyle(SizeType.Absolute, 20f));
            tblControls.Size = new Size(242, 135);
            tblControls.TabIndex = 1;
            txtZDomain.Dock = DockStyle.Fill;
            txtZDomain.Enabled = false;
            txtZDomain.Location = new System.Drawing.Point(84, 68);
            txtZDomain.Margin = new Padding(0);
            txtZDomain.Name = "txtZDomain";
            txtZDomain.Size = new Size(156, 20);
            txtZDomain.TabIndex = 6;
            txtZDomain.Text = "0.0 To 1.0";
            txtZDomain.TextAlign = HorizontalAlignment.Center;
            lblZDomain.Dock = DockStyle.Fill;
            lblZDomain.Enabled = false;
            lblZDomain.Location = new System.Drawing.Point(7, 68);
            lblZDomain.Margin = new Padding(5, 0, 0, 0);
            lblZDomain.Name = "lblZDomain";
            lblZDomain.Size = new Size(75, 20);
            lblZDomain.TabIndex = 5;
            lblZDomain.Text = "Z-Domain";
            lblZDomain.TextAlign = ContentAlignment.MiddleCenter;
            txtYDomain.Dock = DockStyle.Fill;
            txtYDomain.Location = new System.Drawing.Point(84, 46);
            txtYDomain.Margin = new Padding(0);
            txtYDomain.Name = "txtYDomain";
            txtYDomain.Size = new Size(156, 20);
            txtYDomain.TabIndex = 4;
            txtYDomain.Text = "0.0 To 1.0";
            txtYDomain.TextAlign = HorizontalAlignment.Center;
            lblYDomain.Dock = DockStyle.Fill;
            lblYDomain.Location = new System.Drawing.Point(7, 46);
            lblYDomain.Margin = new Padding(5, 0, 0, 0);
            lblYDomain.Name = "lblYDomain";
            lblYDomain.Size = new Size(75, 20);
            lblYDomain.TabIndex = 3;
            lblYDomain.Text = "Y-Domain";
            lblYDomain.TextAlign = ContentAlignment.MiddleCenter;

            lblXDomain.Dock = DockStyle.Fill;
            lblXDomain.Location = new System.Drawing.Point(7, 24);
            lblXDomain.Margin = new Padding(5, 0, 0, 0);
            lblXDomain.Name = "lblXDomain";
            lblXDomain.Size = new Size(75, 20);
            lblXDomain.TabIndex = 1;
            lblXDomain.Text = "X-Domain";
            lblXDomain.TextAlign = ContentAlignment.MiddleCenter;
            txtXDomain.Dock = DockStyle.Fill;
            txtXDomain.Location = new System.Drawing.Point(84, 24);
            txtXDomain.Margin = new Padding(0);
            txtXDomain.Name = "txtXDomain";
            txtXDomain.Size = new Size(156, 20);
            txtXDomain.TabIndex = 2;
            txtXDomain.Text = "0.0 To 1.0";
            txtXDomain.TextAlign = HorizontalAlignment.Center;
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(248, 173);
            Controls.Add(tblControls);
            Controls.Add(pnlButtons);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(245, 185);
            Name = nameof(GH_MDSliderDialog);
            Padding = new Padding(3, 3, 3, 3);
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.Manual;
            Text = "Multi-dimensional slider properties";
            pnlButtons.ResumeLayout(false);
            tblControls.ResumeLayout(false);
            tblControls.PerformLayout();
            ResumeLayout(false);
        }

        [field: AccessedThroughProperty("pnlButtons")]
        internal virtual Panel pnlButtons { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("btnOK")]
        internal virtual Button btnOK { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("btnCancel")]
        internal virtual Button btnCancel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("tblControls")]
        internal virtual TableLayoutPanel tblControls { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("lblXDomain")]
        internal virtual Label lblXDomain { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        internal virtual TextBox txtXDomain
        {
            get => _txtXDomain;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = new EventHandler(txtXDomain_TextChanged);
                TextBox txtXdomain1 = _txtXDomain;
                if (txtXdomain1 != null)
                    txtXdomain1.TextChanged -= eventHandler;
                _txtXDomain = value;
                TextBox txtXdomain2 = _txtXDomain;
                if (txtXdomain2 == null)
                    return;
                txtXdomain2.TextChanged += eventHandler;
            }
        }

        internal virtual TextBox txtZDomain
        {
            get => _txtZDomain;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = new EventHandler(txtXDomain_TextChanged);
                TextBox txtZdomain1 = _txtZDomain;
                if (txtZdomain1 != null)
                    txtZdomain1.TextChanged -= eventHandler;
                _txtZDomain = value;
                TextBox txtZdomain2 = _txtZDomain;
                if (txtZdomain2 == null)
                    return;
                txtZdomain2.TextChanged += eventHandler;
            }
        }

        [field: AccessedThroughProperty("lblZDomain")]
        internal virtual Label lblZDomain { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        internal virtual TextBox txtYDomain
        {
            get => _txtYDomain;
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                EventHandler eventHandler = new EventHandler(txtXDomain_TextChanged);
                TextBox txtYdomain1 = _txtYDomain;
                if (txtYdomain1 != null)
                    txtYdomain1.TextChanged -= eventHandler;
                _txtYDomain = value;
                TextBox txtYdomain2 = _txtYDomain;
                if (txtYdomain2 == null)
                    return;
                txtYdomain2.TextChanged += eventHandler;
            }
        }

        [field: AccessedThroughProperty("lblYDomain")]
        internal virtual Label lblYDomain { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        public void LoadSettings(GH_MultiDimensionalSlider owner)
        {
            
            txtXDomain.Text = GH_Format.FormatInterval(owner.XInterval);
            txtYDomain.Text = GH_Format.FormatInterval(owner.YInterval);
            txtZDomain.Text = GH_Format.FormatInterval(owner.ZInterval);
            
            lblZDomain.Enabled = txtZDomain.Enabled;
        }

        public void SaveSettings(GH_MultiDimensionalSlider owner)
        {
            
            Interval rc = Interval.Unset;
            if (GH_Convert.ToInterval(txtXDomain.Text, ref rc, GH_Conversion.Secondary))
                owner.XInterval = rc;
            if (GH_Convert.ToInterval(txtYDomain.Text, ref rc, GH_Conversion.Secondary))
                owner.YInterval = rc;
            if (!GH_Convert.ToInterval(txtZDomain.Text, ref rc, GH_Conversion.Secondary))
                return;
            owner.ZInterval = rc;
        }

        private void txtXDomain_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.TextLength == 0)
            {
                textBox.ForeColor = ForeColor;
            }
            else
            {
                Interval rc = Interval.Unset;
                if (GH_Convert.ToInterval(textBox.Text, ref rc, GH_Conversion.Secondary))
                    textBox.ForeColor = ForeColor;
                else
                    textBox.ForeColor = Color.Red;
            }
        }
    }
}
