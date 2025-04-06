using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Bulge
{
    public class SplitListPlus : GH_Component
    {
        public SplitListPlus()
          : base("Split List Plus", "Split List Plus", "Split list wrapping negative and out-of-range index", "Bulge",
                SubCategoryName.SETS)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("List", "L", "Base list", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Index", "i", "Splitting index", GH_ParamAccess.item, 0);
            pManager.AddBooleanParameter("Warp", "W", "Wrap indices to list range", GH_ParamAccess.item, true);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("List A", "A", "Items to the left of (i)", GH_ParamAccess.list);
            pManager.AddGenericParameter("List B", "B", "Items to the right of and including (i)", GH_ParamAccess.list);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int target = 0;
            if (!DA.GetData<int>(1, ref target))
                return;
            List<IGH_Goo> list = new List<IGH_Goo>();
            if (!DA.GetDataList<IGH_Goo>(0, list) || list.Count == 0)
                return;
            List<IGH_Goo> data1 = new List<IGH_Goo>();
            List<IGH_Goo> data2 = new List<IGH_Goo>();
            int num = checked(list.Count - 1);
            int i = 0;

            bool wrap = true;
            if (!DA.GetData<bool>(2, ref wrap))
                return;
            int count = list.Count;
            if (wrap)
            {
                if (count <= 0) return;
                if (target < 0) target = count + (target % count);
                target %= count;
            }
            else
            {
                if (target < 0) target = 0;
                if (target >= list.Count) target = list.Count - 1;
            }

            while (i <= num)
            {
                if (i < target)
                    data1.Add(list[i]);
                else
                    data2.Add(list[i]);
                checked { ++i; }
            }
            DA.SetDataList(0, data1);
            DA.SetDataList(1, data2);
        }

        public override Guid ComponentGuid => new Guid("{ccab7ecc-6b88-4997-b842-b3721bd0e8f4}");

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconSplitListPlus);
    }

}