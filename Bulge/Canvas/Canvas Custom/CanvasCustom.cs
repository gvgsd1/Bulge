using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Bulge
{
    public class CanvasCustom : GH_Component
    {
        public CanvasCustom()
          : base("Canvas Custom",
                "Canvas Custom",
                "Canvas Custom",
                "Bulge",
                SubCategoryName.CANVAS)
        {

        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("Color", "C", "Color", GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            Grasshopper.GUI.Canvas.GH_Skin.canvas_back = Color.White;
            Grasshopper.GUI.Canvas.GH_Skin.canvas_edge = Color.White;
            Grasshopper.GUI.Canvas.GH_Skin.canvas_grid = Color.White;
            //Grasshopper.GUI.Canvas.GH_Skin.canvas_grid_col = 5;
            //Grasshopper.GUI.Canvas.GH_Skin.canvas_grid_row = 5;

            Grasshopper.GUI.Canvas.GH_Skin.canvas_mono = true;
            Grasshopper.GUI.Canvas.GH_Skin.canvas_mono_color = Color.White;
            Grasshopper.GUI.Canvas.GH_Skin.canvas_shade = Color.Transparent;
            //Grasshopper.GUI.Canvas.GH_Skin.canvas_shade_size = 5;

            Grasshopper.GUI.Canvas.GH_Skin.panel_back = Color.Red;
            Grasshopper.GUI.Canvas.GH_Skin.wire_default = Color.Blue;
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("E97AE96E-55F4-4184-8C75-5F56E99F1E76");
    }
}