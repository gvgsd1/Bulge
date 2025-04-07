using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Graphs;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;

#nullable disable
namespace Bulge
{
    public class SineCurveSimple : GH_Component
    {
        public SineCurveSimple()
          : base("Sine Curve Simple", "Sine Curve Simple", "Sine Curve simple approximation from Nurb control points (Much Less Control Points)", "Bulge",
                SubCategoryName.CURVE)
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Start Point", "P", "Start Point", GH_ParamAccess.item, new Point3d(0, 0, 0));
            pManager.AddVectorParameter("Direction Vector", "V", "Direction Vector", GH_ParamAccess.item, new Vector3d(1, 0, 0));
            pManager.AddNumberParameter("Amplitude", "A", "Amplitude: ", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Wavelength", "W", "Extension length at end of curve", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Length", "L", "Length of the bounding box in the wavelength direction. NOT the length of Wave", GH_ParamAccess.item, 10);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "Sine Curve", GH_ParamAccess.item);
            pManager.AddPointParameter("Points", "P", "Sine Points. NOT the control points of Curve", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d startPt = new Point3d(0, 0, 0);
            if (!DA.GetData(0, ref startPt))
                return;

            Vector3d direction = new Vector3d(1, 0, 0);
            if (!DA.GetData(1, ref direction))
                return;

            if (!direction.IsZero)
                direction.Unitize();
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Direction vector cannot be zero.");
                return;
            }

            double amplitude = double.NaN;
            if (!DA.GetData(2, ref amplitude))
                return;

            double wavelength = double.NaN;
            if (!DA.GetData(3, ref wavelength))
                return;

            double length = double.NaN;
            if (!DA.GetData(4, ref length))
            {
                return;
            }
            if (length <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid Length");
                return;
            }

            Point3d endPt = startPt + direction * length;

            List<Point3d> sinePts = new List<Point3d>();
            List<Point3d> pickPts = new List<Point3d>();
            int numPoints = (int) Math.Ceiling(length / wavelength * 4) + 1;
            for (int i = -20; i < numPoints + 20; i++)
            {
                double xValue = wavelength / 4 * i;
                double value0 = 0;
                double valueP = amplitude;
                double valueN = -amplitude;
                double yValue = (i % 4 == 0) ? value0 : (i % 4 == 1) ? valueP : (i % 4 == 2) ? 0 : valueN;

                if (i < 0)
                {
                    double j = Math.Abs(i);
                    yValue = (j % 4 == 0) ? value0 : (j % 4 == 1) ? valueN : (j % 4 == 2) ? 0 : valueP;
                }

                if (0 <= i && i < numPoints)
                {
                    pickPts.Add(new Point3d(xValue, yValue, 0));
                }
                sinePts.Add(new Point3d(xValue, yValue, 0));
            }



            List<Point3d> outPts = new List<Point3d>();

            Plane sourcePlane = new Plane(Point3d.Origin, Vector3d.XAxis);
            Plane targetPlane = new Plane(startPt, endPt - startPt);
            Transform orient = Transform.PlaneToPlane(sourcePlane, targetPlane);

            for (int i = 0; i < pickPts.Count; i++)
            {
                var outPt = new Point3d(pickPts[i]);
                outPt.Transform(orient);
                outPts.Add(outPt);
            }

            DA.SetDataList(1, outPts);



            if (sinePts.Count >= 2)
            {
                Curve outCurve = Curve.CreateControlPointCurve(sinePts, 3);
                Rectangle3d rectangle = new Rectangle3d(Plane.WorldXY, new Interval(0, length), new Interval(Math.Abs(amplitude) + 5, -Math.Abs(amplitude) - 5));



                var intersectionEvents = Rhino.Geometry.Intersect.Intersection.CurveCurve(outCurve, rectangle.ToNurbsCurve(), RhinoDoc.ActiveDoc.ModelAbsoluteTolerance, RhinoDoc.ActiveDoc.ModelAngleToleranceRadians);
                var p1 = intersectionEvents[0].ParameterA;
                var p2 = intersectionEvents[1].ParameterA;

                Curve trimmedCurve = outCurve.Trim(p1, p2);

                trimmedCurve.Transform(orient);
                DA.SetData(0, trimmedCurve);
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Not enough control points for a curve");
                return;
            }
        }


        public override Guid ComponentGuid => new Guid("{589f0cb3-1af5-4c17-b1e9-e22b1f8c0e8a}");

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconTogglePlus);
    }

}