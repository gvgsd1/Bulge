using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#nullable disable
namespace Bulge
{
    public class LastRestTreeBranch : GH_Component
    {
        public LastRestTreeBranch()
          : base("Last Last Tree Branch", "Last Last Tree Branch", "Last Last Tree Branch", "Bulge",
                SubCategoryName.SETS)
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree", "T", "Data Tree", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Last Branch", "L", "Last Branch", GH_ParamAccess.list);
            pManager.AddGenericParameter("Rest Branch", "R", "Rest Branch", GH_ParamAccess.tree);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<IGH_Goo> inTree = null;
            if (!DA.GetDataTree(0, out inTree))
                return;

            if (inTree == null || inTree.IsEmpty) return;

            IList<GH_Path> paths = inTree.Paths;

            if (paths.Count == 0) return;

            List<IGH_Goo> lastBranch = inTree[paths[paths.Count - 1]];

            GH_Structure<IGH_Goo> restBranches = new GH_Structure<IGH_Goo>();

            for (int i = 0; i < paths.Count - 1; i++)
            {
                var branch = inTree.get_Branch(paths[i]) as List<IGH_Goo>;
                restBranches.AppendRange(branch, paths[i]);
            }

            DA.SetDataList(0, lastBranch);
            DA.SetDataTree(1, restBranches);

        }

        public override Guid ComponentGuid => new Guid("{f8933635-d0a5-44f6-80cf-a2ebeeb536d0}");

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconLastRestTreeBranch);
    }

}