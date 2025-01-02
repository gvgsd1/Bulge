

using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

#nullable disable
namespace Bulge.Params.NumberSliderPlus
{
    [DesignerGenerated]
    public class GH_NumberSliderSnappingEditor : Form
    {
        private IContainer components;

        public GH_NumberSliderSnappingEditor() => this.InitializeComponent();

        [DebuggerNonUserCode]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!disposing || this.components == null)
                    return;
                this.components.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            this.ButtonPanel = new Panel();
            this.OkButton = new Button();
            this.SnappingTextBox = new TextBox();
            this.ButtonPanel.SuspendLayout();
            this.SuspendLayout();
            this.ButtonPanel.Controls.Add((Control)this.OkButton);
            this.ButtonPanel.Dock = DockStyle.Bottom;
            this.ButtonPanel.Location = new Point(0, 191);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Padding = new Padding(0, 8, 0, 0);
            this.ButtonPanel.Size = new Size(238, 32);
            this.ButtonPanel.TabIndex = 0;
            this.OkButton.DialogResult = DialogResult.OK;
            this.OkButton.Dock = DockStyle.Right;
            this.OkButton.Location = new Point(158, 8);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new Size(80, 24);
            this.OkButton.TabIndex = 0;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.SnappingTextBox.Dock = DockStyle.Fill;
            this.SnappingTextBox.Location = new Point(0, 0);
            this.SnappingTextBox.Multiline = true;
            this.SnappingTextBox.Name = "SnappingTextBox";
            this.SnappingTextBox.Size = new Size(238, 191);
            this.SnappingTextBox.TabIndex = 1;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(238, 223);
            this.Controls.Add((Control)this.SnappingTextBox);
            this.Controls.Add((Control)this.ButtonPanel);
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.Name = nameof(GH_NumberSliderSnappingEditor);
            this.StartPosition = FormStartPosition.Manual;
            this.Text = "Slider snapping values";
            this.ButtonPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        [field: AccessedThroughProperty("ButtonPanel")]
        internal virtual Panel ButtonPanel { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("OkButton")]
        internal virtual Button OkButton { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        [field: AccessedThroughProperty("SnappingTextBox")]
        internal virtual TextBox SnappingTextBox { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> SnapLines
        {
            get
            {
                List<string> snapLines = new List<string>();
                string[] lines = this.SnappingTextBox.Lines;
                int index = 0;
                while (index < lines.Length)
                {
                    string str = lines[index];
                    if (!string.IsNullOrWhiteSpace(str))
                        snapLines.Add(str);
                    checked { ++index; }
                }
                return snapLines;
            }
        }
    }
}
