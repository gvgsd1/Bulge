using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino;
using Rhino.Collections;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Bulge
{
    public class SetPointsFromPoint : GH_Component
    {

        public SetPointsFromPoint()
          : base("Set Points From Point",
                "Set Points From Point",
                "Set Points X, Y, Z to a reference Point's X, Y, Z",
                "Bulge",
                SubCategoryName.VECTOR)
        {

        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "Points", GH_ParamAccess.list);
            pManager.AddPointParameter("Reference Point", "R", "Reference Point", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Set X Value", "X", "Set X Value", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Set Y Value", "Y", "Set Y Value", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Set Z Value", "Z", "Set Z Value", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "Points", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<Point3d> inPts = new List<Point3d>();
            Point3d refPt = Point3d.Unset;
            bool xValue = false;
            bool yValue = false;
            bool zValue = false;
            if (!DA.GetDataList(0, inPts)) return;
            if (!DA.GetData(1, ref refPt) || !refPt.IsValid) return;
            if (!DA.GetData(2, ref xValue)) return;
            if (!DA.GetData(3, ref yValue)) return;
            if (!DA.GetData(4, ref zValue)) return;

            if (refPt == Point3d.Unset)
            {
                DA.SetDataList(0, inPts);
                return;
            }

            if (!xValue && !yValue && !zValue)
            {
                DA.SetDataList(0, inPts);
                return;
            }


            for (int i = 0; i < inPts.Count; i++)
            {
                Point3d inPt = inPts[i];
                if (xValue)
                {
                    inPt.X = refPt.X;
                }
                if (yValue)
                {
                    inPt.Y = refPt.Y;
                }
                if (zValue)
                {
                    inPt.Z = refPt.Z;
                }
                inPts[i] = inPt;
            }
            DA.SetDataList(0, inPts);
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override System.Drawing.Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconSetPointsFromPoint);

        public override Guid ComponentGuid => new Guid("592002bd-1257-4e81-86d9-8f73f794edd8");
    }
}


