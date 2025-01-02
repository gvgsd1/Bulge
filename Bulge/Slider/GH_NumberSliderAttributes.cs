// Decompiled with JetBrains decompiler
// Type: Grasshopper.Kernel.Special.GH_NumberSliderAttributes
// Assembly: Grasshopper, Version=7.33.23248.13001, Culture=neutral, PublicKeyToken=dda4f5ec2cd80803
// MVID: 5BB20879-33FF-4DFC-95F1-EEC62DAC6ECD
// Assembly location: C:\Users\GV\Desktop\Grasshopper\Grasshopper.dll
// XML documentation location: C:\Users\GV\Desktop\Grasshopper\Grasshopper.xml

using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.GUI.Base;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Grasshopper.Kernel.Expressions;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Undo;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Bulge.Slider
{
    /// <exclude />
    public class GH_NumberSliderAttributes : GH_ResizableAttributes<GH_NumberSlider>
    {
        private static readonly int SliderMinWidth = 120;
        private int m_dragmode;
        private string m_cachedName;
        private Rectangle m_boundsName;
        private Rectangle m_boundsSlider;

        public GH_NumberSliderAttributes(GH_NumberSlider nOwner)
          : base(nOwner)
        {
            this.m_dragmode = 0;
            this.ExpireLayout();
            this.Layout();
        }

        protected override void Layout()
        {
            SizeF sizeF = (SizeF)GH_FontServer.MeasureString(this.Owner.ImpliedNickName, GH_FontServer.StandardAdjusted);
            sizeF.Width = Math.Max(sizeF.Width + 10f, (float)this.MinimumSize.Height);
            if (this.Owner.IsExpression)
                sizeF.Width += 16f;
            this.Bounds = new RectangleF(this.Pivot.X, this.Pivot.Y, Math.Max(this.Bounds.Width, sizeF.Width + (float)GH_NumberSliderAttributes.SliderMinWidth), (float)this.MinimumSize.Height);
            this.Bounds = (RectangleF)GH_Convert.ToRectangle(this.Bounds);
            PointF pivot = this.Pivot;
            double x = (double)pivot.X;
            pivot = this.Pivot;
            double y = (double)pivot.Y;
            double width = (double)sizeF.Width;
            double height = (double)this.MinimumSize.Height;
            this.m_boundsName = GH_Convert.ToRectangle(new RectangleF((float)x, (float)y, (float)width, (float)height));
            this.m_boundsSlider = Rectangle.FromLTRB(this.m_boundsName.Right, this.m_boundsName.Top, Convert.ToInt32(this.Bounds.Right), this.m_boundsName.Bottom);
            this.Owner.Slider.Font = GH_FontServer.StandardAdjusted;
            this.Owner.Slider.DrawControlBorder = false;
            this.Owner.Slider.DrawControlShadows = false;
            this.Owner.Slider.DrawControlBackground = false;
            this.Owner.Slider.TickCount = 11;
            this.Owner.Slider.TickFrequency = 5;
            this.Owner.Slider.RailDarkColour = Color.FromArgb(30, Color.Black);
            this.Owner.Slider.TickDisplay = GH_SliderTickDisplay.Simple;
            this.Owner.Slider.RailDisplay = GH_SliderRailDisplay.Simple;
            this.Owner.Slider.Padding = new Padding(6, 2, 6, 1);
            this.Owner.Slider.Bounds = this.m_boundsSlider;
        }

        public override GH_ObjectResponse RespondToMouseDoubleClick(
          GH_Canvas sender,
          GH_CanvasMouseEvent e)
        {
            GH_ObjectResponse mouseDoubleClick;
            if (e.Button == MouseButtons.Left)
            {
                if ((double)sender.Viewport.Zoom >= 0.9 && this.m_boundsSlider.Contains(GH_Convert.ToPoint(e.CanvasLocation)))
                {
                    string gripTextPure = this.Owner.Slider.GripTextPure;
                    if (this.Owner.IsExpression)
                    {
                        GH_ExpressionParser expressionParser = new GH_ExpressionParser(false);
                        try
                        {
                            expressionParser.CacheSymbols(this.Owner.Expression);
                            expressionParser.AddVariable("x", Convert.ToDouble(this.Owner.Slider.Value));
                            GH_Variant ghVariant = expressionParser.Evaluate();
                            if (ghVariant.IsNumeric)
                                gripTextPure = ghVariant.ToString();
                        }
                        catch (Exception ex)
                        {
                            ProjectData.SetProjectError(ex);
                            ProjectData.ClearProjectError();
                        }
                    }
                    this.Owner.Slider.TextInputHandlerDelegate = new GH_SliderBase.TextInputHandler(this.TextInputHandler);
                    this.Owner.Slider.ShowTextInputBox((Control)sender, true, sender.Viewport.XFormMatrix(GH_Viewport.GH_DisplayMatrix.CanvasToControl), gripTextPure);
                }
                else
                    this.Owner.PopupEditor();
                mouseDoubleClick = GH_ObjectResponse.Handled;
            }
            else
                mouseDoubleClick = GH_ObjectResponse.Ignore;
            return mouseDoubleClick;
        }

        private void TextInputHandler(GH_SliderBase slider, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;
            try
            {
                double destination;
                if (!GH_Convert.ToDouble((object)text, out destination, GH_Conversion.Secondary))
                    return;
                this.Owner.RecordUndoEvent("Slider Value Change");
                this.Owner.TrySetSliderValue(Convert.ToDecimal(destination));
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                ProjectData.ClearProjectError();
            }
        }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            GH_ObjectResponse mouseDown;
            if (this.Owner.Slider.MouseDown(e.WinFormsEventArgs, e.CanvasLocation))
            {
                this.m_dragmode = 1;
                sender.Invalidate();
                Rectangle rail = this.Owner.Slider.Rail;
                Decimal d2 = Decimal.Multiply(new Decimal(rail.Right - rail.Left), Convert.ToDecimal(sender.Viewport.Zoom));
                this.Owner.Slider.SnapDistance = Decimal.Multiply(Decimal.Divide(Decimal.Subtract(this.Owner.Slider.Maximum, this.Owner.Slider.Minimum), d2), 10M);
                mouseDown = GH_ObjectResponse.Capture;
            }
            else
                mouseDown = base.RespondToMouseDown(sender, e);
            return mouseDown;
        }

        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            GH_ObjectResponse mouseMove;
            switch (e.Button)
            {
                case MouseButtons.None:
                    if (this.Owner.Slider.Grip.Contains(e.CanvasLocation))
                    {
                        Instances.CursorServer.AttachCursor((Control)sender, "GH_NumericSlider");
                        mouseMove = GH_ObjectResponse.Handled;
                        break;
                    }
                    goto default;
                case MouseButtons.Left:
                    if (this.m_dragmode == 1)
                    {
                        this.Owner.TriggerAutoSave(this.Owner.InstanceGuid);
                        this.Owner.RecordUndoEvent("Slider change", (IGH_UndoAction)new GH_NumberSliderAttributes.SliderDragUndo(this.Owner));
                        this.m_dragmode = 2;
                    }
                    if (this.m_dragmode == 2)
                    {
                        this.Owner.Slider.MouseMove(e.WinFormsEventArgs, e.CanvasLocation);
                        mouseMove = GH_ObjectResponse.Handled;
                        break;
                    }
                    goto default;
                default:
                    mouseMove = base.RespondToMouseMove(sender, e);
                    break;
            }
            return mouseMove;
        }

        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            GH_ObjectResponse mouseUp;
            if (this.m_dragmode > 0)
            {
                this.m_dragmode = 0;
                this.Owner.Slider.MouseUp(e.WinFormsEventArgs, e.CanvasLocation);
                sender.Invalidate();
                mouseUp = GH_ObjectResponse.Release;
            }
            else
                mouseUp = base.RespondToMouseUp(sender, e);
            return mouseUp;
        }

        protected override Size MinimumSize => new Size(100, 20);

        protected override Size MaximumSize => new Size(5000, 20);

        protected override Padding SizingBorders => new Padding(0, 0, 6, 0);

        public override bool HasInputGrip => false;

        public override bool TooltipEnabled => true;

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel != GH_CanvasChannel.Objects)
                return;
            string impliedNickName = this.Owner.ImpliedNickName;
            if (this.m_cachedName == null || !impliedNickName.Equals(this.m_cachedName, StringComparison.Ordinal))
            {
                this.m_cachedName = impliedNickName;
                this.ExpireLayout();
                this.Layout();
            }
            GH_Viewport viewport = canvas.Viewport;
            RectangleF bounds1 = this.Bounds;
            ref RectangleF local = ref bounds1;
            int num = viewport.IsVisible(ref local, 10f) ? 1 : 0;
            this.Bounds = bounds1;
            if (num == 0)
                return;
            int[] radii1 = new int[4] { 3, 0, 0, 3 };
            int[] radii2 = new int[4] { 0, 3, 3, 0 };
            Rectangle textbox = this.m_boundsName;
            Rectangle rectangle = Rectangle.Empty;
            if (this.Owner.IsExpression)
            {
                //rectangle = this.m_boundsName with { Width = 20 };
                textbox = new Rectangle(textbox.X + 20, textbox.Y, textbox.Width - 20, textbox.Height);
            }
            GH_Capsule textCapsule;
            switch (this.Owner.RuntimeMessageLevel)
            {
                case GH_RuntimeMessageLevel.Warning:
                    textCapsule = GH_Capsule.CreateTextCapsule(this.m_boundsName, textbox, GH_Palette.Warning, impliedNickName, radii1, 5);
                    break;
                case GH_RuntimeMessageLevel.Error:
                    textCapsule = GH_Capsule.CreateTextCapsule(this.m_boundsName, textbox, GH_Palette.Error, impliedNickName, radii1, 5);
                    break;
                default:
                    textCapsule = GH_Capsule.CreateTextCapsule(this.m_boundsName, textbox, GH_Palette.Hidden, impliedNickName, radii1, 5);
                    break;
            }
            textCapsule.Render(graphics, this.Selected, this.Owner.Locked, true);
            textCapsule.Dispose();
            if (this.Owner.IsExpression && !rectangle.IsEmpty)
            {
                GH_StateTag_Expression stateTagExpression = new GH_StateTag_Expression(this.Owner.Expression);
                stateTagExpression.Stage = new Rectangle(rectangle.X + rectangle.Width / 2 - 8, rectangle.Y + rectangle.Height / 2 - 8, 16, 16);
                stateTagExpression.Render(graphics);
            }
            GH_Capsule capsule = GH_Capsule.CreateCapsule(this.m_boundsSlider, GH_Palette.Normal, radii2, 5);
            capsule.AddOutputGrip(this.OutputGrip.Y);
            capsule.Render(graphics, this.Selected, this.Owner.Locked, false);
            capsule.Dispose();
            if (!this.Owner.IsExpression || this.Owner.VolatileDataCount == 0)
                this.Owner.Slider.FormatMask = "{0}";
            else
                this.Owner.Slider.FormatMask = ((GH_Goo<double>)RuntimeHelpers.GetObjectValue(this.Owner.VolatileData.get_Branch(0)[0])).Value.ToString();
            int zoomFadeLow = GH_Canvas.ZoomFadeLow;
            if (zoomFadeLow > 0)
            {
                this.Owner.Slider.Render(graphics);
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                if (zoomFadeLow < (int)byte.MaxValue)
                {
                    GH_PaletteStyle impliedStyle = GH_CapsuleRenderEngine.GetImpliedStyle(GH_Palette.Normal, this.Selected, this.Owner.Locked, false);
                    SolidBrush solidBrush = new SolidBrush(Color.FromArgb((int)byte.MaxValue - zoomFadeLow, impliedStyle.Fill));
                    Rectangle bounds2 = this.Owner.Slider.Bounds;
                    bounds2.X += this.Owner.Slider.Padding.Left;
                    bounds2.Y += this.Owner.Slider.Padding.Top;
                    bounds2.Width -= this.Owner.Slider.Padding.Horizontal;
                    bounds2.Height -= this.Owner.Slider.Padding.Vertical;
                    graphics.FillRectangle((Brush)solidBrush, bounds2);
                    solidBrush.Dispose();
                }
            }
            if (this.m_dragmode <= 0 || !this.Owner.IsExpression)
                return;
            PointF location = this.Owner.Slider.Grip.Location;
            location.X += 0.5f * this.Owner.Slider.Grip.Width;
            RectangleF bounds3 = this.Bounds;
            bounds3.Inflate(1000f, 1000f);
            GH_GraphicsUtil.RenderBalloonTag(graphics, "Original: " + this.Owner.Slider.GripTextPure, GH_FontServer.StandardAdjusted, location, bounds3);
        }

        private class SliderDragUndo : GH_ObjectUndoAction
        {
            private Decimal _value;

            public SliderDragUndo(GH_NumberSlider owner)
              : base(owner.InstanceGuid)
            {
                this._value = owner.Slider.Value;
                this.ExpiresSolution = true;
            }

            /// <summary>
            /// Override this property if you want the Grasshopper solution to refresh upon undo completion.
            /// </summary>
            public override bool ExpiresSolution { get; }

            protected override void Object_Undo(GH_Document doc, IGH_DocumentObject obj)
            {
                if (!(obj is GH_NumberSlider ghNumberSlider))
                    return;
                Decimal num = ghNumberSlider.Slider.Value;
                ghNumberSlider.Slider.Value = this._value;
                this._value = num;
            }

            protected override void Object_Redo(GH_Document doc, IGH_DocumentObject obj) => this.Object_Undo(doc, obj);
        }
    }
}
