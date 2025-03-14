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

namespace Bulge.Params.ButtonPlus
{
    /// <exclude />
    public class GH_ButtonObjectAttributes : GH_Attributes<GH_ButtonObject>
    {
        private RectangleF m_button;
        private RectangleF m_name;
        private bool m_captured;

        public GH_ButtonObjectAttributes(GH_ButtonObject owner)
          : base(owner)
        {
            PerformLayout();
            m_captured = false;
        }

        protected override void Layout()
        {
            Pivot = (PointF)GH_Convert.ToPoint(Pivot);
            int num = GH_FontServer.StringWidth(Owner.NickName, GH_FontServer.StandardAdjusted);
            int width = 50;
            m_name = new RectangleF(Pivot, (SizeF)new Size(num + 16, 22));
            m_button = new RectangleF(m_name.Right, m_name.Top, width, m_name.Height);
            Bounds = RectangleF.Union(m_name, m_button);
            m_name.Inflate(-2f, -2f);
            m_button.Inflate(-2f, -2f);
        }

        public override bool HasInputGrip => false;

        public override bool TooltipEnabled => true;

        public override void SetupTooltip(PointF point, GH_TooltipDisplayEventArgs e)
        {
            if (m_button.Contains(point))
            {
                e.Icon = Owner.Icon_24x24;
                e.Title = "Button";
                e.Text = "Click here to toggle the Button state";
                e.Description = string.Format("Normal:  {0}", Owner.ExpressionNormal) + Environment.NewLine + string.Format("Pressed: {0}", Owner.ExpressionPressed);
            }
            else
                base.SetupTooltip(point, e);
        }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            GH_ObjectResponse mouseDown;
            if (e.Button == MouseButtons.Left && (double)sender.Viewport.Zoom >= 0.5 && m_button.Contains(e.CanvasLocation))
            {
                m_captured = true;
                Owner.ButtonDown = true;
                Owner.ExpireSolution(true);
                mouseDown = GH_ObjectResponse.Capture;
            }
            else
                mouseDown = base.RespondToMouseDown(sender, e);
            return mouseDown;
        }

        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            GH_ObjectResponse mouseUp;
            if (Owner.ButtonDown | m_captured)
            {
                m_captured = false;
                Owner.ButtonDown = false;
                Owner.ExpireSolution(true);
                mouseUp = GH_ObjectResponse.Release;
            }
            else
                mouseUp = base.RespondToMouseUp(sender, e);
            return mouseUp;
        }

        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            GH_ObjectResponse mouseMove;
            if (m_captured & e.Button != MouseButtons.Left)
            {
                m_captured = false;
                Owner.ButtonDown = false;
                Owner.ExpireSolution(true);
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
            RectangleF bounds = Bounds;
            ref RectangleF local = ref bounds;
            int num = viewport.IsVisible(ref local, 10f) ? 1 : 0;
            Bounds = bounds;
            if (num == 0)
                return;
            int zoomFadeLow = GH_Canvas.ZoomFadeLow;
            GH_PaletteStyle impliedStyle = GH_CapsuleRenderEngine.GetImpliedStyle(GH_Palette.Normal, Selected, Owner.Locked, true);
            GH_Capsule capsule1 = GH_Capsule.CreateCapsule(Bounds, GH_Palette.Normal);
            capsule1.Font = GH_FontServer.StandardAdjusted;
            capsule1.AddOutputGrip(OutputGrip.Y);
            capsule1.Render(graphics, Selected, Owner.Locked, true);
            capsule1.Dispose();
            if (Owner.NickName.Length > 0)
            {
                canvas.SetSmartTextRenderingHint();
                SolidBrush solidBrush = new SolidBrush(Color.FromArgb(zoomFadeLow, impliedStyle.Text));
                graphics.DrawString(Owner.NickName, GH_FontServer.StandardAdjusted, solidBrush, m_name, GH_TextRenderingConstants.CenterCenter);
                solidBrush.Dispose();
            }
            GH_Capsule capsule2 = GH_Capsule.CreateCapsule(m_button, GH_Palette.Black, 1, 9);
            capsule2.RenderEngine.RenderGrips(graphics);
            GH_PaletteStyle paletteBlackStandard = GH_Skin.palette_black_standard;
            if (Owner.ButtonDown)
            {
                LinearGradientBrush linearGradientBrush = new LinearGradientBrush(capsule2.Box, GH_GraphicsUtil.OffsetColour(paletteBlackStandard.Fill, 0), GH_GraphicsUtil.OffsetColour(paletteBlackStandard.Fill, 100), LinearGradientMode.Vertical);
                graphics.FillPath(linearGradientBrush, capsule2.OutlineShape);
                linearGradientBrush.Dispose();
            }
            else
            {
                capsule2.RenderEngine.RenderBackground(graphics, canvas.Viewport.Zoom, paletteBlackStandard);
                capsule2.RenderEngine.RenderHighlight(graphics);
            }
            capsule2.RenderEngine.RenderOutlines(graphics, canvas.Viewport.Zoom, paletteBlackStandard);
            capsule2.Dispose();
        }
    }
}
