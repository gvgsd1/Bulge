using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Rhino;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Bulge
{
    public class ListMultiplier : GH_Component
    {
        public ListMultiplier()
          : base("List Multiplier", "List Multiplier", "List Multiplier", "Bulge",
                SubCategoryName.MATHS)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("List", "L", "Input list for multiplier", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Zero Option", "0", "Zero Handling Options: 0 - 0, 0 - 1 (0/0 only), 1 - +Infinity", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Multiplier", "M", "Multiplier", GH_ParamAccess.list);
            pManager.AddGenericParameter("Multiplier Include First Value", "M0", "Multiplier Include First Value", GH_ParamAccess.list);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> inList = new List<double>();
            if (!DA.GetDataList(0, inList))
                return;
            if (inList.Count == 0)
                return;

            int option = 0;
            if (!DA.GetData(1, ref option))
                return;

            List<double> result = new List<double>();
            List<double> result0 = new List<double>();

            result0.Add(inList[0]);

            for (int i = 0; i < inList.Count - 1; i++)
            {
                double current = inList[i];
                double next = inList[i+1];

                double multiplier = next / current;
                if (current == 0)
                {
                    if (option == 0 || option == 1)
                    {
                        multiplier = 0;
                        if (next == 0)
                        {
                            multiplier = 1;
                        }
                    }
                }

                result.Add(multiplier);
                result0.Add(multiplier);
            }

            DA.SetDataList(0, result);
            DA.SetDataList(1, result0);
        }

        public override Guid ComponentGuid => new Guid("{743f036a-5d40-46c4-b0e6-e390ae38b0d8}");

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconListMultiplier);
    }

}