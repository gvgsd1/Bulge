using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Bulge
{
    public class PathMapperPlus : GH_Component
    {
        public PathMapperPlus()
          : base("Path Mapper Plus", "Path Mapper Plus", "Path Mapper with text inputs", "Bulge",
                SubCategoryName.SETS)
        {
            Lexers = new List<GH_LexerCombo>();
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree", "T", "Data Tree", GH_ParamAccess.tree);
            pManager.AddTextParameter("Input Path", "P1", "Input Path. Mapping from eg. {A; B; C} [D]", GH_ParamAccess.item);
            pManager.AddTextParameter("Output Path", "P2", "Output Path. Mapping to eg. {C; B; A} [D]", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree", "T", " Tree", GH_ParamAccess.tree);
        }

        public List<GH_LexerCombo> Lexers { get; }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            GH_Structure<IGH_Goo> inTree = null;
            if (!DA.GetDataTree(0, out inTree))
                return;

            string p1 = null;
            string p2 = null;
            if (!DA.GetData(1, ref p1))
                return;
            if (!DA.GetData(2, ref p2))
                return;

            GH_Structure<IGH_Goo> outTree = new GH_Structure<IGH_Goo>();

            GH_LexerCombo lexer = new GH_LexerCombo(p1, p2);

            GH_Lexer.PerformLexicalReplace(lexer.Source, lexer.Target, inTree, outTree);

            DA.SetDataTree(0, outTree);

        }


        public override Guid ComponentGuid => new Guid("{d0a2fbae-8207-4744-a902-0520b302441a}");

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconPathMapperPlus);
    }

}