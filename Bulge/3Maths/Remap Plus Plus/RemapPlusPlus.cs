using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Bulge
{
    public class RemapPlusPlus : GH_Component
    {
        public RemapPlusPlus()
          : base("Remap ++",
                "Remap ++",
                "Remap with input min, input max, output min, output max",
                "Bulge",
                SubCategoryName.MATHS)
        {

        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Value", "V", "Value to remap", GH_ParamAccess.item);
            pManager.AddNumberParameter("Souce Min", "S0", "Souce Min", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("Souce Max", "S1", "Souce Max", GH_ParamAccess.item, 1.0);
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
            double num = double.NaN;
            double s0 = double.NaN;
            double s1 = double.NaN;
            double t0 = double.NaN;
            double t1 = double.NaN;

            if (!DA.GetData(0, ref num) || !DA.GetData(1, ref s0) || !DA.GetData(2, ref s1) || !DA.GetData(3, ref t0) || !DA.GetData(4, ref t1) || !RhinoMath.IsValidDouble(num) || !RhinoMath.IsValidDouble(s0) || !RhinoMath.IsValidDouble(s1) || !RhinoMath.IsValidDouble(t0) || !RhinoMath.IsValidDouble(t1))
                return;

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
            DA.SetData(0, map);
            DA.SetData(1, cap);
        }

        protected override System.Drawing.Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconRemapPlusPlus);

        public override Guid ComponentGuid => new Guid("72eb86ce-1b58-4feb-9915-6bad3e5285d4");
    }
}


