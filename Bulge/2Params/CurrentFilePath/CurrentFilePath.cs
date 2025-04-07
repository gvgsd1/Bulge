using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Bulge
{
    public class CurrentFilePath : GH_Component
    {
        public CurrentFilePath()
          : base("Current File Path", "Current File Path", "Current File Path", "Bulge",
                SubCategoryName.PARAMS)
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Current File Path", "P", "Current File Path", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Document ghDoc = Grasshopper.Instances.ActiveCanvas.Document;
            if (ghDoc != null)
            {
                string ghFilePath = ghDoc.FilePath;
                if (ghFilePath != null)
                {
                    DA.SetData(0, ghFilePath);
                }
            }
        }

        public override Guid ComponentGuid => new Guid("{9d444bff-a626-4ea4-88f6-e017b3e540e0}");

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconFirstLastTreeBranch);
    }

}