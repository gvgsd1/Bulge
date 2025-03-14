using Grasshopper.Kernel;
using Rhino;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Bulge
{
    public class MassSubtractionUntilValue : GH_Component
    {
        public MassSubtractionUntilValue()
          : base("Mass Subtraction Until Value", "Mass Subtraction Until Value", "Mass Subtraction Until Value", "Bulge",
                SubCategoryName.MATHS)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Input", "I", "Input values for mass subtraction", GH_ParamAccess.list);
            pManager.AddNumberParameter("Cap", "C", "Cap Value", GH_ParamAccess.item, double.NaN);
            pManager.AddIntegerParameter("Option", "O", "Options: 0 - Above & Include, 1 - Below & Include, 2 - Above, 3 - Below", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "R", "Result of mass subtraction", GH_ParamAccess.item);
            pManager.AddGenericParameter("Partial Results", "Pr", "List of partial results", GH_ParamAccess.list);

            pManager.AddGenericParameter("End Index", "I", "End Index", GH_ParamAccess.item);
            pManager.AddGenericParameter("Sublist", "S", "Sublist", GH_ParamAccess.list);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> inList = new List<double>();
            if (!DA.GetDataList(0, inList))
                return;
            if (inList.Count == 0)
                return;

            double cap = double.PositiveInfinity;
            int option = int.MinValue;

            if (!DA.GetData(1, ref cap) || !RhinoMath.IsValidDouble(cap))
                return;
            if (!DA.GetData(2, ref option))
                return;
            if (option < 0 || option > 4)
                return;

            double result = 0;
            int endIndex = 0;
            List<double> partialList = new List<double>();

            for (int i = 0; i < inList.Count; i++)
            {
                double num = inList[i];
                double resultNew = result - num;

                if (option == 0)
                {
                    partialList.Add(resultNew);
                    if ((result < cap && resultNew >= cap) || (result > cap && resultNew <= cap))
                    {
                        result = resultNew;
                        endIndex = i;
                        break;
                    }
                }
                else if (option == 1)
                {
                    if ((result <= cap && resultNew > cap) || (result >= cap && resultNew < cap))
                    {
                        endIndex = i - 1;
                        break;
                    }
                    partialList.Add(result);
                }
                else if (option == 2)
                {
                    partialList.Add(resultNew);
                    if ((result <= cap && resultNew > cap) || (result >= cap && resultNew < cap))
                    {
                        result = resultNew;
                        endIndex = i;
                        break;
                    }
                }
                else if (option == 3)
                {
                    if ((result < cap && resultNew >= cap) || (result > cap && resultNew <= cap))
                    {
                        endIndex = i - 1;
                        break;
                    }
                    partialList.Add(result);
                }
                result = resultNew;
            }
            List<double> sublist = inList.GetRange(0, endIndex + 1);
            DA.SetData(0, result);
            DA.SetDataList(1, partialList);
            DA.SetData(2, endIndex);
            DA.SetDataList(3, sublist);
        }

        public override Guid ComponentGuid => new Guid("{38760496-ba8c-4609-86a6-fe42816552b4}");

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        protected override Bitmap Icon => null;
    }

}