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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Bulge.Params.NumberSliderPlus
{
    /// <exclude />
    public class GH_NumberSliderAttributes : GH_ResizableAttributes<GH_NumberSlider>
    {
        private static readonly int SliderMinWidth = 120;
        private int m_dragmode;
        private string m_cachedName;
        private Rectangle rec_words;
        private Rectangle rec_slider;
        private RectangleF rec_input_words;

        public GH_NumberSliderAttributes(GH_NumberSlider nOwner)
          : base(nOwner)
        {
            m_dragmode = 0;
            ExpireLayout();
            Layout();
        }
        //public override void AppendToAttributeTree(List<IGH_Attributes> attributes)
        //{
        //    attributes.Add((IGH_Attributes)this);
        //    //foreach (IGH_Param ghParam in this.Owner.Params.Input)
        //    //    attributes.Add(ghParam.Attributes);
        //    //foreach (IGH_Param ghParam in this.Owner.Params.Output)
        //    //    attributes.Add(ghParam.Attributes);
        //}

        protected override void Layout()
        {
            SizeF sizeF = (SizeF)GH_FontServer.MeasureString(Owner.ImpliedNickName, GH_FontServer.StandardAdjusted);
            sizeF.Width = Math.Max(sizeF.Width + 10f, MinimumSize.Height);
            if (Owner.IsExpression)
                sizeF.Width += 16f;
            Bounds = new RectangleF(Pivot.X, Pivot.Y, Math.Max(Bounds.Width, sizeF.Width + SliderMinWidth), MinimumSize.Height);
            Bounds = (RectangleF)GH_Convert.ToRectangle(Bounds);
            PointF pivot = Pivot;
            double x = (double)pivot.X;
            pivot = Pivot;
            double y = (double)pivot.Y;
            double width = (double)sizeF.Width;
            double height = MinimumSize.Height;

            rec_input_words = new RectangleF(Pivot, new SizeF(22, 22));

            rec_words = GH_Convert.ToRectangle(new RectangleF((float)x, (float)y, (float)width, (float)height));
            rec_slider = Rectangle.FromLTRB(rec_words.Right, rec_words.Top, Convert.ToInt32(Bounds.Right), rec_words.Bottom);
            Owner.Slider.Font = GH_FontServer.StandardAdjusted;
            Owner.Slider.DrawControlBorder = false;
            Owner.Slider.DrawControlShadows = false;
            Owner.Slider.DrawControlBackground = false;
            Owner.Slider.TickCount = 11;
            Owner.Slider.TickFrequency = 5;
            Owner.Slider.RailDarkColour = Color.FromArgb(30, Color.Black);
            Owner.Slider.TickDisplay = GH_SliderTickDisplay.Simple;
            Owner.Slider.RailDisplay = GH_SliderRailDisplay.Simple;
            Owner.Slider.Padding = new Padding(6, 2, 6, 1);
            Owner.Slider.Bounds = rec_slider;
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            switch (channel)
            {
                case GH_CanvasChannel.Wires:
                    if (this.Owner.SourceCount <= 0)
                        break;
                    this.RenderIncomingWires(canvas.Painter, this.Owner.Sources, this.Owner.WireDisplay);
                    break;
                case GH_CanvasChannel.Objects:
                    string impliedNickName = Owner.ImpliedNickName;
                    if (m_cachedName == null || !impliedNickName.Equals(m_cachedName, StringComparison.Ordinal))
                    {
                        m_cachedName = impliedNickName;
                        ExpireLayout();
                        Layout();
                    }
                    GH_Viewport viewport = canvas.Viewport;
                    RectangleF bounds1 = Bounds;
                    ref RectangleF local = ref bounds1;
                    int num = viewport.IsVisible(ref local, 10f) ? 1 : 0;
                    Bounds = bounds1;
                    if (num == 0)
                        return;
                    //int[] radii1 = new int[4] { 3, 0, 0, 3 };
                    //int[] radii2 = new int[4] { 0, 3, 3, 0 };
                    Rectangle boundbox = rec_words;
                    boundbox.Width = rec_words.Width + 20;
                    Rectangle textbox = rec_words;
                    Rectangle rectangle = Rectangle.Empty;
                    if (Owner.IsExpression)
                    {
                        //rectangle = this.rec_words with { Width = 20 };
                        textbox = new Rectangle(textbox.X + 20, textbox.Y, textbox.Width - 20, textbox.Height);
                    }
                    GH_Capsule textCapsule;
                    switch (Owner.RuntimeMessageLevel)
                    {
                        case GH_RuntimeMessageLevel.Warning:
                            textCapsule = GH_Capsule.CreateTextCapsule(boundbox, textbox, GH_Palette.Warning, impliedNickName, 10, 0);
                            break;
                        case GH_RuntimeMessageLevel.Error:
                            textCapsule = GH_Capsule.CreateTextCapsule(boundbox, textbox, GH_Palette.Error, impliedNickName, 10, 0);
                            break;
                        default:
                            textCapsule = GH_Capsule.CreateTextCapsule(boundbox, textbox, GH_Palette.Hidden, impliedNickName, 10, 0);
                            break;
                    }
                    textCapsule.AddInputGrip(InputGrip.Y);
                    //textCapsule.Render(graphics, Selected, Owner.Locked, true);
                    Utility.UtilityRender.Render(textCapsule, graphics, null, Selected, Owner.Locked, true);
                    textCapsule.Dispose();
                    if (Owner.IsExpression && !rectangle.IsEmpty)
                    {
                        //GH_StateTag_Expression stateTagExpression = new GH_StateTag_Expression(Owner.Expression);

                        //stateTagExpression.Stage = new Rectangle(rectangle.X + rectangle.Width / 2 - 8, rectangle.Y + rectangle.Height / 2 - 8, 16, 16);

                        //stateTagExpression.Render(graphics);
                    }
                    GH_Capsule capsule = GH_Capsule.CreateCapsule(rec_slider, GH_Palette.Normal, 10, 0);
                    capsule.AddOutputGrip(OutputGrip.Y);
                    //capsule.Render(graphics, Selected, Owner.Locked, false);
                    Utility.UtilityRender.Render(capsule, graphics, null, Selected, Owner.Locked, false);
                    capsule.Dispose();
                    if (!Owner.IsExpression || Owner.VolatileDataCount == 0)
                        Owner.Slider.FormatMask = "{0}";
                    else
                        Owner.Slider.FormatMask = ((GH_Goo<double>)RuntimeHelpers.GetObjectValue(Owner.VolatileData.get_Branch(0)[0])).Value.ToString();
                    int zoomFadeLow = GH_Canvas.ZoomFadeLow;
                    if (zoomFadeLow > 0)
                    {
                        Owner.Slider.Render(graphics);
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        if (zoomFadeLow < byte.MaxValue)
                        {
                            GH_PaletteStyle impliedStyle = GH_CapsuleRenderEngine.GetImpliedStyle(GH_Palette.Normal, Selected, Owner.Locked, false);
                            SolidBrush solidBrush = new SolidBrush(Color.FromArgb(byte.MaxValue - zoomFadeLow, impliedStyle.Fill));
                            Rectangle bounds2 = Owner.Slider.Bounds;
                            bounds2.X += Owner.Slider.Padding.Left;
                            bounds2.Y += Owner.Slider.Padding.Top;
                            bounds2.Width -= Owner.Slider.Padding.Horizontal;
                            bounds2.Height -= Owner.Slider.Padding.Vertical;
                            graphics.FillRectangle(solidBrush, bounds2);
                            solidBrush.Dispose();
                        }
                    }
                    if (m_dragmode <= 0 || !Owner.IsExpression)
                        return;
                    PointF location = Owner.Slider.Grip.Location;
                    location.X += 0.5f * Owner.Slider.Grip.Width;
                    RectangleF bounds3 = Bounds;
                    bounds3.Inflate(1000f, 1000f);
                    GH_GraphicsUtil.RenderBalloonTag(graphics, "Original: " + Owner.Slider.GripTextPure, GH_FontServer.StandardAdjusted, location, bounds3);
                    break;
            }
        }

        public override GH_ObjectResponse RespondToMouseDoubleClick(
          GH_Canvas sender,
          GH_CanvasMouseEvent e)
        {
            GH_ObjectResponse mouseDoubleClick;
            if (e.Button == MouseButtons.Left)
            {
                if ((double)sender.Viewport.Zoom >= 0.9 && rec_slider.Contains(GH_Convert.ToPoint(e.CanvasLocation)))
                {
                    string gripTextPure = Owner.Slider.GripTextPure;
                    if (Owner.IsExpression)
                    {
                        GH_ExpressionParser expressionParser = new GH_ExpressionParser(false);
                        try
                        {
                            expressionParser.CacheSymbols(Owner.Expression);
                            expressionParser.AddVariable("x", Convert.ToDouble(Owner.Slider.Value));
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
                    Owner.Slider.TextInputHandlerDelegate = new GH_SliderBase.TextInputHandler(TextInputHandler);
                    Owner.Slider.ShowTextInputBox(sender, true, sender.Viewport.XFormMatrix(GH_Viewport.GH_DisplayMatrix.CanvasToControl), gripTextPure);
                }
                else
                    Owner.PopupEditor();
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
                if (!GH_Convert.ToDouble(text, out destination, GH_Conversion.Secondary))
                    return;
                Owner.RecordUndoEvent("Slider Value Change");
                Owner.TrySetSliderValue(Convert.ToDecimal(destination));
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
            if (Owner.Slider.MouseDown(e.WinFormsEventArgs, e.CanvasLocation))
            {
                m_dragmode = 1;
                sender.Invalidate();
                Rectangle rail = Owner.Slider.Rail;
                decimal d2 = decimal.Multiply(new decimal(rail.Right - rail.Left), Convert.ToDecimal(sender.Viewport.Zoom));
                Owner.Slider.SnapDistance = decimal.Multiply(decimal.Divide(decimal.Subtract(Owner.Slider.Maximum, Owner.Slider.Minimum), d2), 10M);
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
                    if (Owner.Slider.Grip.Contains(e.CanvasLocation))
                    {
                        Instances.CursorServer.AttachCursor(sender, "GH_NumericSlider");
                        mouseMove = GH_ObjectResponse.Handled;
                        break;
                    }
                    goto default;
                case MouseButtons.Left:
                    if (m_dragmode == 1)
                    {
                        Owner.TriggerAutoSave(Owner.InstanceGuid);
                        Owner.RecordUndoEvent("Slider change", new SliderDragUndo(Owner));
                        m_dragmode = 2;
                    }
                    if (m_dragmode == 2)
                    {
                        Owner.Slider.MouseMove(e.WinFormsEventArgs, e.CanvasLocation);
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
            if (m_dragmode > 0)
            {
                m_dragmode = 0;
                Owner.Slider.MouseUp(e.WinFormsEventArgs, e.CanvasLocation);
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

        public override bool HasInputGrip => true;

        public override bool TooltipEnabled => true;

        

        private class SliderDragUndo : GH_ObjectUndoAction
        {
            private decimal _value;

            public SliderDragUndo(GH_NumberSlider owner)
              : base(owner.InstanceGuid)
            {
                _value = owner.Slider.Value;
                ExpiresSolution = true;
            }

            /// <summary>
            /// Override this property if you want the Grasshopper solution to refresh upon undo completion.
            /// </summary>
            public override bool ExpiresSolution { get; }

            protected override void Object_Undo(GH_Document doc, IGH_DocumentObject obj)
            {
                if (!(obj is GH_NumberSlider ghNumberSlider))
                    return;
                decimal num = ghNumberSlider.Slider.Value;
                ghNumberSlider.Slider.Value = _value;
                _value = num;
            }

            protected override void Object_Redo(GH_Document doc, IGH_DocumentObject obj) => Object_Undo(doc, obj);
        }
    }
}
