using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace snek
{
    internal class Apple : Entity
    {
        public Apple(Point p, Color c) : base(p, c)
        {

        }

        internal void Render(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color), Position.X, Position.Y, Size.Width, Size.Height);
        }
    }
}
