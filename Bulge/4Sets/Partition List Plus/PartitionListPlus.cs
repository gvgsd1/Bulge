using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#nullable disable
namespace Bulge
{
    public class PartitionListPlus : GH_Component
    {
        public PartitionListPlus()
          : base("Partition List Plus", "Partition List Plus", "Partition list with options: wrap, repeat last, remain", "Bulge",
                SubCategoryName.SETS)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("List", "L", "List to partition", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Size", "S", "Size of partitions", GH_ParamAccess.list);
            ((GH_PersistentParam<GH_Integer>)pManager[1]).SetPersistentData(2, 1);
            pManager.AddIntegerParameter("Option", "O",
                @"Option: 0 - Wrap (Default), 1 - Repeat Last, 2 - Remain

                Example - 20 inputs with pattern (2,3):
                Wrap:        {0}(2), {1}(3), {2}(2), {3}(3) ...
                Repeat Last: {0}(2), {1}(3), {2}(3), {3}(3) ...
                Remain:      {0}(2), {1}(3), {2}(15)",
                GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Chunks", "C", "List chunks", GH_ParamAccess.tree);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<IGH_Goo> inList = new List<IGH_Goo>();
            List<int> sizeList = new List<int>();
            DA.GetDataList(0, inList);
            DA.GetDataList(1, sizeList);
            if (inList.Count == 0 || sizeList.Count == 0)
                return;

            int option = 0;
            if (!DA.GetData(2, ref option))
                return;

            int inCount = inList.Count;

            if (option == 1)
            {
                int currentSum = sizeList.Sum();
                int lastNumber = sizeList.Last();

                while (currentSum < inCount)
                {
                    sizeList.Add(lastNumber);
                    currentSum += lastNumber;
                }
            }
            else if (option == 2)
            {
                int target = inCount - sizeList.Sum();
                if (target > 0)
                {
                    sizeList.Add(target);
                }
            }

            int num1 = 0;
            int num2 = checked(sizeList.Count - 1);
            int index1 = 0;
            while (index1 <= num2)
            {
                if (sizeList[index1] < 0)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Partial list lengths cannot be negative.");
                    sizeList[index1] = 0;
                }
                checked { num1 += sizeList[index1]; }
                checked { ++index1; }
            }
            if (num1 == 0)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The sum-total of all partition sizes must be larger than zero");
            }
            else
            {
                int index2 = -1;
                int num3 = 0;
                GH_Structure<IGH_Goo> tree = new GH_Structure<IGH_Goo>();
                GH_Path ghPath = DA.ParameterTargetPath(0);
                int index3 = -1;
                do
                {
                    checked { ++index3; }
                    checked { ++index2; }
                    if (index2 >= sizeList.Count)
                        index2 = 0;
                    int num4 = sizeList[index2];
                    if (num4 == 0)
                    {
                        tree.EnsurePath(ghPath.AppendElement(index3));
                    }
                    else
                    {
                        int num5 = num3;
                        int num6 = Math.Min(checked(num3 + num4), inCount);
                        if (num5 < num6)
                        {
                            List<IGH_Goo> data = new List<IGH_Goo>();
                            int num7 = num5;
                            int num8 = checked(num6 - 1);
                            int index4 = num7;
                            while (index4 <= num8)
                            {
                                data.Add(inList[index4]);
                                checked { ++index4; }
                            }
                            tree.AppendRange(data, ghPath.AppendElement(index3));
                            num3 = num6;
                        }
                        else
                            break;
                    }
                }
                while (num3 < inCount);
                DA.SetDataTree(0, tree);
            }
        }

        public override Guid ComponentGuid => new Guid("{f6199a98-1611-4901-8462-192e70309c15}");

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconPartitionListPlus);
    }

}