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
    public class SetPoints : GH_Component
    {

        public SetPoints()
          : base("Set Points",
                "Set Points",
                "Set Points X, Y, Z to value",
                "Bulge",
                SubCategoryName.VECTOR)
        {

        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "Points", GH_ParamAccess.list);
            pManager.AddNumberParameter("Set X Value", "X", "Set X Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("Set Y Value", "Y", "Set Y Value", GH_ParamAccess.item);
            pManager.AddNumberParameter("Set Z Value", "Z", "Set Z Value", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "Points", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<Point3d> inPts = new List<Point3d>();
            double xValue = double.NaN;
            double yValue = double.NaN;
            double zValue = double.NaN;
            if (!DA.GetDataList(0, inPts)) return;
            DA.GetData(1, ref xValue);
            DA.GetData(2, ref yValue);
            DA.GetData(3, ref zValue);

            if (double.IsNaN(xValue) && double.IsNaN(yValue) && double.IsNaN(zValue))
            {
                DA.SetDataList(0, inPts);
                return;
            }
            for (int i = 0; i < inPts.Count; i++)
            {
                Point3d inPt = inPts[i];
                if (!double.IsNaN(xValue))
                {
                    inPt.X = xValue;
                }
                if (!double.IsNaN(yValue))
                {
                    inPt.Y = yValue;
                }
                if (!double.IsNaN(zValue))
                {
                    inPt.Z = zValue;
                }
                inPts[i] = inPt;
            }
            DA.SetDataList(0, inPts);
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override System.Drawing.Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconSetPoints);

        public override Guid ComponentGuid => new Guid("1c797ce4-6337-4a8f-b224-3a2ee0bab411");
    }
}


