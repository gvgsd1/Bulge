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
    public class FirstRestTreeBranch : GH_Component
    {
        public FirstRestTreeBranch()
          : base("First Rest Tree Branch", "First Rest Tree Branch", "First Rest Tree Branch", "Bulge",
                SubCategoryName.SETS)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree", "T", "Data Tree", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("First Branch", "F", "First Branch", GH_ParamAccess.list);
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

            List<IGH_Goo> firstBranch = inTree[paths[0]];

            GH_Structure<IGH_Goo> restBranches = new GH_Structure<IGH_Goo>();

            for (int i = 1; i < paths.Count; i++)
            {
                var branch = inTree.get_Branch(paths[i]) as List<IGH_Goo>;
                restBranches.AppendRange(branch, paths[i]);
            }

            DA.SetDataList(0, firstBranch);
            DA.SetDataTree(1, restBranches);

        }

        public override Guid ComponentGuid => new Guid("{9721315e-ab37-43db-b7f8-e0fe8d67cf08}");

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconFirstRestTreeBranch);
    }

}