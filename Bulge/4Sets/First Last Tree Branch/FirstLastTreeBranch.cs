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
    public class FirstLastTreeBranch : GH_Component
    {
        public FirstLastTreeBranch()
          : base("First Last Tree Branch", "First Last Tree Branch", "First Last Tree Branch", "Bulge",
                SubCategoryName.SETS)
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree", "T", "Data Tree", GH_ParamAccess.tree);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("First Branch", "F", "First Branch", GH_ParamAccess.list);
            pManager.AddGenericParameter("Last Branch", "L", "Last Branch", GH_ParamAccess.list);
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
            List<IGH_Goo> lastBranch = inTree[paths[paths.Count - 1]];

            DA.SetDataList(0, firstBranch);
            DA.SetDataList(1, lastBranch);

        }

        public override Guid ComponentGuid => new Guid("{62f948a9-2ab6-44cd-99a5-f42b8ba5d828}");

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconFirstLastTreeBranch);
    }

}