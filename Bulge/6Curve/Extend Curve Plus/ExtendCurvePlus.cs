using Bulge.Properties;
using Bulge.Utility;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Bulge
{
    public class ExtendCurvePlus : GH_Component, IGH_InitCodeAware
    {
        public ExtendCurvePlus()
          : base("Extend Curve Plus", "Extend Curve +", "Extend Curve with a default 0 input", "Bulge",
                SubCategoryName.CURVE)
        {
        }

        void IGH_InitCodeAware.SetInitCode(string code)
        {
            AssignInitCodeToInputParameter(2, code);
            AssignInitCodeToInputParameter(3, code);
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "Curve to extend", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Type", "T", "Type of extension (0=Line, 1=Arc, 2=Smooth)", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Start", "L0", "Extension length at start of curve", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("End", "L1", "Extension length at end of curve", GH_ParamAccess.item, 0);
            if (!(pManager[1] is Param_Integer paramInteger))
                return;
            paramInteger.AddNamedValue("Line", 0);
            paramInteger.AddNamedValue("Arc", 1);
            paramInteger.AddNamedValue("Smooth", 2);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "Extended curve", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve destination1 = null;
            if (!DA.GetData(0, ref destination1))
                return;
            int destination2 = 0;
            if (!DA.GetData(1, ref destination2))
                return;
            double destination3 = 0.0;
            double destination4 = 0.0;
            if (!DA.GetData(2, ref destination3) || !DA.GetData(3, ref destination4))
                return;
            if (destination1.GetLength() + destination3 + destination4 <= 1E-64)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The negative extension length is longer than the curve");
            }
            else
            {
                CurveExtensionStyle style = 0;
                if (destination2 == 1)
                    style = (CurveExtensionStyle)1;
                if (destination2 == 2)
                    style = (CurveExtensionStyle)2;
                Curve crv = destination1;
                Curve data = destination3 >= 0.0 || destination4 < 0.0 ? this.Extend(this.Extend(crv, destination3, 0, style), destination4, 1, style) : this.Extend(this.Extend(crv, destination4, 1, style), destination3, 0, style);
                if (data == null)
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Curve extend failed for unknown reasons");
                else
                    DA.SetData(0, data);
            }
        }

        private Curve Extend(Curve crv, double length, int end, CurveExtensionStyle style)
        {
            if (crv == null)
                return null;
            if (Math.Abs(length) < 1E-32)
                return crv;
            if (length < 0.0)
            {
                if (end == 0)
                {
                    double length1 = crv.GetLength();
                    if (length >= length1)
                        return null;
                    double num1;
                    if (!crv.LengthParameter(-length, out num1))
                        return crv;
                    Curve curve = crv;
                    double num2 = num1;
                    Interval domain = crv.Domain;
                    double t1 = domain.T1;
                    return curve.Trim(num2, t1);
                }
                double length2 = crv.GetLength();
                if (length >= length2)
                    return null;
                double num3;
                if (!crv.LengthParameter(length2 + length, out num3))
                    return crv;
                Curve curve1 = crv;
                Interval domain1 = crv.Domain;
                double t0 = domain1.T0;
                double num4 = num3;
                return curve1.Trim(t0, num4);
            }
            CurveEnd curveEnd = (CurveEnd)1;
            if (end != 0)
                curveEnd = (CurveEnd)2;
            return crv.Extend(curveEnd, length, style) ?? crv;
        }

        public override Guid ComponentGuid => new Guid("{9fdb38b9-d02d-490d-bf28-68b146c9c7e5}");

        protected override Bitmap Icon => UtilityIcon.ResizeIcon(Resources.IconCloseParallelCurves);
    }

}