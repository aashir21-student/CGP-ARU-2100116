using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace Week3
{
    public partial class XORDrawing : Form
    {
        Rectangle aRect;
        Rectangle anEllipse;
        Rectangle moving;
        int x = 0, y = 0;
        Graphics g;

        public XORDrawing()
        {
            InitializeComponent();

            aRect = new Rectangle(100, 100, 200, 200);
            anEllipse = new Rectangle(150, 150, 200, 100);
            moving = new Rectangle(x, y, 10, 10);

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.Width = 500;
            this.Height = 500;
            this.BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;

            Brush redBrush = new SolidBrush(Color.Red);
            g.FillRectangle(redBrush, aRect);

            Brush greenBrush = new SolidBrush(Color.Green);
            g.FillEllipse(greenBrush, anEllipse);

            x = 490;
            y = 0;

            while (x >= 0 && y < 500)
            {
                moving.Location = this.PointToScreen(new Point(x, y));
                ControlPaint.FillReversibleRectangle(moving, Color.Red);
                Thread.Sleep(10);
                ControlPaint.FillReversibleRectangle(moving, Color.Red);

                x--;
                y++;
            }
        }
    }

    public class XORDemo
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new XORDrawing());
        }
    }
}