using System;
using System.Drawing;
using GrafPack.Utilities;

namespace GrafPack.Models
{
    public class Circle : Shape
    {
        public Point Center { get; private set; } 
        public int Radius { get; private set; } 
        private const int HandleSize = 8;
        private const int MinimumRadius = 5;

        public Circle(Point center, int radius)
        {
            if (radius <= 0) throw new ArgumentException("Radius must be greater than zero.", nameof(radius));
            Center = center;
            Radius = radius;
        }

        public override void Draw(Graphics graphics, Pen pen)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));

            try
            {
                int canvasWidth = (int)graphics.VisibleClipBounds.Width;
                int canvasHeight = (int)graphics.VisibleClipBounds.Height;

                if (canvasWidth <= 0 || canvasHeight <= 0) return;

                Bitmap bitmap = new Bitmap(canvasWidth, canvasHeight);

                using (CustomGraphicsEngine engine = new CustomGraphicsEngine(bitmap))
                {
                    engine.DrawCircle(Center.X, Center.Y, Radius, pen.Color);
                }

                graphics.DrawImageUnscaled(bitmap, 0, 0);
                bitmap.Dispose();
            }
            catch (OutOfMemoryException)
            {
                graphics.DrawEllipse(pen, Center.X - Radius, Center.Y - Radius, Radius * 2, Radius * 2);
            }

            if (IsSelected)
            {
                Rectangle handle = GetResizeHandle();
                graphics.FillRectangle(Brushes.Red, handle);
                graphics.DrawRectangle(Pens.DarkRed, handle);
            }
        }

        public override void Move(int deltaX, int deltaY)
        {
            Center = new Point(Center.X + deltaX, Center.Y + deltaY);
        }

        public override void Rotate(double angle)
        {
            // Rotation has no visible effect on a perfect circle
            return;
        }

        public override bool Contains(Point point)
        {
            double distance = Math.Sqrt(Math.Pow(point.X - Center.X, 2) + Math.Pow(point.Y - Center.Y, 2));
            return distance <= Radius;
        }
        public Rectangle GetResizeHandle()
        {
            return new Rectangle(
                Center.X + Radius - HandleSize / 2,
                Center.Y - HandleSize / 2,
                HandleSize,
                HandleSize);
        }
        public bool IsOverResizeHandle(Point point)
        {
            return GetResizeHandle().Contains(point);
        }

        public void Resize(int delta)
        {
            int newRadius = Radius + delta;
            if (newRadius < MinimumRadius) newRadius = MinimumRadius;
            Radius = newRadius;
        }

        public override void Reflect(bool horizontal)
        {
            if (horizontal)
            {
                // Flip over horizontal axis (Y-axis reflection)
                Center = new Point(Center.X, -Center.Y + 2 * GetReflectionAnchor().Y);
            }
            else
            {
                // Flip over vertical axis (X-axis reflection)
                Center = new Point(-Center.X + 2 * GetReflectionAnchor().X, Center.Y);
            }
        }

        private Point GetReflectionAnchor()
        {
            return Center;
        }

    }
}
