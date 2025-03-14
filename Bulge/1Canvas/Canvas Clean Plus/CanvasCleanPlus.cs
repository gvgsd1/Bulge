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
    public class CanvasCleanPlus : GH_Component
    {
        public CanvasCleanPlus()
          : base("Canvas Clean +",
                "Canvas Clean +",
                "Canvas Clean +",
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

            Grasshopper.GUI.Canvas.GH_Skin.wire_default = Color.LightGray;
            Grasshopper.GUI.Canvas.GH_Skin.wire_empty = Color.White;
            Grasshopper.GUI.Canvas.GH_Skin.wire_selected_a = Color.Gray;
            Grasshopper.GUI.Canvas.GH_Skin.wire_selected_b = Color.Gray;

            Grasshopper.GUI.Canvas.GH_Skin.group_back = Color.White;
            Grasshopper.GUI.Canvas.GH_Skin.panel_back = Color.White;

        }

        protected override System.Drawing.Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconCanvasCleanPlus);

        public override Guid ComponentGuid => new Guid("fd6e29ac-44a7-46c0-a538-c2efd0b55823");
    }
}