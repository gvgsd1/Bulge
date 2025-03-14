using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Bulge
{
    public class VectorFromAngle : GH_Component
    {
        private bool m_degrees;

        public VectorFromAngle()
          : base("Vector From Angle",
                "Vector From Angle",
                "Vector From Angle",
                "Bulge",
                SubCategoryName.VECTOR)
        {

        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane", "P", "Base Plane", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddNumberParameter("Angle", "A", "Rotation Angle (in Degrees)", GH_ParamAccess.item, 0.0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddVectorParameter("Vector", "V", "Vector", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

           

            //if (DA.Iteration == 0)
            //    this.m_degrees = ((Param_Number)this.Params.Input[2]).UseDegrees;
            Plane inPlane = Plane.Unset;
            double degrees = double.NaN;
            if (!DA.GetData<Plane>(0, ref inPlane) || !DA.GetData<double>(1, ref degrees) || !RhinoMath.IsValidDouble(degrees))
                return;
            //if (this.m_degrees)
            //    radians = RhinoMath.ToRadians(radians);
            double radians = RhinoMath.ToRadians(degrees);
            inPlane.Rotate(radians, inPlane.Normal);

            Vector3d outVector = inPlane.XAxis;
            DA.SetData(0, outVector);


        }

        protected override System.Drawing.Bitmap Icon => ResourcesVector.VectorRotate_24x24;

        public override Guid ComponentGuid => new Guid("eae33f0e-c2a2-4f17-b363-f2804d733f82");
    }
}


