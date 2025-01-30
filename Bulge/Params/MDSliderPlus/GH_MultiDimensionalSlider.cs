
using GH_IO.Serialization;
using GH_IO.Types;
using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Grasshopper.My.Resources;
using Microsoft.VisualBasic.CompilerServices;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Bulge.Params.MDSliderPlus
{
    /// <exclude />
    public class GH_MultiDimensionalSlider : GH_Component, IGH_StateAwareObject
    {
        private Point3d m_value;
        private Interval m_x;
        private Interval m_y;
        private Interval m_z;

        public GH_MultiDimensionalSlider()
          : base("MD Slider +", "MD Slider  +", "A multidimensional slider", "Bulge", "Params")
        {
            m_value = new Point3d(0.5, 0.5, 0.5);
            m_x = new Interval(0.0, 1.0);
            m_y = new Interval(0.0, 1.0);
            m_z = new Interval(0.0, 1.0);
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            
            pManager.AddIntervalParameter("X Domain", "X", "X Domain", 0, new Interval(0.0, 1.0));
            pManager.AddIntervalParameter("Y Domain", "Y", "Y Domain", 0, new Interval(0.0, 1.0));
            pManager.AddNumberParameter("Z Value", "Z", "Z Value", 0, 0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) => pManager.AddPointParameter("Point", "P", "Output point", (GH_ParamAccess)1);


        public override void CreateAttributes() => m_attributes = new GH_MultiDimensionalSliderAttributes(this);


        /// <summary>Gets the normalized values of the slider.</summary>
        public Point3d Value
        {
            get => m_value;
            set => m_value = value;
        }

        public Interval XInterval
        {
            get => m_x;
            set => m_x = value;
        }

        public Interval YInterval
        {
            get => m_y;
            set => m_y = value;
        }

        public Interval ZInterval
        {
            get => m_z;
            set => m_z = value;
        }

        public double X => ((Interval)m_x).ParameterAt(m_value.X);

        public double Y => ((Interval)m_y).ParameterAt(m_value.Y);

        public double Z => ((Interval)m_z).ParameterAt(m_value.Z);


        public void DisplaySliderSettings()
        {
            GH_MDSliderDialog F = new GH_MDSliderDialog();
            GH_WindowsFormUtil.CenterFormOnCursor(F, true);
            F.LoadSettings(this);
            if (F.ShowDialog(Instances.DocumentEditor) != DialogResult.OK)
                return;
            TriggerAutoSave();
            RecordUndoEvent("Slider Settings");
            F.SaveSettings(this);
            ExpireSolution(true);
        }

        //protected override void CollectVolatileData_Custom()
        //{
        //    m_data.Clear();
        //    if (SliderMode == GH_MDSliderMode._2d)
        //        m_data.Append(new GH_Point(new Point3d(X, Y, 0.0)), new GH_Path(0));
        //    else
        //        m_data.Append(new GH_Point(new Point3d(X, Y, Z)), new GH_Path(0));


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Interval input_XInterval = Interval.Unset;
            Interval input_YInterval = Interval.Unset;
            double zValue = double.NaN;
            DA.GetData<Interval>(0, ref input_XInterval);
            DA.GetData<Interval>(1, ref input_YInterval);
            DA.GetData<double>(2, ref zValue);
            XInterval = input_XInterval;
            YInterval = input_YInterval;
            DA.SetData(0, new GH_Point(new Point3d(X, Y, zValue)));
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override Bitmap Icon => Properties.Resources.S44316481;

        public override Guid ComponentGuid => new Guid("{3c820811-49c7-43f5-9763-3556288f6fbd}");

        private bool WriteData(GH_IWriter writer)
        {
            writer.SetPoint3D("slider_value", new GH_Point3D(m_value.X, m_value.Y, m_value.Z));
            writer.SetInterval1D("x_domain", new GH_Interval1D(m_x.Min, m_x.Max));
            writer.SetInterval1D("y_domain", new GH_Interval1D(m_y.Min, m_y.Max));
            writer.SetInterval1D("z_domain", new GH_Interval1D(m_z.Min, m_z.Max));
            return true;
        }

        private bool ReadData(GH_IReader reader)
        {
            GH_Point3D point3D = reader.GetPoint3D("slider_value");
            m_value = new Point3d(point3D.x, point3D.y, point3D.z);
            GH_Interval1D interval1D1 = reader.GetInterval1D("x_domain");
            m_x = new Interval(interval1D1.a, interval1D1.b);
            GH_Interval1D interval1D2 = reader.GetInterval1D("y_domain");
            m_y = new Interval(interval1D2.a, interval1D2.b);
            GH_Interval1D interval1D3 = reader.GetInterval1D("z_domain");
            m_z = new Interval(interval1D3.a, interval1D3.b);
            int num = 0;
            reader.TryGetInt32("SliderMode", ref num);
            
            return true;
        }

        public override bool Write(GH_IWriter writer)
        {
            WriteData(writer);
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            ReadData(reader);
            return base.Read(reader);
        }

        public string SaveState()
        {
            GH_LooseChunk writer = new GH_LooseChunk("MDSlider");
            WriteData(writer);
            return writer.Serialize_Xml();
        }

        public void LoadState(string state)
        {
            try
            {
                GH_LooseChunk reader = new GH_LooseChunk("MDSlider");
                reader.Deserialize_Xml(state);
                ReadData(reader);
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                ProjectData.ClearProjectError();
            }
        }
    }
}
