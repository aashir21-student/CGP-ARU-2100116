using System;
using System.Drawing;
using GrafPack.Utilities;

namespace GrafPack.Models
{
    public class Square : Shape
    {
        public Point TopLeft { get; private set; } 
        public int SideLength { get; private set; } 
        private Point[] rotatedCorners;
        private double rotationAngle = 0;
        private Point Center => new Point(TopLeft.X + SideLength / 2, TopLeft.Y + SideLength / 2); 
        private const int HandleSize = 8;
        private const int MinimumSideLength = 5;

        public Square(Point topLeft, int sideLength)
        {
            if (sideLength <= 0) throw new ArgumentException("Side length must be greater than zero.", nameof(sideLength));
            TopLeft = topLeft;
            SideLength = sideLength;
            InitializeCorners();
        }

        private void InitializeCorners()
        {
            rotatedCorners = new Point[4];
            rotatedCorners[0] = TopLeft;
            rotatedCorners[1] = new Point(TopLeft.X + SideLength, TopLeft.Y);
            rotatedCorners[2] = new Point(TopLeft.X + SideLength, TopLeft.Y + SideLength);
            rotatedCorners[3] = new Point(TopLeft.X, TopLeft.Y + SideLength);
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
                    int penWidth = Math.Max(1, (int)pen.Width);

                    engine.DrawLineThick(rotatedCorners[0].X, rotatedCorners[0].Y, rotatedCorners[1].X, rotatedCorners[1].Y, pen.Color, penWidth);
                    engine.DrawLineThick(rotatedCorners[1].X, rotatedCorners[1].Y, rotatedCorners[2].X, rotatedCorners[2].Y, pen.Color, penWidth);
                    engine.DrawLineThick(rotatedCorners[2].X, rotatedCorners[2].Y, rotatedCorners[3].X, rotatedCorners[3].Y, pen.Color, penWidth);
                    engine.DrawLineThick(rotatedCorners[3].X, rotatedCorners[3].Y, rotatedCorners[0].X, rotatedCorners[0].Y, pen.Color, penWidth);
                }

                graphics.DrawImageUnscaled(bitmap, 0, 0);
                bitmap.Dispose();
            }
            catch (OutOfMemoryException)
            {
                graphics.DrawPolygon(pen, rotatedCorners);
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
            TopLeft = new Point(TopLeft.X + deltaX, TopLeft.Y + deltaY);
            for (int i = 0; i < rotatedCorners.Length; i++)
            {
                rotatedCorners[i] = new Point(rotatedCorners[i].X + deltaX, rotatedCorners[i].Y + deltaY);
            }
        }

        public override void Rotate(double angle)
        {
            angle = angle % 360;
            rotationAngle = angle;

            double radians = angle * (Math.PI / 180);
            Point[] corners = new Point[4];
            corners[0] = TopLeft;
            corners[1] = new Point(TopLeft.X + SideLength, TopLeft.Y);
            corners[2] = new Point(TopLeft.X + SideLength, TopLeft.Y + SideLength);
            corners[3] = new Point(TopLeft.X, TopLeft.Y + SideLength);

            Point center = Center;
            for (int i = 0; i < corners.Length; i++)
            {
                rotatedCorners[i] = RotatePoint(corners[i], center, radians);
            }
        }

        public override bool Contains(Point point)
        {
            if (rotationAngle == 0)
            {
                if (point.X < TopLeft.X || point.Y < TopLeft.Y) return false;
                if (point.X > TopLeft.X + SideLength || point.Y > TopLeft.Y + SideLength) return false;
                return true;
            }

            // For rotated squares, use cross product method to check if point is inside
            double area = GetPolygonArea(rotatedCorners);
            double area1 = GetPolygonArea(new Point[] { point, rotatedCorners[1], rotatedCorners[2] });
            double area2 = GetPolygonArea(new Point[] { rotatedCorners[0], point, rotatedCorners[2] });
            double area3 = GetPolygonArea(new Point[] { rotatedCorners[0], rotatedCorners[1], point });
            double area4 = GetPolygonArea(new Point[] { rotatedCorners[0], rotatedCorners[1], rotatedCorners[3] });
            double area5 = GetPolygonArea(new Point[] { point, rotatedCorners[1], rotatedCorners[3] });
            double area6 = GetPolygonArea(new Point[] { rotatedCorners[0], point, rotatedCorners[3] });

            // Check if point is in one of the two triangles that make up the square
            return (Math.Abs(area1 + area2 + area3 - area) < 1.0) || (Math.Abs(area4 + area5 + area6 - area) < 1.0);
        }

        private double GetPolygonArea(Point[] points)
        {
            if (points.Length < 3) return 0;
            double area = 0;
            for (int i = 0; i < points.Length; i++)
            {
                int j = (i + 1) % points.Length;
                area += points[i].X * points[j].Y;
                area -= points[j].X * points[i].Y;
            }
            return Math.Abs(area) / 2.0;
        }

        private Point RotatePoint(Point point, Point center, double radians)
        {
            int x = point.X - center.X;
            int y = point.Y - center.Y;
            int rotatedX = (int)(x * Math.Cos(radians) - y * Math.Sin(radians)) + center.X;
            int rotatedY = (int)(x * Math.Sin(radians) + y * Math.Cos(radians)) + center.Y;
            return new Point(rotatedX, rotatedY);
        }
        public Rectangle GetResizeHandle()
        {
            // Use the bottom-right corner for resize handle
            Point corner = rotatedCorners[2];
            return new Rectangle(
                corner.X - HandleSize / 2,
                corner.Y - HandleSize / 2,
                HandleSize,
                HandleSize);
        }

        public bool IsOverResizeHandle(Point point)
        {
            return GetResizeHandle().Contains(point);
        }

        public void Resize(int delta)
        {
            int newSize = SideLength + delta;
            if (newSize < MinimumSideLength) newSize = MinimumSideLength;

            int oldSize = SideLength;
            SideLength = newSize;

            // Recompute corners with new size
            if (rotationAngle == 0)
            {
                InitializeCorners();
            }
            else
            {
                // Update corners maintaining rotation
                Point[] corners = new Point[4];
                corners[0] = TopLeft;
                corners[1] = new Point(TopLeft.X + SideLength, TopLeft.Y);
                corners[2] = new Point(TopLeft.X + SideLength, TopLeft.Y + SideLength);
                corners[3] = new Point(TopLeft.X, TopLeft.Y + SideLength);

                double radians = rotationAngle * (Math.PI / 180);
                Point center = Center;
                for (int i = 0; i < corners.Length; i++)
                {
                    rotatedCorners[i] = RotatePoint(corners[i], center, radians);
                }
            }
        }

        public override void Reflect(bool horizontal)
        {
            Point center = Center;

            if (horizontal)
            {
                for (int i = 0; i < rotatedCorners.Length; i++)
                {
                    rotatedCorners[i] = new Point(rotatedCorners[i].X, center.Y - (rotatedCorners[i].Y - center.Y));
                }
            }
            else
            {
                for (int i = 0; i < rotatedCorners.Length; i++)
                {
                    rotatedCorners[i] = new Point(center.X - (rotatedCorners[i].X - center.X), rotatedCorners[i].Y);
                }
            }
        }

    }
}

