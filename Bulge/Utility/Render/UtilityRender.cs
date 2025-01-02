
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace Bulge.Utility
{
    public static class UtilityRender
    {
        public static void Render(GH_Capsule capsule, Graphics G, Image icon, bool selected, bool locked, bool hidden)
        {
            GH_Palette m_palette = capsule.Palette;
            GH_PaletteStyle impliedStyle = GH_CapsuleRenderEngine.GetImpliedStyle(m_palette, selected, locked, hidden);
            Render(capsule, G, icon, impliedStyle);
        }

        public static void Render(GH_Capsule capsule, Graphics G, Image icon, Color colour)
        {
            GH_PaletteStyle style = new GH_PaletteStyle(colour);
            Render(capsule, G, icon, style);
        }

        public static void Render(GH_Capsule capsule, Graphics G, Image icon, GH_PaletteStyle style)
        {
            GH_CapsuleRenderEngine m_engine = capsule.RenderEngine;
            string m_text = capsule.Text;
            Font m_font = capsule.Font;
            float zoom = G.Transform.Elements[0];
            if (style.Fill.A < byte.MaxValue)
            {
                m_engine.RenderGrips_Alternative(G);
            }
            else
            {
                m_engine.RenderGrips(G);
            }

            m_engine.RenderBackground(G, zoom, style);
            //m_engine.RenderHighlight(G);
            //m_engine.RenderOutlines(G, zoom, style);
            RenderOutlinesSimple(capsule, G, style);
            if (icon != null)
            {
                m_engine.RenderIcon(G, icon);
            }
            else if (m_text != null && m_font != null)
            {
                m_engine.RenderText(G, style.Text);
            }
        }

        public static void RenderOutlinesSimple(GH_Capsule capsule, Graphics G, GH_PaletteStyle style)
        {
            Pen pen4 = new Pen(style.Edge);
            pen4.LineJoin = LineJoin.Round;
            G.DrawPath(pen4, capsule.OutlineShape);
            pen4.Dispose();
        }

        public static void RenderOutlinesSimple1(GH_Capsule capsule, Graphics G, float zoom, GH_PaletteStyle style)
        {
            if (zoom * (float)capsule.MaxRadius < 1f || capsule.OutlineShape == null)
            {
                //if (zoom >= 0.5f)
                //{
                //    Pen pen = new Pen(style.Edge);
                //    Rectangle rect = GH_Convert.ToRectangle(capsule.Box);
                //    rect.Inflate(-1, -1);
                //    G.DrawRectangle(pen, rect);
                //    pen.Dispose();
                //}

                Pen pen2 = new Pen(style.Edge);
                G.DrawRectangle(pen2, GH_Convert.ToRectangle(capsule.Box));
                pen2.Dispose();
            }
            else
            {
                //if (zoom >= 0.5f)
                //{
                //    Pen pen3 = new Pen(style.Edge);
                //    G.DrawPath(pen3, capsule.OutlineShape);
                //    pen3.Dispose();
                //}

                Pen pen4 = new Pen(style.Edge);
                pen4.LineJoin = LineJoin.Round;
                G.DrawPath(pen4, capsule.OutlineShape);
                pen4.Dispose();
            }
        }
    }
}
