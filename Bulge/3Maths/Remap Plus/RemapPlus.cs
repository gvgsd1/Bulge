using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Bulge
{
    public class RemapPlus : GH_Component
    {
        public RemapPlus()
          : base("Remap +",
                "Remap +",
                "Remap with Auto Input range",
                "Bulge",
                SubCategoryName.MATHS)
        {

        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Value", "V", "Value to remap", GH_ParamAccess.list);
            pManager.AddNumberParameter("Target Min", "T0", "Target Min", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("Target Max", "T1", "Target Max", GH_ParamAccess.item, 1.0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Mapped", "R", "Remapped number", GH_ParamAccess.list);
            pManager.AddNumberParameter("Clipped", "C", "Remapped and clipped number", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<double> inList = new List<double>();
            if (!DA.GetDataList(0, inList))
                return;
            if (inList.Count == 0)
                return;

            double t0 = double.NaN;
            double t1 = double.NaN;

            if (!DA.GetData(1, ref t0) || !DA.GetData(2, ref t1) || !RhinoMath.IsValidDouble(t0) || !RhinoMath.IsValidDouble(t1))
                return;
            double s0 = inList.Min();
            double s1 = inList.Max();
            List<double> mapList = new List<double>();
            List<double> capList = new List<double>();

            for (int i = 0; i < inList.Count; i++)
            {
                double num = inList[i];
                double map;
                double cap;
                if (s0 == s1 || t0 == t1)
                {
                    map = t0;
                    cap = t0;
                }
                else
                {
                    map = t0 + (num - s0) / (s1 - s0) * (t1 - t0);
                    cap = Math.Min(Math.Max(map, t0), t1);
                }
                mapList.Add(map);
                capList.Add(cap);
            }

            DA.SetDataList(0, mapList);
            DA.SetDataList(1, capList);

        }

        protected override System.Drawing.Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconRemapPlus);

        public override Guid ComponentGuid => new Guid("b1e89d0a-13d4-4f3f-9e2a-aafb2a4ebbd7");
    }
}


