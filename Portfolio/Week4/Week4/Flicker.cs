using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Week4
{
    public partial class Flicker : Form
    {
        Rectangle rect;
        int x = 0;
        int y = 200;

        int dx = 1;
        int dy = 1;

        Bitmap backBuffer;

        public Flicker()
        {
            InitializeComponent();

            // Position the form
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);

            // Size the form
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.Width = 400;
            this.Height = 400;
            this.BackColor = Color.White;

            // Create the small rectangle object
            rect = new Rectangle(x, y, 50, 50);

            // Create back buffer
            backBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics frontGraphics = e.Graphics;

            Pen blackPen = new Pen(Color.Black);
            Brush redBrush = new SolidBrush(Color.Red);
            Brush whiteBrush = new SolidBrush(Color.White);
            Font myFont = new Font("Helvetica", 9);

            while (true)
            {
                // Draw everything to the back buffer first
                using (Graphics g = Graphics.FromImage(backBuffer))
                {
                    // Clear background
                    g.FillRectangle(whiteBrush, 0, 0, this.ClientSize.Width, this.ClientSize.Height);

                    // Update rectangle position
                    rect.Location = new Point(x, y);

                    // Draw moving rectangle
                    g.DrawRectangle(blackPen, rect);

                    // Draw message
                    g.DrawString("Moving rectangle", myFont, redBrush, 150, 150);
                }

                // Copy back buffer to front buffer
                frontGraphics.DrawImageUnscaled(backBuffer, 0, 0);

                // Bounce off left and right edges
                if (x <= 0)
                    dx = 1;
                if (x + rect.Width >= this.ClientSize.Width)
                    dx = -1;

                // Bounce off top and bottom edges
                if (y <= 0)
                    dy = 1;
                if (y + rect.Height >= this.ClientSize.Height)
                    dy = -1;

                // Move rectangle
                x += dx;
                y += dy;

                Thread.Sleep(10);
            }
        }
    }
}