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
using System.IO;

#nullable disable
namespace Bulge
{
    public class CurrentFolderPath : GH_Component
    {
        public CurrentFolderPath()
          : base("Current Folder Path", "Current Folder Path", "Current Folder Path", "Bulge",
                SubCategoryName.PARAMS)
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Current Folder Path", "P", "Current Folder Path", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Document ghDoc = Grasshopper.Instances.ActiveCanvas.Document;
            if (ghDoc != null)
            {
                string ghFilePath = ghDoc.FilePath;
                if (ghFilePath != null)
                {
                    string folderPath = Path.GetDirectoryName(ghFilePath);
                    DA.SetData(0, folderPath);
                }
            }
        }

        public override Guid ComponentGuid => new Guid("{59fb6f63-3e6f-4fb5-9574-b5432a00a18c}");

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconCurrentFolderPath);
    }

}