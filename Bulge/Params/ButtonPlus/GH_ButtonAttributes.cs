// Decompiled with JetBrains decompiler
// Type: Grasshopper.Kernel.Special.GH_ButtonObjectAttributes
// Assembly: Grasshopper, Version=7.33.23248.13001, Culture=neutral, PublicKeyToken=dda4f5ec2cd80803
// MVID: 5BB20879-33FF-4DFC-95F1-EEC62DAC6ECD
// Assembly location: C:\Program Files\Rhino 7\Plug-ins\Grasshopper\Grasshopper.dll
// XML documentation location: C:\Program Files\Rhino 7\Plug-ins\Grasshopper\Grasshopper.xml

using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Bulge.Params.ButtonPlus
{
    /// <exclude />
    public class GH_ButtonAttributes : GH_Attributes<GH_Button>
    {
        private RectangleF rec_button;
        private RectangleF rec_words;
        private bool m_captured;

        public GH_ButtonAttributes(GH_Button owner)
          : base(owner)
        {
            PerformLayout();
            m_captured = false;
        }



        //public override bool HasInputGrip => true;

        //public override bool TooltipEnabled => true;

        public override void AppendToAttributeTree(List<IGH_Attributes> attributes)
        {
            attributes.Add((IGH_Attributes)this);
            foreach (IGH_Param ghParam in this.Owner.Params.Input)
                attributes.Add(ghParam.Attributes);
            foreach (IGH_Param ghParam in this.Owner.Params.Output)
                attributes.Add(ghParam.Attributes);
        }

        public override void SetupTooltip(PointF point, GH_TooltipDisplayEventArgs e)
        {
            if (rec_button.Contains(point))
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
            if (e.Button == MouseButtons.Left && (double)sender.Viewport.Zoom >= 0.5 && rec_button.Contains(e.CanvasLocation))
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

        protected override void Layout()
        {
            Pivot = (PointF)GH_Convert.ToPoint(Pivot);
            int num = GH_FontServer.StringWidth(Owner.NickName, GH_FontServer.StandardAdjusted);
            int width = 50;

            rec_words = new RectangleF(Pivot, new SizeF(num + 16, 22));



            rec_button = new RectangleF(rec_words.Right, rec_words.Top, width, rec_words.Height);
            Bounds = RectangleF.Union(rec_words, rec_button);
            var Bounds2 = Bounds;
            Bounds2.X += 15;
            Bounds2.Width -= 35;
            GH_ComponentAttributes.LayoutInputParams(this.Owner, Bounds2);
            GH_ComponentAttributes.LayoutOutputParams(this.Owner, Bounds2);

            rec_words.Inflate(-2f, -2f);
            rec_button.Inflate(-2f, -2f);
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            switch (channel)
            {
                case GH_CanvasChannel.Wires:

                    foreach (IGH_DocumentObject ghDocumentObject in this.Owner.Params.Input)
                        ghDocumentObject.Attributes.RenderToCanvas(canvas, GH_CanvasChannel.Wires);

                    //if (this.Owner.SourceCount <= 0)
                    //    break;
                    //this.RenderIncomingWires(canvas.Painter, this.Owner.Sources, this.Owner.WireDisplay);
                    break;

                case GH_CanvasChannel.Objects:
                    GH_Viewport viewport = canvas.Viewport;
                    RectangleF bounds = Bounds;
                    ref RectangleF local = ref bounds;
                    int num = viewport.IsVisible(ref local, 10f) ? 1 : 0;
                    Bounds = bounds;
                    if (num == 0)
                        return;
                    int zoomFadeLow = GH_Canvas.ZoomFadeLow;
                    GH_PaletteStyle impliedStyle = GH_CapsuleRenderEngine.GetImpliedStyle(GH_Palette.Normal, Selected, Owner.Locked, true);
                    GH_PaletteStyle paletteBlackStandard = GH_Skin.palette_black_standard;
                    GH_Capsule capsule_bounds = GH_Capsule.CreateCapsule(Bounds, GH_Palette.Normal, 25, 0);
                    capsule_bounds.Font = GH_FontServer.StandardAdjusted;
                    capsule_bounds.AddInputGrip(InputGrip.Y);
                    capsule_bounds.AddInputGrip(InputGrip.Y + 20);
                    capsule_bounds.AddOutputGrip(OutputGrip.Y);
                    Image icon = Owner.Icon_24x24;
                    //capsule_bounds.Render(graphics, this.Selected, this.Owner.Locked, true);
                    //capsule_bounds.Render(graphics, GH_Skin.palette_hidden_standard);
                    //capsule_bounds.RenderEngine.RenderGrips(graphics);
                    //capsule_bounds.RenderEngine.RenderBackground(graphics, canvas.Viewport.Zoom, GH_Skin.palette_hidden_standard);
                    //Utility.UtilityRender.RenderOutlinesSimple(capsule_bounds, graphics, GH_Skin.palette_hidden_standard);
                    Utility.UtilityRender.Render(capsule_bounds, graphics, null, this.Selected, this.Owner.Locked, true);
                    capsule_bounds.Dispose();
                    if (Owner.NickName.Length > 0)
                    {
                        canvas.SetSmartTextRenderingHint();
                        SolidBrush solidBrush = new SolidBrush(Color.FromArgb(zoomFadeLow, impliedStyle.Text));
                        graphics.DrawString(Owner.NickName, GH_FontServer.StandardAdjusted, solidBrush, rec_words, GH_TextRenderingConstants.CenterCenter);
                        solidBrush.Dispose();


                    }
                    GH_Capsule capsule_button = GH_Capsule.CreateCapsule(rec_button, GH_Palette.Black, 25, 0);
                    capsule_button.RenderEngine.RenderGrips(graphics);

                    if (Owner.ButtonDown)
                    {
                        //LinearGradientBrush linearGradientBrush = new LinearGradientBrush(capsule_button.Box, GH_GraphicsUtil.OffsetColour(paletteBlackStandard.Fill, 0), GH_GraphicsUtil.OffsetColour(paletteBlackStandard.Fill, 100), LinearGradientMode.Vertical);

                        //graphics.FillPath(linearGradientBrush, capsule_button.OutlineShape);
                        //linearGradientBrush.Dispose();

                        Brush solidBrush = new SolidBrush(Color.DarkGray);
                        graphics.FillPath(solidBrush, capsule_button.OutlineShape);
                        solidBrush.Dispose();

                    }
                    else
                    {
                        //capsule_button.RenderEngine.RenderBackground(graphics, canvas.Viewport.Zoom, paletteBlackStandard);
                        //capsule_button.RenderEngine.RenderHighlight(graphics);

                        capsule_button.RenderEngine.RenderBackground(graphics, canvas.Viewport.Zoom, paletteBlackStandard);

                    }

                    Utility.UtilityRender.RenderOutlinesSimple(capsule_button, graphics, paletteBlackStandard);
                    capsule_button.Dispose();

                    break;
            }
        }

    }
}
