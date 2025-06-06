using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#nullable disable
namespace Bulge
{
    public class CircleTanTanRadius : GH_Component
    {
        public CircleTanTanRadius()
          : base("Circle Tan Tan Radius", "Circle Tan Tan Radius", "Circle Tan Tan Radius", "Bulge",
                SubCategoryName.CURVE)
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve A", "C1", "First curve", GH_ParamAccess.item);
            pManager.AddCurveParameter("Curve B", "C2", "Second curve", GH_ParamAccess.item);
            pManager.AddNumberParameter("Radius", "R", "Circle radius", GH_ParamAccess.item, 10);
            pManager.AddPlaneParameter("Plane", "P", "Plane", GH_ParamAccess.item, Plane.WorldXY);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCircleParameter("Circles", "C", "Circles", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curveA = null;
            Curve curveB = null;
            double radius = 0;
            Plane plane = Plane.Unset;

            if (!DA.GetData(0, ref curveA)) return;
            if (!DA.GetData(1, ref curveB)) return;
            if (!DA.GetData(2, ref radius)) return;
            if (!DA.GetData(3, ref plane)) return;

            if (curveA == null || curveB == null || radius <= 0 || !plane.IsValid)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid input.");
                return;
            }

            List<Circle> circles = new List<Circle>();

            double tolerance = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;

            Curve[] offsetA1 = curveA.Offset(plane, radius, tolerance, CurveOffsetCornerStyle.Sharp);
            Curve[] offsetA2 = curveA.Offset(plane, -radius, tolerance, CurveOffsetCornerStyle.Sharp);
            Curve[] offsetB1 = curveB.Offset(plane, radius, tolerance, CurveOffsetCornerStyle.Sharp);
            Curve[] offsetB2 = curveB.Offset(plane, -radius, tolerance, CurveOffsetCornerStyle.Sharp);

            // Intersect all combinations of offset pairs
            List<Curve> offsetsA = offsetA1.Concat(offsetA2).ToList();
            List<Curve> offsetsB = offsetB1.Concat(offsetB2).ToList();

            foreach (var oa in offsetsA)
            {
                foreach (var ob in offsetsB)
                {
                    var events = Rhino.Geometry.Intersect.Intersection.CurveCurve(oa, ob, 0.001, 0.001);
                    foreach (var ev in events)
                    {
                        Point3d center = ev.PointA;
                        circles.Add(new Circle(center, radius));
                    }
                }
            }
            DA.SetDataList(0, circles);
        }


        public override GH_Exposure Exposure => GH_Exposure.primary;

        public override Guid ComponentGuid => new Guid("{A43B83E6-B1C5-44CE-AF4C-456B8B341BCA}");

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconCircleTanTanRadius);
    }

}