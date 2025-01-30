using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#nullable disable
namespace Bulge.Params.MDSliderPlus
{
    /// <exclude />
    public class GH_MultiDimensionalSliderAttributes :
      GH_ResizableAttributes<GH_MultiDimensionalSlider>
    {
        private DragMode m_drag;
        private Point3d m_drag_start;
        private PointF m_drag_point;
        private const int grip_size = 5;
        private GH_2DLayout m_2d;
        private GH_Capsule rec_big;

        public GH_MultiDimensionalSliderAttributes(GH_MultiDimensionalSlider owner)
          : base(owner)
        {
            m_drag = DragMode.None;
            m_drag_start = Point3d.Unset;
            Bounds = (RectangleF)new Rectangle(0, 0, 222, 200);
        }

        public override void AppendToAttributeTree(List<IGH_Attributes> attributes)
        {
            attributes.Add((IGH_Attributes)this);
            foreach (IGH_Param ghParam in this.Owner.Params.Input)
                attributes.Add(ghParam.Attributes);
            foreach (IGH_Param ghParam in this.Owner.Params.Output)
                attributes.Add(ghParam.Attributes);
        }


        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            GH_ObjectResponse mouseDown;
            if (e.Button == MouseButtons.Left)
            {
                m_drag_point = e.CanvasLocation;
                m_drag_start = Owner.Value;
                if (m_2d != null)
                {
                    if (m_2d.XY_Grip_Box.Contains(e.CanvasLocation))
                    {
                        m_drag = DragMode.DragStart;
                        mouseDown = GH_ObjectResponse.Capture;
                        goto label_9;
                    }
                    else if (m_2d.X_Grip_Box.Contains(e.CanvasLocation))
                    {
                        m_drag = DragMode.DragStart;
                        mouseDown = GH_ObjectResponse.Capture;
                        goto label_9;
                    }
                    else if (m_2d.Y_Grip_Box.Contains(e.CanvasLocation))
                    {
                        m_drag = DragMode.DragStart;
                        mouseDown = GH_ObjectResponse.Capture;
                        goto label_9;
                    }
                }
            }
            mouseDown = base.RespondToMouseDown(sender, e);
        label_9:
            return mouseDown;
        }

        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            GH_ObjectResponse mouseMove;
            if (m_drag == DragMode.DragStart)
            {
                if (m_2d == null)
                {
                    mouseMove = GH_ObjectResponse.Release;
                    goto label_28;
                }
                else
                {
                    if (m_2d.XY_Grip_Box.Contains(e.CanvasLocation))
                    {
                        m_drag = DragMode.Grip;
                        Owner.TriggerAutoSave(GH_AutoSaveTrigger.object_generic);
                        Owner.RecordUndoEvent("2D Grip drag");
                    }
                    if (m_2d.X_Grip_Box.Contains(e.CanvasLocation))
                    {
                        m_drag = DragMode.GripX;
                        Owner.TriggerAutoSave(GH_AutoSaveTrigger.object_generic);
                        Owner.RecordUndoEvent("2D Grip drag");
                    }
                    if (m_2d.Y_Grip_Box.Contains(e.CanvasLocation))
                    {
                        m_drag = DragMode.GripY;
                        Owner.TriggerAutoSave(GH_AutoSaveTrigger.object_generic);
                        Owner.RecordUndoEvent("2D Grip drag");
                    }
                }
            }
            switch (m_drag)
            {
                case DragMode.Grip:
                case DragMode.GripX:
                case DragMode.GripY:
                case DragMode.GripZ:
                    double num1 = (double)e.CanvasX - (double)m_drag_point.X;
                    double num2 = ((double)e.CanvasY - (double)m_drag_point.Y) * -1.0;

                    double width = (double)m_2d.m_xy_plane.Width;
                    double height = (double)m_2d.m_xy_plane.Height;
                    double num3 = num1 / width;
                    double num4 = num2 / height;
                    double num5 = m_drag_start.X + num3;
                    double num6 = m_drag_start.Y + num4;
                    if (m_drag == DragMode.GripX)
                        num6 = m_drag_start.Y;
                    if (m_drag == DragMode.GripY)
                        num5 = m_drag_start.X;
                    if (num5 < 0.0)
                        num5 = 0.0;
                    if (num5 > 1.0)
                        num5 = 1.0;
                    if (num6 < 0.0)
                        num6 = 0.0;
                    if (num6 > 1.0)
                        num6 = 1.0;
                    Point3d point3d1 = Owner.Value;
                    if (point3d1.X == num5)
                    {
                        Point3d point3d2 = Owner.Value;
                        if (point3d2.Y == num6)
                        {
                            mouseMove = GH_ObjectResponse.Handled;
                            goto label_28;
                        }
                    }
                    GH_MultiDimensionalSlider owner = Owner;
                    double num7 = num5;
                    double num8 = num6;
                    Point3d point3d3 = Owner.Value;
                    double z = point3d3.Z;
                    Point3d point3d4 = new Point3d(num7, num8, z);
                    owner.Value = point3d4;
                    Owner.ExpireSolution(true);
                    mouseMove = GH_ObjectResponse.Handled;
                    goto label_28;

            }
            mouseMove = base.RespondToMouseMove(sender, e);
        label_28:
            return mouseMove;
        }

        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            GH_ObjectResponse mouseUp;
            if (m_drag != DragMode.None)
            {
                m_drag = DragMode.None;
                mouseUp = GH_ObjectResponse.Release;
            }
            else
                mouseUp = base.RespondToMouseUp(sender, e);
            return mouseUp;
        }

        public override GH_ObjectResponse RespondToMouseDoubleClick(
          GH_Canvas sender,
          GH_CanvasMouseEvent e)
        {
            GH_ObjectResponse mouseDoubleClick;
            if (e.Button == MouseButtons.Left)
            {
                Owner.DisplaySliderSettings();
                mouseDoubleClick = GH_ObjectResponse.Handled;
            }
            else
                mouseDoubleClick = base.RespondToMouseDoubleClick(sender, e);
            return mouseDoubleClick;
        }

        //public override bool HasInputGrip => true;

        //public override bool HasOutputGrip => true;

        protected override Size MinimumSize => new Size(100, 100);

        protected override Padding SizingBorders => new Padding(6);

        public override void ExpireLayout()
        {
            base.ExpireLayout();
            if (rec_big != null)
            {
                rec_big.Dispose();
                rec_big = null;
            }
            m_2d = null;
        }

        protected override void Layout()
        {
            RectangleF bounds = Bounds;
            bounds.Location = Pivot;
            bounds.Width = Math.Max(bounds.Width, MinimumSize.Width);
            bounds.Height = Math.Max(bounds.Height, MinimumSize.Height);
            Bounds = (RectangleF)GH_Convert.ToRectangle(bounds);
            Layout2D();


            var Bounds2 = Bounds;
            Bounds2.X += 15;
            Bounds2.Width -= 35;
            GH_ComponentAttributes.LayoutInputParams(this.Owner, Bounds2);
            GH_ComponentAttributes.LayoutOutputParams(this.Owner, Bounds2);
        }

        private RectangleF SliderBounds
        {
            get
            {
                RectangleF bounds = Bounds;
                bounds.Inflate(-11f, 0f);
                return bounds;
            }
        }

        private void Layout2D()
        {
            m_2d = new GH_2DLayout();
            m_2d.m_xy_plane = SliderBounds;
            //m_2d.m_xy_plane.Inflate(-5f, 0f);
            RectangleF xyPlane = m_2d.m_xy_plane;
            Point3d point3d = Owner.Value;
            double x1 = point3d.X;
            point3d = Owner.Value;
            double y1 = point3d.Y;
            float x2 = float.NaN;
            ref float local1 = ref x2;
            float y2 = float.NaN;
            ref float local2 = ref y2;
            EvaluateBox(xyPlane, x1, y1, ref local1, ref local2);
            m_2d.m_xy_grip = new PointF(x2, y2);
            m_2d.m_x_grip = new PointF(x2, m_2d.m_xy_plane.Bottom);
            m_2d.m_y_grip = new PointF(m_2d.m_xy_plane.Left, y2);
        }

        private void Layout3D()
        {
        }

        private void EvaluateBox(RectangleF box, double x, double y, ref float px, ref float py)
        {
            x = (double)box.X + x * (double)box.Width;
            y = (double)box.Bottom - y * (double)box.Height;
            px = Convert.ToSingle(x);
            py = Convert.ToSingle(y);
        }

        protected override void PrepareForRender(GH_Canvas canvas)
        {
            if (rec_big != null)
                return;
            rec_big = GH_Capsule.CreateCapsule(Bounds, GH_Palette.Hidden, 11, 0);
            rec_big.AddInputGrip(OutputGrip.Y - 66);
            rec_big.AddInputGrip(OutputGrip.Y);
            rec_big.AddInputGrip(OutputGrip.Y + 66);
            rec_big.AddOutputGrip(OutputGrip.Y);
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            switch (channel)
            {
                case GH_CanvasChannel.Wires:
                    //if (this.Owner.SourceCount <= 0)
                    //    break;
                    //this.RenderIncomingWires(canvas.Painter, this.Owner.Sources, this.Owner.WireDisplay);

                    foreach (IGH_DocumentObject ghDocumentObject in this.Owner.Params.Input)
                        ghDocumentObject.Attributes.RenderToCanvas(canvas, GH_CanvasChannel.Wires);

                    break;
                case GH_CanvasChannel.Objects:
                    GH_Viewport viewport = canvas.Viewport;
                    RectangleF bounds = Bounds;
                    ref RectangleF local = ref bounds;
                    int num = viewport.IsVisible(ref local, 10f) ? 1 : 0;
                    Bounds = bounds;
                    if (num == 0)
                        break;
                    //rec_big.Render(graphics, Selected, Owner.Locked, true);
                    Utility.UtilityRender.Render(rec_big, graphics, null, Selected, Owner.Locked, true);
                    //Rectangle rectangle = GH_Convert.ToRectangle(SliderBounds);
                    //graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.White)), rectangle);
                    Render2D(graphics);

                    //GH_GraphicsUtil.ShadowRectangle(graphics, rectangle);
                    //graphics.DrawRectangle(Pens.Black, rectangle);
                    break;
            }
        }

        private void Render2D(Graphics G)
        {
            //GH_GraphicsUtil.Grid(G, m_2d.m_xy_plane, 10);
            Grid(G, m_2d.m_xy_plane, 10, 10, Color.FromArgb(30, Color.White), Color.FromArgb(40, GH_Skin.palette_hidden_standard.Edge));
            Pen pen = new Pen(GH_Skin.palette_hidden_standard.Edge, 1f);
            //pen.DashPattern = new float[2] { 2f, 2f };
            G.DrawRectangle(pen, GH_Convert.ToRectangle(m_2d.m_xy_plane));
            pen.Dispose();
            RectangleF xyPlane = m_2d.m_xy_plane;
            Point3d point3d1 = Owner.Value;
            double x1 = point3d1.X;
            point3d1 = Owner.Value;
            double y1 = point3d1.Y;
            float num1 = float.NaN;
            ref float local1 = ref num1;
            float num2 = float.NaN;
            ref float local2 = ref num2;
            EvaluateBox(xyPlane, x1, y1, ref local1, ref local2);
            RectangleF rectangleF = RectangleF.FromLTRB(m_2d.m_xy_plane.X, num2, num1, m_2d.m_xy_plane.Bottom);
            //if ((double)rectangleF.Width > 0.5 && (double)rectangleF.Height > 0.5)
            //{
            //    LinearGradientBrush linearGradientBrush1 = new LinearGradientBrush(rectangleF, Color.FromArgb(1, Color.White), Color.FromArgb(200, Color.White), LinearGradientMode.Horizontal);
            //    linearGradientBrush1.WrapMode = WrapMode.TileFlipXY;
            //    G.FillRectangle(linearGradientBrush1, rectangleF);
            //    linearGradientBrush1.Dispose();
            //    LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(rectangleF, Color.FromArgb(200, Color.White), Color.FromArgb(1, Color.White), LinearGradientMode.Vertical);
            //    linearGradientBrush2.WrapMode = WrapMode.TileFlipXY;
            //    G.FillRectangle(linearGradientBrush2, rectangleF);
            //    linearGradientBrush2.Dispose();
            //    G.DrawRectangle(Pens.Black, GH_Convert.ToRectangle(rectangleF));
            //}
            //PointF pA1 = new PointF(m_2d.m_xy_plane.Left, m_2d.m_xy_plane.Bottom);
            //PointF pB1 = new PointF(m_2d.m_xy_plane.Right, m_2d.m_xy_plane.Bottom);
            //Point3d point3d2 = Owner.Value;
            //double x2 = point3d2.X;
            //GraphicsPath path1 = TriangleGrip(pA1, pB1, x2, 5f);
            //PointF pA2 = new PointF(m_2d.m_xy_plane.X, m_2d.m_xy_plane.Bottom);
            //PointF pB2 = new PointF(m_2d.m_xy_plane.X, m_2d.m_xy_plane.Top);
            //Point3d point3d3 = Owner.Value;
            //double y2 = point3d3.Y;
            //GraphicsPath path2 = TriangleGrip(pA2, pB2, y2, 5f);
            //G.FillPath(Brushes.Black, path1);
            //G.FillPath(Brushes.Black, path2);
            G.FillEllipse(Brushes.Black, m_2d.XY_Grip_Box);
            G.FillEllipse(Brushes.White, RectangleF.Inflate(m_2d.XY_Grip_Box, -2f, -2f));
            string text = string.Format("{0:0.0#} ; {1:0.0#}", Owner.X, Owner.Y);
            GH_GraphicsUtil.RenderBalloonTag(G, text, new PointF(num1, num2), m_2d.m_xy_plane);
        }

        public static void Grid(Graphics g, RectangleF rec, int densityX, int densityY, Color colorBg, Color colorWire)
        {
            Rectangle rect = GH_Convert.ToRectangle(rec);
            SolidBrush solidBrush = new SolidBrush(colorBg);
            g.FillRectangle(solidBrush, rect);
            solidBrush.Dispose();
            Pen pen = new Pen(colorWire);
            if (g.Transform.Elements[0] == 1f)
            {
                if (densityX > 2)
                {
                    double num = 1.0 / (double)densityX;
                    double num2 = 1.0 - 0.5 * num;
                    double num3 = num;
                    bool flag = num3 >= 0.0;
                    for (double num4 = num; flag ? (num4 <= num2) : (num4 >= num2); num4 += num3)
                    {
                        int num5 = Convert.ToInt32((double)rec.X + num4 * (double)rec.Width);
                        g.DrawLine(pen, num5, rect.Y, num5, rect.Bottom);
                    }
                }

                if (densityY > 2)
                {
                    double num6 = 1.0 / (double)densityY;
                    double num7 = 1.0 - 0.5 * num6;
                    double num8 = num6;
                    bool flag2 = num8 >= 0.0;
                    for (double num9 = num6; flag2 ? (num9 <= num7) : (num9 >= num7); num9 += num8)
                    {
                        int num10 = Convert.ToInt32((double)rec.Y + num9 * (double)rec.Height);
                        g.DrawLine(pen, rect.X, num10, rect.Right, num10);
                    }
                }
            }
            else
            {
                if (densityX > 2)
                {
                    double num11 = 1.0 / (double)densityX;
                    double num12 = 1.0 - 0.5 * num11;
                    double num13 = num11;
                    bool flag3 = num13 >= 0.0;
                    for (double num14 = num11; flag3 ? (num14 <= num12) : (num14 >= num12); num14 += num13)
                    {
                        float num15 = Convert.ToSingle((double)rec.X + num14 * (double)rec.Width);
                        g.DrawLine(pen, num15, rect.Y, num15, rect.Bottom);
                    }
                }

                if (densityY > 2)
                {
                    double num16 = 1.0 / (double)densityY;
                    double num17 = 1.0 - 0.5 * num16;
                    double num18 = num16;
                    bool flag4 = num18 >= 0.0;
                    for (double num19 = num16; flag4 ? (num19 <= num17) : (num19 >= num17); num19 += num18)
                    {
                        float num20 = Convert.ToSingle((double)rec.Y + num19 * (double)rec.Height);
                        g.DrawLine(pen, rect.X, num20, rect.Right, num20);
                    }
                }
            }

            pen.Dispose();
        }

        /// <summary>Create a triangular grip of a specific size perpendicular to a given edge.</summary>
        /// <param name="pA">Start point of edge.</param>
        /// <param name="pB">End point of edge.</param>
        /// <param name="t">Normalised parameter along edge.</param>
        /// <param name="size">Size of grip.</param>
        private GraphicsPath TriangleGrip(PointF pA, PointF pB, double t, float size)
        {
            float num1 = pB.X - pA.X;
            float num2 = pB.Y - pA.Y;
            float single = Convert.ToSingle(Math.Sqrt((double)num1 * (double)num1 + (double)num2 * (double)num2));
            PointF pointF1 = new PointF(Convert.ToSingle((double)pA.X + t * (double)num1), Convert.ToSingle((double)pA.Y + t * (double)num2));
            float num3 = num1 / single;
            float num4 = num2 / single;
            PointF pointF2 = new PointF(pointF1.X + num4 * 2f * size, pointF1.Y + num3 * 2f * size);
            PointF pointF3 = new PointF(pointF2.X + size * num3, pointF2.Y + size * num4);
            PointF pointF4 = new PointF(pointF2.X - size * num3, pointF2.Y - size * num4);
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddLines(new PointF[4]
            {
        pointF1,
        pointF3,
        pointF4,
        pointF1
            });
            return graphicsPath;
        }

        private enum DragMode
        {
            None,
            DragStart,
            Grip,
            GripX,
            GripY,
            GripZ,
        }

        private class GH_2DLayout
        {
            public RectangleF m_xy_plane;
            public PointF m_xy_grip;
            public PointF m_x_grip;
            public PointF m_y_grip;

            public RectangleF XY_Grip_Box
            {
                get => new RectangleF(m_xy_grip.X - 5f, m_xy_grip.Y - 5f, 10f, 10f);
            }

            public RectangleF X_Grip_Box
            {
                get => new RectangleF(m_x_grip.X - 5f, m_x_grip.Y, 10f, 10f);
            }

            public RectangleF Y_Grip_Box
            {
                get => new RectangleF(m_y_grip.X - 10f, m_y_grip.Y - 5f, 10f, 10f);
            }
        }

    }
}
