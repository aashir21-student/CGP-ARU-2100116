using System;
using System.Drawing;
using System.Windows.Forms;

namespace CGP
{
    public partial class Triangles : Form
    {
        public Triangles()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.Width = 650;
            this.Height = 600;
            this.BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen blackPen = new Pen(Color.Black);

            PointF p1 = new PointF(100, 100);
            PointF p2 = new PointF(500, 100);
            PointF p3 = new PointF(300, 446);

            DrawRecursiveTriangle(g, blackPen, p1, p2, p3);
        }

        private void DrawRecursiveTriangle(Graphics g, Pen pen, PointF a, PointF b, PointF c)
        {
            g.DrawLine(pen, a, b);
            g.DrawLine(pen, b, c);
            g.DrawLine(pen, c, a);

            float side1 = Distance(a, b);
            float side2 = Distance(b, c);
            float side3 = Distance(c, a);

            if (side1 < 1 || side2 < 1 || side3 < 1)
                return;

            PointF abMid = MidPoint(a, b);
            PointF bcMid = MidPoint(b, c);
            PointF caMid = MidPoint(c, a);

            DrawRecursiveTriangle(g, pen, abMid, bcMid, caMid);
        }

        private PointF MidPoint(PointF p1, PointF p2)
        {
            return new PointF((p1.X + p2.X) / 2f, (p1.Y + p2.Y) / 2f);
        }

        private float Distance(PointF p1, PointF p2)
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}