using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Windows.Input;
using System.Diagnostics;

namespace snek
{
    public partial class SnakeForm : Form
    {
        public GraphicsPath Model { get; set; }
        public Heading CurrentHeading { get; set; } = Extensions.RandomEnumValue<Heading>();
        private readonly Snake SnakeGuy;
        private readonly int NumCells;
        private readonly int CellSize = 10;
        private readonly Timer Timer;
        private int Interval = 100;


        public SnakeForm()
        {
            InitializeComponent();

            Timer = new Timer();
            Timer.Tick += Tick;
            Timer.Enabled = true;
            Timer.Interval = Interval;
            Timer.Start();

            KeyDown += SnakeForm_KeyDown;

            SnakeGuy = new Snake(new Point(ClientSize.Width / 2 - 10, ClientSize.Height / 2 + 5), Color.Red);

            //testing
            SnakeGuy.Body.Add(new Snake(new Point(ClientSize.Width / 2, ClientSize.Height / 2 + 5), Color.Green));
            SnakeGuy.Body.Add(new Snake(new Point(ClientSize.Width / 2, ClientSize.Height / 2 + 5), Color.Green));
            SnakeGuy.Body.Add(new Snake(new Point(ClientSize.Width / 2, ClientSize.Height / 2 + 5), Color.Green));

            //Snake.Add(new SnakeObject(new Point(ClientSize.Width / 2 - 10, ClientSize.Height / 2 + 5), Color.Red));
            NumCells = (Size.Width / CellSize) * (Size.Height / CellSize);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void SnakeForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    CurrentHeading = Heading.Up;
                    break;
                case Keys.A:
                    CurrentHeading = Heading.Left;
                    break;
                case Keys.S:
                    CurrentHeading = Heading.Down;
                    break;
                case Keys.D:
                    CurrentHeading = Heading.Right;
                    break;
            }
        }

        private void DrawGrid(BufferedGraphics bg, Color c, int numCells, int cellSize)
        {
            var pen = new Pen(new SolidBrush(c));
            for (int x = 0; x < NumCells; x++)
            {
                bg.Graphics.DrawLine(pen, x * cellSize, 0, x * cellSize, numCells * cellSize);
                bg.Graphics.DrawLine(pen, 0, x * cellSize, numCells * cellSize, x * cellSize);
            }
        }

        private void Gameover(BufferedGraphics bg, Color c, Size clientSz)
        {
            var p = new PointF(clientSz.Width / 2 - 50, clientSz.Height / 2);

            bg.Graphics.DrawString("Gameover", new Font(FontFamily.GenericMonospace, 20), new SolidBrush(c), p);
        }

        public void Tick(object sender, EventArgs e)
        {
            using (BufferedGraphicsContext bgc = new BufferedGraphicsContext())
            using (BufferedGraphics bg = bgc.Allocate(CreateGraphics(), ClientRectangle))
            {
                bg.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                bg.Graphics.Clear(Color.Black);

                DrawGrid(bg, Color.DarkSlateGray, NumCells, CellSize);

                SnakeGuy.Tick(CurrentHeading, bg.Graphics);

                //i think the form should be checking where the snake is as the snake shouldn't care where it is
                //because then it would need to know form information
                //thoughts?
                if (SnakeGuy.Head.Position.X <= 0 ||
                    SnakeGuy.Head.Position.X >= ClientRectangle.Width ||
                    SnakeGuy.Head.Position.Y <= 0 ||
                    SnakeGuy.Head.Position.Y >= ClientRectangle.Height)
                {
                    Timer.Stop();
                    bg.Graphics.Clear(Color.Black);
                    Gameover(bg, Color.White, ClientSize);
                }

                bg.Render();
            }
        }
    }
}
