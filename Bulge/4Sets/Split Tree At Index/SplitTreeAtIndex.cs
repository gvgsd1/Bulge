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
    public class SplitTreeAtIndex : GH_Component
    {
        public SplitTreeAtIndex()
          : base("Split Tree at Index", "Split Tree at Index", "Split Tree at Index", "Bulge",
                SubCategoryName.SETS)
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree", "T", "Data Tree", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("Index", "i", "Index", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree A", "A", "Tree A", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Tree B", "B", "Tree B", GH_ParamAccess.tree);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {

            GH_Structure<IGH_Goo> inTree = null;
            int n = 0;
            if (!DA.GetDataTree(0, out inTree))
                return;

            if (!DA.GetData(1, ref n))
                return;

            if (inTree == null || inTree.IsEmpty || n < 1 || n >= inTree.Paths.Count) return;

            GH_Structure<IGH_Goo> renumberedTree = new GH_Structure<IGH_Goo>();

            for (int i = 0; i < inTree.Paths.Count; i++)
            {
                GH_Path newPath = new GH_Path(i + 1);
                var branch = inTree.get_Branch(inTree.Paths[i]) as List<IGH_Goo>;
                renumberedTree.AppendRange(branch, newPath);
            }

            GH_Structure<IGH_Goo> A = new GH_Structure<IGH_Goo>();
            GH_Structure<IGH_Goo> B = new GH_Structure<IGH_Goo>();

            for (int i = 0; i <= n - 1; i++)
            {
                var branch = renumberedTree.get_Branch(renumberedTree.Paths[i]) as List<IGH_Goo>;
                A.AppendRange(branch, renumberedTree.Paths[i]);
            }

            for (int i = n; i < renumberedTree.Paths.Count; i++)
            {
                var branch = renumberedTree.get_Branch(renumberedTree.Paths[i]) as List<IGH_Goo>;
                B.AppendRange(branch, renumberedTree.Paths[i]);
            }

            DA.SetDataTree(0, A);
            DA.SetDataTree(1, B);

        }

        public override Guid ComponentGuid => new Guid("{c1d6224c-dc5d-4a0f-a757-85eb663bcc27}");

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconSplitTreeAtIndex);
    }

}