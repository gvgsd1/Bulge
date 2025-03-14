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
using Grasshopper.Kernel.Undo;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Bulge.Params.NumberSliderPlus
{
    /// <exclude />
    public class GH_NumberSliderAttributes : GH_ResizableAttributes<GH_NumberSlider>
    {
        private static readonly int SliderMinWidth = 160;
        private int m_dragmode;
        private string m_cachedName;
        private RectangleF bounds_dup;
        private RectangleF rec_words_only;
        private RectangleF rec_middle;
        private RectangleF rec_slider;
        private RectangleF rec_input;

        public GH_NumberSliderAttributes(GH_NumberSlider nOwner)
          : base(nOwner)
        {
            m_dragmode = 0;
            ExpireLayout();
            Layout();
        }

        protected override Size MinimumSize => new Size(100, 20);

        protected override Size MaximumSize => new Size(5000, 20);

        protected override Padding SizingBorders => new Padding(0, 0, 6, 0);

        public override bool HasInputGrip => true;

        public override bool TooltipEnabled => true;


        public override void AppendToAttributeTree(List<IGH_Attributes> attributes)
        {
            attributes.Add((IGH_Attributes)this);
            foreach (IGH_Param ghParam in this.Owner.Params.Input)
                attributes.Add(ghParam.Attributes);
            foreach (IGH_Param ghParam in this.Owner.Params.Output)
                attributes.Add(ghParam.Attributes);
        }

        protected override void Layout()
        {

            float width_words = GH_FontServer.MeasureString(Owner.NickName, GH_FontServer.StandardAdjusted).Width;
            width_words = Math.Max(width_words + 10f, MinimumSize.Height);
            if (Owner.IsExpression)
                width_words += 16f;   
            
            float height_min = MinimumSize.Height;
            float height_bounds = 84;

            // bounds_dup: Bounding Box for everything
            bounds_dup = new RectangleF(Pivot.X, Pivot.Y, Math.Max(Bounds.Width, width_words + SliderMinWidth), height_bounds);

            // rec_input: Bounding Box for left side input bar
            rec_input = new RectangleF(Pivot, new SizeF(22, height_bounds));

            // rec_words_only: Bounding Box for middle words only
            rec_words_only = new RectangleF(rec_input.Right, Pivot.Y + (height_bounds - height_min)/2, width_words, height_min);

            // rec_middle: Bounding Box for middle pill with words (from all the way left to all the way right)
            rec_middle = new RectangleF(Pivot.X, rec_words_only.Top, bounds_dup.Width, height_min);

            Bounds = new RectangleF(Pivot.X, rec_words_only.Top, Math.Max(Bounds.Width, width_words + SliderMinWidth), height_min);


            // rec_slider: Bounding Box for slider pill
            rec_slider = RectangleF.FromLTRB(rec_words_only.Right, rec_words_only.Top, bounds_dup.Right, rec_words_only.Bottom);

            if (Owner.Slider != null)
            {
                Owner.Slider.Font = GH_FontServer.StandardAdjusted;
                Owner.Slider.DrawControlBorder = false;
                Owner.Slider.DrawControlShadows = false;
                Owner.Slider.DrawControlBackground = false;
                Owner.Slider.TickCount = 11;
                Owner.Slider.TickFrequency = 5;
                Owner.Slider.RailDarkColour = Color.FromArgb(30, Color.Black);
                Owner.Slider.TickDisplay = GH_SliderTickDisplay.Simple;
                Owner.Slider.RailDisplay = GH_SliderRailDisplay.Simple;
                Owner.Slider.Padding = new Padding(6, 2, 8, 1);
                Owner.Slider.Bounds = Rectangle.Round(rec_slider);
            }

            //rec_input_words.Inflate(-2f, -2f);

            // rec_wire: Bounding Box for input and output wires
            var rec_wire_in = bounds_dup;
            rec_wire_in.Inflate(-2f, -2f);
            rec_wire_in.X += 20;
            rec_wire_in.Width -= 16;
            GH_ComponentAttributes.LayoutInputParams(this.Owner, rec_wire_in);
            var rec_wire_out = bounds_dup;
            rec_wire_out.X = bounds_dup.Right;
            rec_wire_out.Width = 0.0f;
            GH_ComponentAttributes.LayoutOutputParams(this.Owner, rec_wire_out);

        }


        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            switch (channel)
            {
                case GH_CanvasChannel.Wires:
                    foreach (IGH_DocumentObject ghDocumentObject in this.Owner.Params.Input)
                        ghDocumentObject.Attributes.RenderToCanvas(canvas, GH_CanvasChannel.Wires);

                    break;
                case GH_CanvasChannel.Objects:
                    string nickName = Owner.NickName;
                    if (m_cachedName == null || !nickName.Equals(m_cachedName, StringComparison.Ordinal))
                    {
                        m_cachedName = nickName;
                        ExpireLayout();
                        Layout();
                    }

                    #region Visibility Test
                    GH_Viewport viewport = canvas.Viewport;
                    RectangleF bounds1 = Bounds;
                    ref RectangleF local = ref bounds1;
                    int num = viewport.IsVisible(ref local, 10f) ? 1 : 0;
                    Bounds = bounds1;
                    if (num == 0)
                        return;
                    #endregion

                    #region Middle Pill (with Words)
                    RectangleF rec_middle_dup = rec_middle;
                    rec_middle_dup.Width += 16;
                    RectangleF rec_words_only_dup = rec_words_only;
                    RectangleF rectangle = RectangleF.Empty;

                    if (Owner.IsExpression)
                    {
                        rec_words_only_dup = new RectangleF(rec_words_only_dup.X + 20, rec_words_only_dup.Y, rec_words_only_dup.Width - 20, rec_words_only_dup.Height);
                    }

                    GH_Capsule pill_middle;
                    switch (Owner.RuntimeMessageLevel)
                    {
                        case GH_RuntimeMessageLevel.Warning:
                            pill_middle = GH_Capsule.CreateTextCapsule(rec_middle_dup, rec_words_only_dup, GH_Palette.Warning, nickName, 10, 0);
                            break;
                        case GH_RuntimeMessageLevel.Error:
                            pill_middle = GH_Capsule.CreateTextCapsule(rec_middle_dup, rec_words_only_dup, GH_Palette.Error, nickName, 10, 0);
                            break;
                        default:
                            pill_middle = GH_Capsule.CreateTextCapsule(rec_middle_dup, rec_words_only_dup, GH_Palette.Hidden, nickName, 10, 0);
                            break;
                    }
                    pill_middle.AddOutputGrip(OutputGrip.Y);
                    Utility.UtilityRender.Render(pill_middle, graphics, null, Selected, Owner.Locked, true);
                    pill_middle.Dispose();
                    #endregion

                    #region Left Pill (Input)

                    GH_Capsule pill_input = GH_Capsule.CreateCapsule(rec_input, GH_Palette.Normal, 25, 0);
                    pill_input.AddInputGrip(InputGrip.Y - 30);
                    pill_input.AddInputGrip(InputGrip.Y - 10);
                    pill_input.AddInputGrip(InputGrip.Y + 10);
                    pill_input.AddInputGrip(InputGrip.Y + 30);
                    pill_input.Font = GH_FontServer.StandardAdjusted;
                    Utility.UtilityRender.Render(pill_input, graphics, null, this.Selected, this.Owner.Locked, true);
                    pill_input.Dispose();

                    #endregion

                    #region Left Pill Input Text
                    int zoomFadeLow = GH_Canvas.ZoomFadeLow;

                    GH_PaletteStyle impliedStyle = GH_CapsuleRenderEngine.GetImpliedStyle(GH_Palette.Normal, Selected, Owner.Locked, false);

                    SolidBrush brush_input_words = new SolidBrush(Color.FromArgb(zoomFadeLow, impliedStyle.Text));
                    float x_input_text = bounds_dup.X + 11;
                    graphics.DrawString("L0", GH_FontServer.StandardAdjusted, brush_input_words, x_input_text, InputGrip.Y - 30, GH_TextRenderingConstants.CenterCenter);
                    graphics.DrawString("L1", GH_FontServer.StandardAdjusted, brush_input_words, x_input_text, InputGrip.Y - 10, GH_TextRenderingConstants.CenterCenter);
                    graphics.DrawString("D", GH_FontServer.StandardAdjusted, brush_input_words, x_input_text, InputGrip.Y + 10, GH_TextRenderingConstants.CenterCenter);
                    graphics.DrawString("T", GH_FontServer.StandardAdjusted, brush_input_words, x_input_text, InputGrip.Y + 30, GH_TextRenderingConstants.CenterCenter);
                    #endregion

                    #region Expression Icon
                    if (Owner.IsExpression && !rectangle.IsEmpty)
                    {
                        //GH_StateTag_Expression stateTagExpression = new GH_StateTag_Expression(Owner.Expression);

                        //stateTagExpression.Stage = new Rectangle(rectangle.X + rectangle.Width / 2 - 8, rectangle.Y + rectangle.Height / 2 - 8, 16, 16);

                        //stateTagExpression.Render(graphics);
                    }
                    #endregion

                    #region Right Pill (Slider)
                    var rec_slider_dup = rec_slider;
                    //rec_slider_dup.Y += 10;
                    GH_Capsule pill_slider = GH_Capsule.CreateCapsule(rec_slider_dup, GH_Palette.Normal, 10, 0);
                    Utility.UtilityRender.Render(pill_slider, graphics, null, Selected, Owner.Locked, false);
                    pill_slider.Dispose();
                    #endregion

                    #region Expression Slider Number Format
                    //if (!Owner.IsExpression || Owner.VolatileDataCount == 0)
                    //    Owner.Slider.FormatMask = "{0}";
                    //else
                    //    Owner.Slider.FormatMask = ((GH_Goo<double>)RuntimeHelpers.GetObjectValue(Owner.VolatileData.get_Branch(0)[0])).Value.ToString();
                    #endregion

                    if (zoomFadeLow > 0)
                    {
                        Owner.Slider.Render(graphics);
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        if (zoomFadeLow < byte.MaxValue)
                        {
                            SolidBrush solidBrush = new SolidBrush(Color.FromArgb(byte.MaxValue - zoomFadeLow, impliedStyle.Fill));
                            Rectangle bounds_zoom = Owner.Slider.Bounds;
                            bounds_zoom.X += Owner.Slider.Padding.Left;
                            bounds_zoom.Y += Owner.Slider.Padding.Top;
                            bounds_zoom.Width -= Owner.Slider.Padding.Horizontal;
                            bounds_zoom.Height -= Owner.Slider.Padding.Vertical;
                            graphics.FillRectangle(solidBrush, bounds_zoom);
                            solidBrush.Dispose();
                        }
                    }
                    if (m_dragmode <= 0 || !Owner.IsExpression)
                        return;
                    PointF location = Owner.Slider.Grip.Location;
                    location.X += 0.5f * Owner.Slider.Grip.Width;
                    RectangleF bounds_zoom_tag = Bounds;
                    bounds_zoom_tag.Inflate(1000f, 1000f);
                    GH_GraphicsUtil.RenderBalloonTag(graphics, "Original: " + Owner.Slider.GripTextPure, GH_FontServer.StandardAdjusted, location, bounds_zoom_tag);
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
