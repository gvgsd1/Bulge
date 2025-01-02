using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulge
{
    public static class Utility_Render
    {
        public static void RenderOutlinesSimple(GH_Capsule m_capsule, Graphics G, GH_PaletteStyle style)
        {
            Pen pen4 = new Pen(style.Edge);
            pen4.LineJoin = LineJoin.Round;
            G.DrawPath(pen4, m_capsule.OutlineShape);
            pen4.Dispose();
        }

        public static void RenderOutlinesSimple1(GH_Capsule m_capsule, Graphics G, float zoom, GH_PaletteStyle style)
        {
            if (zoom * (float)m_capsule.MaxRadius < 1f || m_capsule.OutlineShape == null)
            {
                //if (zoom >= 0.5f)
                //{
                //    Pen pen = new Pen(style.Edge);
                //    Rectangle rect = GH_Convert.ToRectangle(m_capsule.Box);
                //    rect.Inflate(-1, -1);
                //    G.DrawRectangle(pen, rect);
                //    pen.Dispose();
                //}

                Pen pen2 = new Pen(style.Edge);
                G.DrawRectangle(pen2, GH_Convert.ToRectangle(m_capsule.Box));
                pen2.Dispose();
            }
            else
            {
                //if (zoom >= 0.5f)
                //{
                //    Pen pen3 = new Pen(style.Edge);
                //    G.DrawPath(pen3, m_capsule.OutlineShape);
                //    pen3.Dispose();
                //}

                Pen pen4 = new Pen(style.Edge);
                pen4.LineJoin = LineJoin.Round;
                G.DrawPath(pen4, m_capsule.OutlineShape);
                pen4.Dispose();
            }
        }
    }
}
