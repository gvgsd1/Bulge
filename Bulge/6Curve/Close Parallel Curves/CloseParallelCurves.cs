using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Bulge
{
    public class CloseParallelCurves : GH_Component
    {
        public CloseParallelCurves()
          : base("Close Parallel Curves", "Close Parallel Curves", "Close Parallel Curves", "Bulge",
                SubCategoryName.CURVE)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve 1", "C1", "Curve 1", GH_ParamAccess.item);
            pManager.AddCurveParameter("Curve 2", "C2", "Curve 2", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Flip", "F", "Flip", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Closed Curve", "C", "Closed Curve", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Curve ghCurve1 = null;
            GH_Curve ghCurve2 = null;
            if (!DA.GetData(0, ref ghCurve1) || ghCurve1 == null)
                return;
            if (!DA.GetData(1, ref ghCurve2) || ghCurve2 == null)
                return;
            GH_Boolean flip = null;
            if (!DA.GetData(2, ref flip))
                return;

            Curve curve1 = ghCurve1.Value;
            Curve curve2 = ghCurve2.Value;

            Point3d start1 = curve1.PointAtStart;
            Point3d end1 = curve1.PointAtEnd;

            double distStart = start1.DistanceTo(curve2.PointAtStart);
            double distEnd = start1.DistanceTo(curve2.PointAtEnd);

            if (distEnd < distStart) curve2.Reverse();
            if (flip.Value) curve2.Reverse();

            Point3d start2 = curve2.PointAtStart;
            Point3d end2 = curve2.PointAtEnd;

            Line startLine = new Line(start1, start2);
            Line endLine = new Line(end1, end2);

            Curve c3 = startLine.ToNurbsCurve();
            Curve c4 = endLine.ToNurbsCurve();
            List<Curve> curves = new List<Curve>() { curve1, curve2, c3, c4 };
            Curve[] joinedCurves = Curve.JoinCurves(curves, GH_Component.DocumentTolerance(), false);
            if (joinedCurves == null) AddRuntimeMessage((GH_RuntimeMessageLevel)20, "Curve joining failed.");

            DA.SetDataList(0, joinedCurves);
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        public override Guid ComponentGuid => new Guid("{16e575e1-4121-484b-927c-9dee132577b1}");

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconCloseParallelCurves);
    }

}