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
        public static Random Random = new Random();
        public GraphicsPath Model { get; set; }
        public Heading CurrentHeading { get; set; } = Extensions.RandomEnumValue<Heading>();
        private readonly Snake SnakeGuy;
        private readonly int NumCells;
        private readonly int CellSize = 10;
        private readonly Timer Timer;
        private readonly int Interval = 10;
        private Apple Apple;


        public SnakeForm()
        {
            InitializeComponent();

            //ClientSize = new Size(400, 400);

            Timer = new Timer();
            Timer.Tick += Tick;
            Timer.Interval = Interval;
            Timer.Start();

            //handle input
            KeyDown += SnakeForm_KeyDown;

            //spawn in middle of screen
            SnakeGuy = new Snake(new Point(ClientSize.Width / 2 - 10, ClientSize.Height / 2 + 5), Color.Red);

            //determine number of cells in the grid
            NumCells = (Size.Width / CellSize) * (Size.Height / CellSize);

            //spawn initial food
            Apple = new Apple(new Point(Random.Next(0, ClientSize.Width / CellSize) * 10, Random.Next(0, ClientSize.Height / CellSize) * 10), Color.Green);

            SnakeGuy.Body = CreateSnake(1);
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

        private List<Entity> CreateSnake(int size)
        {
            var snake = new List<Entity>();

            for (int i = 0; i < size; i++)
                snake.Add(new Snake(new Point(SnakeGuy.Head.Position.X + i * CellSize, SnakeGuy.Head.Position.Y), Color.Red));

            return snake;
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
            bg.Graphics.Clear(Color.Black);
            Timer.Stop();
        }

        public void Tick(object sender, EventArgs e)
        {
            using (BufferedGraphicsContext bgc = new BufferedGraphicsContext())
            using (BufferedGraphics bg = bgc.Allocate(CreateGraphics(), ClientRectangle))
            {
                bg.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                bg.Graphics.Clear(Color.Black);

                DrawGrid(bg, Color.DarkSlateGray, NumCells, CellSize);



                //handle collision between snake and borders
                if (SnakeGuy.Head.Position.X < 0 ||
                    SnakeGuy.Head.Position.X >= ClientRectangle.Width ||
                    SnakeGuy.Head.Position.Y < 0 ||
                    SnakeGuy.Head.Position.Y >= ClientRectangle.Height)
                {
                    Gameover(bg, Color.White, ClientSize);
                }

                //handle collision between snake and body
                foreach (var s in SnakeGuy.Body.Where(x => x != SnakeGuy.Head))
                {
                    if (s.Equals(SnakeGuy.Head))
                        Gameover(bg, Color.White, ClientSize);
                }

                //handle collision between snake and apple
                if (SnakeGuy.Head.Position.X == Apple.Position.X && SnakeGuy.Head.Position.Y == Apple.Position.Y)
                {
                    SnakeGuy.AteApple = true;
                    Apple = new Apple(new Point(Random.Next(0, ClientSize.Width / CellSize) * 10, Random.Next(0, ClientSize.Height / CellSize) * 10), Color.Green);
                }

                Apple.Render(bg.Graphics);
                SnakeGuy.Tick(CurrentHeading, bg.Graphics);

                bg.Render();
            }
        }
    }
}
