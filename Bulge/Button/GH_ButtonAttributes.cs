// Decompiled with JetBrains decompiler
// Type: Grasshopper.Kernel.Special.GH_ButtonObjectAttributes
// Assembly: Grasshopper, Version=7.33.23248.13001, Culture=neutral, PublicKeyToken=dda4f5ec2cd80803
// MVID: 5BB20879-33FF-4DFC-95F1-EEC62DAC6ECD
// Assembly location: C:\Program Files\Rhino 7\Plug-ins\Grasshopper\Grasshopper.dll
// XML documentation location: C:\Program Files\Rhino 7\Plug-ins\Grasshopper\Grasshopper.xml

using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Bulge
{
    /// <exclude />
    public class GH_ButtonAttributes : GH_Attributes<GH_Button>
    {
        private RectangleF m_button;
        private RectangleF m_name;
        private bool m_captured;

        public GH_ButtonAttributes(GH_Button owner)
          : base(owner)
        {
            this.PerformLayout();
            this.m_captured = false;
        }

        protected override void Layout()
        {
            this.Pivot = (PointF)GH_Convert.ToPoint(this.Pivot);
            int num = GH_FontServer.StringWidth(this.Owner.NickName, GH_FontServer.StandardAdjusted);
            int width = 50;
            this.m_name = new RectangleF(this.Pivot, (SizeF)new Size(num + 16, 22));
            this.m_button = new RectangleF(this.m_name.Right, this.m_name.Top, (float)width, this.m_name.Height);
            this.Bounds = RectangleF.Union(this.m_name, this.m_button);
            this.m_name.Inflate(-2f, -2f);
            this.m_button.Inflate(-2f, -2f);
        }

        public override bool HasInputGrip => true;

        public override bool TooltipEnabled => true;

        public override void SetupTooltip(PointF point, GH_TooltipDisplayEventArgs e)
        {
            if (this.m_button.Contains(point))
            {
                e.Icon = this.Owner.Icon_24x24;
                e.Title = "Button";
                e.Text = "Click here to toggle the Button state";
                e.Description = string.Format("Normal:  {0}", (object)this.Owner.ExpressionNormal) + Environment.NewLine + string.Format("Pressed: {0}", (object)this.Owner.ExpressionPressed);
            }
            else
                base.SetupTooltip(point, e);
        }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            GH_ObjectResponse mouseDown;
            if (e.Button == MouseButtons.Left && (double)sender.Viewport.Zoom >= 0.5 && this.m_button.Contains(e.CanvasLocation))
            {
                this.m_captured = true;
                this.Owner.ButtonDown = true;
                this.Owner.ExpireSolution(true);
                mouseDown = GH_ObjectResponse.Capture;
            }
            else
                mouseDown = base.RespondToMouseDown(sender, e);
            return mouseDown;
        }

        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            GH_ObjectResponse mouseUp;
            if (this.Owner.ButtonDown | this.m_captured)
            {
                this.m_captured = false;
                this.Owner.ButtonDown = false;
                this.Owner.ExpireSolution(true);
                mouseUp = GH_ObjectResponse.Release;
            }
            else
                mouseUp = base.RespondToMouseUp(sender, e);
            return mouseUp;
        }

        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            GH_ObjectResponse mouseMove;
            if (this.m_captured & e.Button != MouseButtons.Left)
            {
                this.m_captured = false;
                this.Owner.ButtonDown = false;
                this.Owner.ExpireSolution(true);
                mouseMove = GH_ObjectResponse.Release;
            }
            else
                mouseMove = base.RespondToMouseMove(sender, e);
            return mouseMove;
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel != GH_CanvasChannel.Objects)
                return;
            GH_Viewport viewport = canvas.Viewport;
            RectangleF bounds = this.Bounds;
            ref RectangleF local = ref bounds;
            int num = viewport.IsVisible(ref local, 10f) ? 1 : 0;
            this.Bounds = bounds;
            if (num == 0)
                return;
            int zoomFadeLow = GH_Canvas.ZoomFadeLow;
            GH_PaletteStyle impliedStyle = GH_CapsuleRenderEngine.GetImpliedStyle(GH_Palette.Normal, this.Selected, this.Owner.Locked, true);
            GH_PaletteStyle paletteBlackStandard = GH_Skin.palette_black_standard;
            GH_Capsule capsule1 = GH_Capsule.CreateCapsule(this.Bounds, GH_Palette.Normal, 25, 0);
            capsule1.Font = GH_FontServer.StandardAdjusted;
            capsule1.AddInputGrip(this.InputGrip.Y);
            capsule1.AddInputGrip(this.InputGrip.Y + 20);
            capsule1.AddOutputGrip(this.OutputGrip.Y);
            Image icon = this.Owner.Icon_24x24;
            //capsule1.Render(graphics, this.Selected, this.Owner.Locked, true);
            //capsule1.Render(graphics, GH_Skin.palette_hidden_standard);
            capsule1.RenderEngine.RenderGrips(graphics);
            capsule1.RenderEngine.RenderBackground(graphics, canvas.Viewport.Zoom, GH_Skin.palette_hidden_standard);
            Utility_Render.RenderOutlinesSimple(capsule1, graphics, GH_Skin.palette_hidden_standard);
            capsule1.Dispose();
            if (this.Owner.NickName.Length > 0)
            {
                canvas.SetSmartTextRenderingHint();
                SolidBrush solidBrush = new SolidBrush(Color.FromArgb(zoomFadeLow, impliedStyle.Text));
                graphics.DrawString(this.Owner.NickName, GH_FontServer.StandardAdjusted, solidBrush, this.m_name, GH_TextRenderingConstants.CenterCenter);
                solidBrush.Dispose();


            }
            GH_Capsule capsule2 = GH_Capsule.CreateCapsule(this.m_button, GH_Palette.Black, 25, 0);
            capsule2.RenderEngine.RenderGrips(graphics);
            
            if (this.Owner.ButtonDown)
            {
                //LinearGradientBrush linearGradientBrush = new LinearGradientBrush(capsule2.Box, GH_GraphicsUtil.OffsetColour(paletteBlackStandard.Fill, 0), GH_GraphicsUtil.OffsetColour(paletteBlackStandard.Fill, 100), LinearGradientMode.Vertical);

                //graphics.FillPath(linearGradientBrush, capsule2.OutlineShape);
                //linearGradientBrush.Dispose();

                Brush solidBrush = new SolidBrush(Color.DarkGray);
                graphics.FillPath(solidBrush, capsule2.OutlineShape);
                solidBrush.Dispose();

            }
            else
            {
                //capsule2.RenderEngine.RenderBackground(graphics, canvas.Viewport.Zoom, paletteBlackStandard);
                //capsule2.RenderEngine.RenderHighlight(graphics);

                capsule2.RenderEngine.RenderBackground(graphics, canvas.Viewport.Zoom, paletteBlackStandard);

            }

            Utility_Render.RenderOutlinesSimple(capsule2, graphics, paletteBlackStandard);
            capsule2.Dispose();
        }

        

    }
}
