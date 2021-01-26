using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snek
{
    internal class SnakeObject : GameObject
    {
        public Size Size = new Size(10, 10); //size of one segment of our snake
        public List<GameObject> Body = new List<GameObject>();
        public GameObject Head => Body.Last();

        public Heading CurrentHeading { get; internal set; }

        public SnakeObject(Point pos, Color col) : base(pos, col)
        {
            Body.Add(new GameObject(pos, col));
        }

        public void Render(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color), Position.X, Position.Y, Size.Width, Size.Height);
        }

        public void Tick(Size sz, Heading newHeading, Graphics g)
        {
            if (newHeading != Heading.None)
            {
                // test if new heading is valid, if so update CurrentHeading
                var isValid = true;

                if (newHeading == Heading.Left && CurrentHeading == Heading.Right) isValid = false;
                if (newHeading == Heading.Right && CurrentHeading == Heading.Left) isValid = false;
                if (newHeading == Heading.Up && CurrentHeading == Heading.Down) isValid = false;
                if (newHeading == Heading.Down && CurrentHeading == Heading.Up) isValid = false;

                if (isValid)
                {
                    CurrentHeading = newHeading;
                }
            }
            
            switch (CurrentHeading)
            {
                case Heading.Left:
                    DeltaX = -Speed;
                    DeltaY = 0;
                    break;
                case Heading.Right:
                    DeltaX = Speed;
                    DeltaY = 0;
                    break;
                case Heading.Up:
                    DeltaY = -Speed;
                    DeltaX = 0;
                    break;
                case Heading.Down:
                    DeltaY = Speed;
                    DeltaX = 0;
                    break;
            }

            //foreach (GameObject s in Body)
            //{
                Body.Add(new SnakeObject(Head.Position, Head.Color));
                Head.Position.X += DeltaX;
                Head.Position.Y += DeltaY;
                Body.RemoveAt(0);
                Body.ForEach(s => g.FillRectangle(new SolidBrush(Color), s.Position.X, s.Position.Y, Size.Width, Size.Height));

            //}


            // use CurrentHeading from here on out
            // CurrentHeading == Heading.Left

            //if (Position.X + DeltaX < 0)
            //    Position.X = sz.Width;
            //else if (Position.X + DeltaX >= sz.Width)
            //    Position.X = 0;
            //else

            //if (Position.Y + DeltaY < 0)
            //    Position.Y = sz.Height;
            //else if (Position.Y + DeltaY >= sz.Height)
            //    Position.Y = 0;
            //else

        }

    }
}
