using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#nullable disable
namespace Bulge
{
    public class CullNthPlus : GH_Component
    {
        public CullNthPlus()
          : base("Cull Nth Plus", "CullN+", "Cull Nth with a list of N input", "Bulge",
                SubCategoryName.SETS)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("List", "L", "List to cull", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Cull frequency", "N", "Cull frequency", GH_ParamAccess.list);
            ((GH_PersistentParam<GH_Integer>)pManager[1]).SetPersistentData(2, 3);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Remain List", "L", "Remain list", GH_ParamAccess.list);
            pManager.AddGenericParameter("Remove List", "R", "Remove list", GH_ParamAccess.list);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<int> nList = new List<int>();
            if (!DA.GetDataList(1, nList))
                return;

            List<IGH_Goo> inList = new List<IGH_Goo>();
            
            if (!DA.GetDataList(0, inList) || inList.Count == 0)
                return;

            if (nList.Count == 0)
            {
                DA.SetDataList(0, inList);
                return;
            }

            int inCount = inList.Count;

            foreach (var n in nList)
            {
                if (n == 0)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Frequency can't be 0");
                    return;
                }
            }


            nList = NormalizeIndex(nList, inCount);

            nList = RepeatNList(nList, inCount);

            List<IGH_Goo> remainList, cullList;
            RemoveNthElements(inList, nList, out remainList, out cullList);

            DA.SetDataList(0, remainList);
            DA.SetDataList(1, cullList);

        }

        public List<int> NormalizeIndex(List<int> indices, int length)
        {
            List<int> normalized = new List<int>(indices.Count);

            foreach (int index in indices)
            {
                var newIndex = (index - 1) % length;
                if (index < 0) newIndex = length + ((index - 1) % length);
                normalized.Add(newIndex + 1);
            }

            return normalized;
        }

        public List<int> RepeatNList(List<int> nList, int target)
        {
            int sum = nList.Sum();

            List<int> newNList = new List<int>(nList.Count);
            int newNum = 0;
            foreach (int num in nList)
            {
                newNum += num;
                newNList.Add(newNum);
            }

            List<int> outList = new List<int>(newNList);
            int repeat = 1;
            while (outList.Last() <= target)
            {
                int nCount = newNList.Count;
                int outCount = outList.Count;
                for (int i = 0; i < nCount; i++)
                {
                    int newValue = newNList[i] + sum * repeat;
                    outList.Add(newValue);
                    if (newValue >= target) break; 
                }
                repeat++;
            }
            return outList;
        }

        public void RemoveNthElements(List<IGH_Goo> inputList, List<int> nthList, out List<IGH_Goo> remainList, out List<IGH_Goo> cullList)
        {
            List<int> sortedIndices = nthList.Distinct()
                                             .Where(i => i >= 0 && i <= inputList.Count)
                                             .OrderByDescending(i => i)
                                             .ToList();

            remainList = new List<IGH_Goo>(inputList);
            cullList = new List<IGH_Goo>();

            foreach (int index in sortedIndices)
            {
                cullList.Add(remainList[index - 1]);
                remainList.RemoveAt(index - 1);
            }
            cullList.Reverse();
        }

        public override Guid ComponentGuid => new Guid("{9e27debb-5e86-4606-973b-d9f0038d9a9f}");

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconCullNthPlus);
    }

}