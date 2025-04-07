
using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Data;
using Grasshopper.My.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Grasshopper.Kernel.Special
{

    public class PathMapperAttributes : GH_ResizableAttributes<PathMapper>
    {
        private GH_Capsule _capsule;

        public PathMapperAttributes(PathMapper owner)
          : base(owner)
        {
            Bounds = (RectangleF)new Rectangle(0, 0, 125, 40);
        }

        public override GH_ObjectResponse RespondToMouseDoubleClick(
          GH_Canvas sender,
          GH_CanvasMouseEvent e)
        {
            GH_ObjectResponse mouseDoubleClick;
            if (e.Button == MouseButtons.Left && Bounds.Contains(e.CanvasLocation))
            {
                Owner.DisplayLexerEditor();
                mouseDoubleClick = GH_ObjectResponse.Handled;
            }
            else
                mouseDoubleClick = base.RespondToMouseDoubleClick(sender, e);
            return mouseDoubleClick;
        }

        protected override Size MinimumSize => new Size(100, 40);

        protected override Padding SizingBorders => new Padding(6, 0, 6, 0);

        public override void ExpireLayout()
        {
            base.ExpireLayout();
            if (_capsule == null)
                return;
            _capsule.Dispose();
            _capsule = null;
        }

        protected override void Layout()
        {
            int height = Owner.Lexers.Count * 22 + 12 + 6;
            if (height < MinimumSize.Height)
                height = MinimumSize.Height;
            Bounds = new RectangleF(Pivot, (SizeF)new Size(Convert.ToInt32(Bounds.Width), height));
        }

        protected override void PrepareForRender(GH_Canvas canvas)
        {
            if (_capsule != null)
                return;
            GH_Palette palette = GH_Palette.Hidden;
            switch (Owner.RuntimeMessageLevel)
            {
                case GH_RuntimeMessageLevel.Warning:
                    palette = GH_Palette.Warning;
                    break;
                case GH_RuntimeMessageLevel.Error:
                    palette = GH_Palette.Error;
                    break;
            }
            _capsule = GH_Capsule.CreateCapsule(Bounds, palette, 5, 20);
            _capsule.AddInputGrip(InputGrip.Y);
            _capsule.AddOutputGrip(OutputGrip.Y);
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel == GH_CanvasChannel.Wires)
                RenderIncomingWires(canvas.Painter, Owner.Sources, Owner.WireDisplay);
            if (channel != GH_CanvasChannel.Objects)
                return;
            GH_Viewport viewport = canvas.Viewport;
            RectangleF bounds = Bounds;
            ref RectangleF local = ref bounds;
            int num1 = viewport.IsVisible(ref local, 10f) ? 1 : 0;
            Bounds = bounds;
            if (num1 == 0)
                return;
            _capsule.Render(graphics, Selected, Owner.Locked, true);
            Rectangle rectangle1 = GH_Convert.ToRectangle(Bounds);
            rectangle1.Inflate(-6, -6);
            SolidBrush solidBrush1 = new SolidBrush(Color.FromArgb(50, Color.White));
            graphics.FillRectangle((Brush)solidBrush1, rectangle1);
            solidBrush1.Dispose();
            if (Owner.Lexers.Count == 0)
            {
                graphics.SetClip(rectangle1);
                bounds = Bounds;
                double x = (double)bounds.X;
                bounds = Bounds;
                double num2 = 0.5 * (double)bounds.Width;
                int int32_1 = Convert.ToInt32(x + num2);
                bounds = Bounds;
                double y = (double)bounds.Y;
                bounds = Bounds;
                double num3 = 0.5 * (double)bounds.Height;
                int int32_2 = Convert.ToInt32(y + num3);
                Rectangle frame = Rectangle.Intersect(Rectangle.FromLTRB(int32_1 - 12, int32_2 - 12, int32_1 + 12, int32_2 + 12), rectangle1);

                GH_GraphicsUtil.RenderIcon(graphics, (RectangleF)frame, UtilityIcon.ResizeIcon(Resources.IconFirstLastTreeBranch));
                //GH_GraphicsUtil.RenderIcon(graphics, (RectangleF)frame, (Image)Res_SignalIcons.Warning_24x24);
                graphics.ResetClip();
            }
            else if ((double)canvas.Viewport.Zoom > 0.5)
            {
                canvas.SetSmartTextRenderingHint();
                Color color = GH_GraphicsUtil.FadeColour(0.5, 0.8, (double)canvas.Viewport.Zoom, Color.Black);
                SolidBrush solidBrush2 = new SolidBrush(color);
                SolidBrush solidBrush3 = new SolidBrush(Color.FromArgb(color.A, Color.White));
                Pen pen = new Pen(color);
                graphics.SetClip(rectangle1);
                Rectangle rectangle2 = rectangle1;
                rectangle2.Inflate(-4, -4);
                int num4 = rectangle1.X + rectangle2.Width / 2;
                int y = rectangle2.Y;

                    foreach (GH_LexerCombo lexer in Owner.Lexers)
                    {
                        SizeF sizeF1 = (SizeF)GH_FontServer.MeasureString(lexer.Source.Mask, GH_FontServer.ConsoleSmallAdjusted);
                        SizeF sizeF2 = (SizeF)GH_FontServer.MeasureString(lexer.Target.Mask, GH_FontServer.ConsoleSmallAdjusted);
                        int int32_3 = Convert.ToInt32(sizeF1.Width + 4f);
                        int int32_4 = Convert.ToInt32(sizeF2.Width + 4f);
                        Rectangle rectangle3 = new Rectangle(rectangle2.X, y, int32_3, 20);
                        Rectangle rectangle4 = new Rectangle(rectangle2.Right - int32_4, y, int32_4, 20);
                        if (rectangle3.Width + rectangle4.Width + 10 > rectangle2.Width)
                        {
                            int val2 = Math.Min(rectangle3.Right, num4 - 5);
                            rectangle3 = Rectangle.FromLTRB(rectangle3.X, rectangle3.Y, Math.Max(rectangle4.Left - 10, val2), rectangle3.Bottom);
                            rectangle4 = Rectangle.FromLTRB(rectangle3.Right + 10, rectangle4.Y, rectangle4.Right, rectangle4.Bottom);
                        }
                        DrawMappingArrow(graphics, canvas.Viewport.Zoom, rectangle3, rectangle4);
                        graphics.FillRectangle(solidBrush3, rectangle3);
                        graphics.FillRectangle(solidBrush3, rectangle4);
                        graphics.DrawRectangle(pen, rectangle3);
                        graphics.DrawRectangle(pen, rectangle4);
                        graphics.DrawString(lexer.Source.Mask, GH_FontServer.ConsoleSmallAdjusted, solidBrush2, (RectangleF)rectangle3, GH_TextRenderingConstants.CenterCenter);
                        graphics.DrawString(lexer.Target.Mask, GH_FontServer.ConsoleSmallAdjusted, solidBrush2, (RectangleF)rectangle4, GH_TextRenderingConstants.CenterCenter);
                        y += 22;
                    }

                graphics.ResetClip();
                solidBrush2.Dispose();
                solidBrush3.Dispose();
                pen.Dispose();
            }
            GH_GraphicsUtil.ShadowRectangle(graphics, rectangle1);
            graphics.DrawRectangle(Pens.Black, rectangle1);
        }

        private void DrawMappingArrow(Graphics graphics, float zoom, Rectangle A, Rectangle B)
        {
            Pen pen1 = new Pen(Color.FromArgb(GH_GraphicsUtil.BlendInteger(0.5, 0.8, 0, 35, (double)zoom), Color.Black));
            Pen pen2 = new Pen(Color.FromArgb(GH_GraphicsUtil.BlendInteger(0.5, 0.8, 0, 100, (double)zoom), Color.Black));
            Pen pen3 = new Pen(Color.FromArgb(GH_GraphicsUtil.BlendInteger(0.5, 0.8, 0, 150, (double)zoom), Color.White));
            int right = A.Right;
            int left = B.Left;
            int num1 = A.Top + A.Height / 2;
            graphics.DrawLine(pen2, right, num1 - 1, left, num1 - 1);
            graphics.DrawLine(pen1, right, num1, left, num1);
            graphics.DrawLine(pen3, right, num1 + 1, left, num1 + 1);
            pen1.Dispose();
            pen2.Dispose();
            pen3.Dispose();
            int num2 = (right + left) / 2;

            Bitmap mappingOperator14x15 = UtilityIcon.ResizeIcon(Resources.IconFirstLastTreeBranch);
            
            //Bitmap mappingOperator14x15 = Res_GUI.Mapping_Operator_14x15;
            graphics.DrawImage(mappingOperator14x15, num2 - mappingOperator14x15.Width / 2, num1 - mappingOperator14x15.Height / 2, mappingOperator14x15.Width, mappingOperator14x15.Height);
            mappingOperator14x15.Dispose();
        }
    }
}
