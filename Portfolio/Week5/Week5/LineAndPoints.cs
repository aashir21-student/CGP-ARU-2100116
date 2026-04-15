using System;
using System.Drawing;
using System.Windows.Forms;

namespace Week5
{
    public partial class LineAndPoints : Form
    {
        private Point[] pts;
        private int[,] lines;

        public LineAndPoints()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.Width = 700;
            this.Height = 500;
            this.BackColor = Color.White;

            // Points table
            pts = new Point[]
            {
                new Point(220, 120), // 0
                new Point(380, 120), // 1
                new Point(380, 230), // 2
                new Point(220, 230), // 3
                new Point(500, 120), // 4
                new Point(500, 180), // 5
                new Point(380, 180), // 6
                new Point(220, 280), // 7
                new Point(500, 280)  // 8
            };

            // Line table
            lines = new int[,]
            {
                {0, 1},
                {1, 2},
                {2, 3},
                {3, 0},

                {1, 4},
                {4, 5},
                {5, 6},
                {6, 1},

                {3, 2},
                {2, 8},
                {8, 7},
                {7, 3}
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen blackPen = new Pen(Color.Black);

            for (int i = 0; i < lines.GetLength(0); i++)
            {
                g.DrawLine(blackPen, pts[lines[i, 0]], pts[lines[i, 1]]);
            }
        }
    }
}