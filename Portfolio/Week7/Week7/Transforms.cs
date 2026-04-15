using System;
using System.Drawing;
using System.Windows.Forms;

namespace Week7
{
    public partial class Transforms : Form
    {
    public Transforms()
    {
        InitializeComponent();
        this.SetStyle(ControlStyles.ResizeRedraw, true);
        this.StartPosition = FormStartPosition.Manual;
        this.Location = new Point(0, 0);
        this.Width = 500;
        this.Height = 500;
        this.BackColor = Color.White;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        Graphics g = e.Graphics;

        Pen blackPen = new Pen(Color.Black);
        Font myFont = new Font("Helvetica", 9);
        Brush blackWriter = new SolidBrush(Color.Black);

        // Rectangle 1: 0 degrees (Original)
        PointF[] rect1 =
        {
            new PointF(50, 50),
            new PointF(120, 50),
            new PointF(120, 120),
            new PointF(50, 120)
        };
        PointF centre1 = new PointF(85, 85);
        PointF[] rotatedRect1 = Tmatrix.MatrixRotate(0, rect1, centre1);
        g.DrawPolygon(blackPen, rotatedRect1);
        g.DrawString("0°", myFont, blackWriter, 75, 135);

        // Rectangle 2: 45 degrees
        PointF[] rect2 =
        {
            new PointF(250, 50),
            new PointF(320, 50),
            new PointF(320, 120),
            new PointF(250, 120)
        };
        PointF centre2 = new PointF(285, 85);
        PointF[] rotatedRect2 = Tmatrix.MatrixRotate(45, rect2, centre2);
        g.DrawPolygon(blackPen, rotatedRect2);
        g.DrawString("45°", myFont, blackWriter, 270, 135);

        // Rectangle 3: 90 degrees
        PointF[] rect3 =
        {
            new PointF(50, 250),
            new PointF(120, 250),
            new PointF(120, 320),
            new PointF(50, 320)
        };
        PointF centre3 = new PointF(85, 285);
        PointF[] rotatedRect3 = Tmatrix.MatrixRotate(90, rect3, centre3);
        g.DrawPolygon(blackPen, rotatedRect3);
        g.DrawString("90°", myFont, blackWriter, 75, 335);

        // Rectangle 4: 135 degrees
        PointF[] rect4 =
        {
            new PointF(250, 250),
            new PointF(320, 250),
            new PointF(320, 320),
            new PointF(250, 320)
        };
        PointF centre4 = new PointF(285, 285);
        PointF[] rotatedRect4 = Tmatrix.MatrixRotate(135, rect4, centre4);
        g.DrawPolygon(blackPen, rotatedRect4);
        g.DrawString("135°", myFont, blackWriter, 265, 335);
    }
    }
}