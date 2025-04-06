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
    public class ListIncrement : GH_Component
    {
        public ListIncrement()
          : base("List Increment", "List Increment", "List Increment", "Bulge",
                SubCategoryName.MATHS)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("List", "L", "Input list for increment", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Increment", "I", "Increment", GH_ParamAccess.list);
            pManager.AddGenericParameter("Increment Include First Value", "I0", "Increment Include First Value", GH_ParamAccess.list);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> inList = new List<double>();
            if (!DA.GetDataList(0, inList))
                return;
            if (inList.Count == 0)
                return;

            List<double> result = new List<double>();
            List<double> result0 = new List<double>();

            result0.Add(inList[0]);

            for (int i = 0; i < inList.Count - 1; i++)
            {
                double current = inList[i];
                double next = inList[i+1];

                double increment = next - current;

                result.Add(increment);
                result0.Add(increment);
            }

            DA.SetDataList(0, result);
            DA.SetDataList(1, result0);
        }

        public override Guid ComponentGuid => new Guid("{059a6384-9f98-45a8-b96e-0039f24525ad}");

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconListIncrement);
    }

}