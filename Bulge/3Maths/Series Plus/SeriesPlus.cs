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
    public class SeriesPlus : GH_Component, IGH_InitCodeAware
    {
        public SeriesPlus()
          : base("Series Plus",
                "Series +",
                "Series that support multiple Step inputs",
                "Bulge",
                SubCategoryName.MATHS)
        {
        }

        public void SetInitCode(string code) => this.AssignInitCodeToInputParameter(2, code);

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Start", "S", "First number in the series", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("Step", "N", "Step size for each successive number", GH_ParamAccess.list, 1.0);
            pManager.AddIntegerParameter("Count", "C", "Number of values in the series", GH_ParamAccess.item, 10);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Series", "S", "Series of numbers", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double start = double.NaN;
            List<double> step = new List<double>();
            int count = 0;
            if (!DA.GetData(0, ref start) || !DA.GetDataList(1, step) || !DA.GetData(2, ref count))
                return;

            if (count < 0)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)10, "Series with negative count ignored.");
            }
            else
            {
                if (count == 0)
                    return;
                List<GH_Number> outList = new List<GH_Number>() { new GH_Number(start) };
                var num = start;
                for (int i = 0; i < count - 1; i++)
                {
                    int iStep = i % step.Count;

                    num += step[iStep];
                    outList.Add(new GH_Number(num));
                }
                DA.SetDataList(0, outList);
            }
        }


        protected override System.Drawing.Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconSeriesPlus);

        public override Guid ComponentGuid => new Guid("{0b41c951-2e63-447c-b6ae-8bc5334bca57}");

    }
}
