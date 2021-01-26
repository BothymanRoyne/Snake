using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace snek
{
    internal class Entity
    {
        public Point Position;
        public Color Color;
        public int DeltaX { get; set; }                      //amount traveled in X axis per tick
        public int DeltaY { get; set; }                      //amount traveled in Y axis per tick
        public int Speed { get; set; } = 10;                       //speed of shape
        public float Acceleration { get; set; } = 0.05f;       //acceleration of shape

        public Entity(Point p, Color c)
        {
            Position = p;
            Color = c;
        }

        //public virtual GraphicsPath GetPath()
        //{
        //    Matrix m = new Matrix();
        //    GraphicsPath gp = (GraphicsPath)Model.Clone();
        //    m.Translate(Position.X, Position.Y);
        //    gp.Transform(m);

        //    return gp;
        //}

        //public abstract void Render(Graphics g);

    }
}
