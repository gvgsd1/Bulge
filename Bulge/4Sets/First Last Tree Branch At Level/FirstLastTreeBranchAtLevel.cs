using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Bulge
{
    public class FirstLastTreeBranchAtLevel : GH_Component
    {
        public FirstLastTreeBranchAtLevel()
          : base("First Last Tree Branch At Level", "First Last Tree Branch At Level", "First Last Tree Branch At Level. eg. {0; 1; *}, * means wild card", "Bulge",
                SubCategoryName.SETS)
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree", "T", "Data Tree", GH_ParamAccess.tree);
            pManager.AddTextParameter("Masks", "M", "Splitting masks", GH_ParamAccess.list);
            pManager[1].DataMapping = GH_DataMapping.Flatten;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("First Branch", "F", "First Branch", GH_ParamAccess.list);
            pManager.AddGenericParameter("Last Branch", "L", "Last Branch", GH_ParamAccess.list);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {

            GH_Structure<IGH_Goo> inTree = null;
            List<string> maskList = new List<string>();
            if (!DA.GetDataTree(0, out inTree) || !DA.GetDataList(1, maskList))
                return;
            if (maskList.Count == 0)
            {
                IList<GH_Path> paths = inTree.Paths;
                List<IGH_Goo> firstBranch = inTree[paths[0]];
                List<IGH_Goo> lastBranch = inTree[paths[paths.Count - 1]];

                DA.SetDataList(0, firstBranch);
                DA.SetDataList(1, lastBranch);
            }
            else
            {
                List<GH_TreeRules> ghTreeRulesList = new List<GH_TreeRules>();
                bool itemOnlyRuleFlag = false;

                foreach (string mask in maskList)
                {
                    string errorMsg = null;
                    GH_TreeRules ghTreeRules = GH_TreeRules.FromString(mask, ref errorMsg);
                    if (errorMsg != null)
                        this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, errorMsg);
                    if (ghTreeRules != null)
                    {
                        ghTreeRulesList.Add(ghTreeRules);
                        if (ghTreeRules.HasItemRule)
                            itemOnlyRuleFlag = true;
                    }
                }


                GH_Structure<IGH_Goo> outTree = new GH_Structure<IGH_Goo>();

                for (int branchInd = 0; branchInd < inTree.PathCount; branchInd++)
                {
                    GH_Path path = inTree.get_Path(branchInd);
                    List<IGH_Goo> branch = inTree.Branches[branchInd];
                    if (itemOnlyRuleFlag)
                    {
                        List<IGH_Goo> data1 = new List<IGH_Goo>();

                        for (int itemInd = 0; itemInd < branch.Count; itemInd++)
                        {

                            // check if item pass or replace with null

                            bool curItemFlag = false;

                            foreach (GH_TreeRules ghTreeRules in ghTreeRulesList)
                            {
                                if (ghTreeRules.Apply(path, itemInd))
                                {
                                    curItemFlag = true;
                                    break;
                                }
                            }
                            if (curItemFlag)
                            {
                                data1.Add(branch[itemInd]);
                            }
                            else
                            {
                                data1.Add(null);
                            }
                        }
                        outTree.AppendRange(data1, path);
                    }
                    else
                    {
                        bool branchFlag = false;

                        // check if whole branch pass or not

                        foreach (GH_TreeRules ghTreeRules in ghTreeRulesList)
                        {
                            if (ghTreeRules.Apply(path))
                            {
                                branchFlag = true;
                                break;
                            }
                        }
                        if (branchFlag)
                            outTree.AppendRange(branch, path);
                    }
                }

                IList<GH_Path> outPaths = outTree.Paths;
                List<IGH_Goo> firstBranch = outTree[outPaths[0]];
                List<IGH_Goo> lastBranch = outTree[outPaths[outPaths.Count - 1]];

                DA.SetDataList(0, firstBranch);
                DA.SetDataList(1, lastBranch);
            }
        }

        public override Guid ComponentGuid => new Guid("{6c030564-32ee-47d1-b809-6aa76973524a}");

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconFirstLastTreeBranchAtLevel);
    }

}