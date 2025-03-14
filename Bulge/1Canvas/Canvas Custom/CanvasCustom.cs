using System;
using System.Collections.Generic;
using System.Drawing;
using Bulge.Properties;
using Bulge.Utility;
using Grasshopper;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
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
            pManager.AddBooleanParameter("Mono Mode", "Mono", "Mono Mode: No Edge, No Shade, Just a single flat color", GH_ParamAccess.item, false);
            pManager.AddColourParameter("Mono Color", "MC", "Mono Color", GH_ParamAccess.item, Color.FromArgb(255, 255, 255));

            pManager.AddColourParameter("Back Color", "BC", "Back Color", GH_ParamAccess.item, Color.FromArgb(212, 208, 200));
            pManager.AddColourParameter("Edge Color", "EC", "Edge Color", GH_ParamAccess.item, Color.FromArgb(0, 0, 0));
            pManager.AddColourParameter("Grid Color", "GC", "Grid Color", GH_ParamAccess.item, Color.FromArgb(30, 0, 0, 0));
            pManager.AddIntegerParameter("Grid Width", "WS", "Grid Width Size, Default 150", GH_ParamAccess.item, 150);
            pManager.AddIntegerParameter("Grid Height", "HS", "Grid Height Size, Default 50", GH_ParamAccess.item, 50);

            pManager.AddColourParameter("Shade Color", "SC", "Shade Color", GH_ParamAccess.item, Color.FromArgb(80, 0, 0, 0));
            pManager.AddIntegerParameter("Shade Size", "SS", "Shade Size, Default 30", GH_ParamAccess.item, 30);

            pManager.AddColourParameter("Wire Color", "WC", "Wire Color", GH_ParamAccess.item, Color.FromArgb(150, 0, 0, 0));
            pManager.AddColourParameter("Wire Empty Color", "WEC", "Wire Empty Color", GH_ParamAccess.item, Color.FromArgb(180, 255, 60, 0));
            pManager.AddColourParameter("Wire Selected In Color", "WS1C", "Wire Selected In Color", GH_ParamAccess.item, Color.FromArgb(125, 210, 40));
            pManager.AddColourParameter("Wire Selected Out Color", "WS2C", "Wire Selected Out Color", GH_ParamAccess.item, Color.FromArgb(50, 0, 0, 0));

            pManager.AddColourParameter("Group Color", "GC", "Group Color", GH_ParamAccess.item, Color.FromArgb(150, 170, 135, 255));
            pManager.AddColourParameter("Panel Color", "PC", "Panel Color", GH_ParamAccess.item, Color.FromArgb(255, 250, 90));
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Boolean canvas_mono = null;
            GH_Colour canvas_mono_color = null;

            GH_Colour canvas_back = null;
            GH_Colour canvas_edge = null;
            GH_Colour canvas_grid = null;
            int canvas_grid_col = int.MinValue;
            int canvas_grid_row = int.MinValue;
            
            GH_Colour canvas_shade = null;
            int canvas_shade_size = int.MinValue;

            GH_Colour wire_default = null;
            GH_Colour wire_empty = null;
            GH_Colour wire_selected_a = null;
            GH_Colour wire_selected_b = null;

            GH_Colour group_back = null;
            GH_Colour panel_back = null;

            if (!DA.GetData<GH_Boolean>(0, ref canvas_mono)) return;
            if (!DA.GetData<GH_Colour>(1, ref canvas_mono_color)) return;

            if (!DA.GetData<GH_Colour>(2, ref canvas_back)) return;
            if (!DA.GetData<GH_Colour>(3, ref canvas_edge)) return;
            if (!DA.GetData<GH_Colour>(4, ref canvas_grid)) return;
            if (!DA.GetData<int>(5, ref canvas_grid_col)) return;
            if (!DA.GetData<int>(6, ref canvas_grid_row)) return;
            
            if (!DA.GetData<GH_Colour>(7, ref canvas_shade)) return;
            if (!DA.GetData<int>(8, ref canvas_shade_size)) return;

            if (!DA.GetData<GH_Colour>(9, ref wire_default)) return;
            if (!DA.GetData<GH_Colour>(10, ref wire_empty)) return;
            if (!DA.GetData<GH_Colour>(11, ref wire_selected_a)) return;
            if (!DA.GetData<GH_Colour>(12, ref wire_selected_b)) return;

            if (!DA.GetData<GH_Colour>(13, ref group_back)) return;
            if (!DA.GetData<GH_Colour>(14, ref panel_back)) return;

            GH_Skin.canvas_mono = canvas_mono.Value;
            GH_Skin.canvas_mono_color = canvas_mono_color.Value;

            GH_Skin.canvas_back = canvas_back.Value;
            GH_Skin.canvas_edge = canvas_edge.Value;
            GH_Skin.canvas_grid = canvas_grid.Value;
            GH_Skin.canvas_grid_col = canvas_grid_col;
            GH_Skin.canvas_grid_row = canvas_grid_row;

            GH_Skin.canvas_shade = canvas_shade.Value;
            GH_Skin.canvas_shade_size = canvas_shade_size;

            GH_Skin.wire_default = wire_default.Value;
            GH_Skin.wire_empty = wire_empty.Value;
            GH_Skin.wire_selected_a = wire_selected_a.Value;
            GH_Skin.wire_selected_b = wire_selected_b.Value;

            GH_Skin.group_back = group_back.Value;
            GH_Skin.panel_back = panel_back.Value;
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override System.Drawing.Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconCanvasCustom);

        public override Guid ComponentGuid => new Guid("E97AE96E-55F4-4184-8C75-5F56E99F1E76");
    }
}