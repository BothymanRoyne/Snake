using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snek
{
    internal class Snake : Entity
    {
        public List<Entity> Body = new List<Entity>();
        public Entity Head => Body.Last();
        public Heading CurrentHeading { get; internal set; }
        public int UpdateDelay { get; set; } = 10;
        private int _updateCount { get; set; } = 0;
        public bool AteApple { get; internal set; }

        public Snake(Point p, Color c) : base(p, c)
        {
            Body.Add(new Entity(p, c));
        }

        public override bool Equals(object obj)
        {
            Snake s = (obj as Snake);

            if (Position == s.Position)
                return true;
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        public void Render(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color), Position.X, Position.Y, Size.Width, Size.Height);
        }

        public void Tick(Heading newHeading, Graphics g)
        {
            if (newHeading != Heading.None)
            {
                // test if new heading is valid, if so update CurrentHeading
                var isValid = true;

                if (newHeading == Heading.Left && CurrentHeading == Heading.Right) isValid = false;
                if (newHeading == Heading.Right && CurrentHeading == Heading.Left) isValid = false;
                if (newHeading == Heading.Up && CurrentHeading == Heading.Down) isValid = false;
                if (newHeading == Heading.Down && CurrentHeading == Heading.Up) isValid = false;

                if (isValid) CurrentHeading = newHeading;
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

            _updateCount++;
            if (_updateCount >= UpdateDelay)
            {
                _updateCount = 0;
                Body.Add(new Snake(Head.Position, Head.Color));
                Head.Position.X += DeltaX;
                Head.Position.Y += DeltaY;
                if (!AteApple)
                    Body.RemoveAt(0);
                AteApple = false;
            }
            Body.ForEach(s => g.FillRectangle(new SolidBrush(Color), s.Position.X, s.Position.Y, Size.Width, Size.Height));
        }
    }
}