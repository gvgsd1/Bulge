using System;
using System.Collections.Generic;
using System.Drawing;
using Bulge.Properties;
using Bulge.Utility;
using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Bulge
{
    public class CanvasClean : GH_Component
    {
        public CanvasClean()
          : base("Canvas Clean",
                "Canvas Clean",
                "Canvas Clean",
                "Bulge",
                SubCategoryName.CANVAS)
        {

        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Grasshopper.GUI.Canvas.GH_Skin.canvas_mono = true;
            Grasshopper.GUI.Canvas.GH_Skin.canvas_mono_color = Color.White;
        }

        protected override System.Drawing.Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconCanvasClean);

        public override Guid ComponentGuid => new Guid("eef6a8d8-4325-4689-b40a-a05d26fb6308");
    }
}