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
    public class CanvasDefault : GH_Component
    {
        public CanvasDefault()
          : base("Canvas Default",
                "Canvas Default",
                "Canvas Default",
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
            Grasshopper.GUI.Canvas.GH_Skin.LoadSkin();
        }

        protected override System.Drawing.Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconCanvasDefault);

        public override Guid ComponentGuid => new Guid("83FABD5F-8F0E-4B2F-9729-17844A8D4939");
    }
}