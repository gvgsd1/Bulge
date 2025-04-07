using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Bulge
{
    public class SineCurve : GH_Component
    {
        public SineCurve()
          : base("Sine Curve", "Sine Curve", "Sine Curve", "Bulge",
                SubCategoryName.CURVE)
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Start Point", "P", "Start Point", GH_ParamAccess.item, Point3d.Origin);
            pManager.AddVectorParameter("Direction Vector", "V", "Quick Direction Vector. Used only if Direction Plane is not connected", GH_ParamAccess.item, Vector3d.XAxis);
            pManager.AddPlaneParameter("Direction Plane", "Pl", "[Optional] Direction Plane ", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddNumberParameter("Amplitude", "A", "Amplitude", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Wavelength", "W", "Extension length at end of curve", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Length", "L", "Length of the bounding box in the wavelength direction. NOT the length of Wave", GH_ParamAccess.item, 10);
            pManager.AddNumberParameter("Point Density", "D", "Distance between points in the wavelength direction", GH_ParamAccess.item, 0.05);
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "Sine Curve", GH_ParamAccess.item);
            pManager.AddPointParameter("Points", "P", "Points used to interpolate the sine curve. NOT the curve control points", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d startPt = Point3d.Unset;
            if (!DA.GetData(0, ref startPt))
                return;

            Vector3d directionVector = Vector3d.Unset;
            if (!DA.GetData(1, ref directionVector))
                return;

            Plane directionPlane = Plane.Unset;
            Vector3d direction = Vector3d.Unset;

            if (Params.Input[2].Sources.Count > 0)
            {
                if (!DA.GetData(2, ref directionPlane))
                    return;
                directionPlane.Origin = startPt;
                direction = directionPlane.XAxis;
            }
            else
            {
                directionVector.Unitize();
                if (directionVector != Vector3d.ZAxis)
                {
                    direction = directionVector;
                    Vector3d direction2 = Vector3d.CrossProduct(direction, Vector3d.ZAxis);
                    directionPlane = new Plane(startPt, direction, direction2);
                }
                else
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Direction vector cannot be Unit Z");
                    return;
                }
            }


            if (!direction.IsZero)
                direction.Unitize();
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Direction vector cannot be zero.");
                return;
            }

            double amplitude = double.NaN;
            if (!DA.GetData(3, ref amplitude))
                return;

            double wavelength = double.NaN;
            if (!DA.GetData(4, ref wavelength))
                return;

            double length = double.NaN;
            if (!DA.GetData(5, ref length))
            {
                return;
            }
            if (length <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid Length");
                return;
            }

            double density = double.NaN;
            if (!DA.GetData(6, ref density))
            {
                return;
            }
            if (density <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid Density");
                return;
            }

            List<Point3d> sinePts = new List<Point3d>();

            for (double x = 0.0; length - x > 1e-6; x += density)
            {
                double y = Math.Sin(x * 2 * Math.PI / wavelength) * amplitude;
                sinePts.Add(new Point3d(x, y, 0));
            }

            double y1 = Math.Sin(length * 2 * Math.PI / wavelength) * amplitude;
            sinePts.Add(new Point3d(length, y1, 0));

            List<Point3d> outPts = new List<Point3d>();

            Plane sourcePlane = Plane.WorldXY;
            Plane targetPlane = directionPlane;
            Transform orient = Transform.PlaneToPlane(sourcePlane, targetPlane);

            for (int i = 0; i < sinePts.Count; i++)
            {
                var outPt = new Point3d(sinePts[i]);
                outPt.Transform(orient);
                outPts.Add(outPt);
            }

            DA.SetDataList(1, outPts);

            if (sinePts.Count >= 2)
            {
                Curve outCurve = Curve.CreateInterpolatedCurve(sinePts, 3);
                outCurve.Transform(orient);
                DA.SetData(0, outCurve);
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Not enough control points for a curve");
                return;
            }
        }


        public override Guid ComponentGuid => new Guid("{b6fc6ddc-9066-4261-9f06-52e350b608c5}");

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconTogglePlus);
    }

}