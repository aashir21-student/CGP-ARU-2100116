using System;
using System.Collections.Generic;
using System.Drawing;
using GrafPack.Utilities;

namespace GrafPack.Models
{
    public class Triangle : Shape
    {
        public Point Vertex1 { get; private set; }
        public Point Vertex2 { get; private set; }
        public Point Vertex3 { get; private set; }
        private const int HandleSize = 8;
        private const double CollinearityThreshold = 0.5;

        public Triangle(Point vertex1, Point vertex2, Point vertex3)
        {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
            Vertex3 = vertex3;
        }

        public override void Draw(Graphics graphics, Pen pen)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));

            try
            {
                int canvasWidth = (int)graphics.VisibleClipBounds.Width;
                int canvasHeight = (int)graphics.VisibleClipBounds.Height;

                if (canvasWidth <= 0 || canvasHeight <= 0) return;

                // Edge case: check if triangle is degenerate (collinear vertices)
                if (IsDegenerate())
                {
                    graphics.DrawLine(pen, Vertex1, Vertex2);
                    graphics.DrawLine(pen, Vertex2, Vertex3);
                    graphics.DrawLine(pen, Vertex3, Vertex1);
                }
                else
                {
                    Bitmap bitmap = new Bitmap(canvasWidth, canvasHeight);

                    using (CustomGraphicsEngine engine = new CustomGraphicsEngine(bitmap))
                    {
                        Point[] points = { Vertex1, Vertex2, Vertex3 };
                        engine.DrawPolygon(points, pen.Color);
                    }

                    graphics.DrawImageUnscaled(bitmap, 0, 0);
                    bitmap.Dispose();
                }
            }
            catch (OutOfMemoryException)
            {
                Point[] points = { Vertex1, Vertex2, Vertex3 };
                graphics.DrawPolygon(pen, points);
            }

            if (IsSelected)
            {
                graphics.FillRectangle(Brushes.Red, GetHandleRect(Vertex1));
                graphics.FillRectangle(Brushes.Red, GetHandleRect(Vertex2));
                graphics.FillRectangle(Brushes.Red, GetHandleRect(Vertex3));
            }
        }

        public override void Move(int deltaX, int deltaY)
        {
            Vertex1 = new Point(Vertex1.X + deltaX, Vertex1.Y + deltaY);
            Vertex2 = new Point(Vertex2.X + deltaX, Vertex2.Y + deltaY);
            Vertex3 = new Point(Vertex3.X + deltaX, Vertex3.Y + deltaY);
        }

        public override void Rotate(double angle)
        {
            angle = angle % 360;
            if (angle == 0) return;

            double radians = angle * (Math.PI / 180);
            Point center = GetCentroid();

            Vertex1 = RotatePoint(Vertex1, center, radians);
            Vertex2 = RotatePoint(Vertex2, center, radians);
            Vertex3 = RotatePoint(Vertex3, center, radians);
        }

        public override bool Contains(Point point)
        {
            // Edge case: degenerate triangle
            if (IsDegenerate()) return false;

            double area = GetArea(Vertex1, Vertex2, Vertex3);
            double area1 = GetArea(point, Vertex2, Vertex3);
            double area2 = GetArea(Vertex1, point, Vertex3);
            double area3 = GetArea(Vertex1, Vertex2, point);

            return Math.Abs(area - (area1 + area2 + area3)) < 0.01;
        }

        /// <summary>
        /// Checks if triangle is degenerate (collinear vertices)
        /// </summary>
        private bool IsDegenerate()
        {
            double area = GetArea(Vertex1, Vertex2, Vertex3);
            return area < CollinearityThreshold;
        }

        private Point GetCentroid()
        {
            int x = (Vertex1.X + Vertex2.X + Vertex3.X) / 3;
            int y = (Vertex1.Y + Vertex2.Y + Vertex3.Y) / 3;
            return new Point(x, y);
        }

        private Point RotatePoint(Point point, Point center, double radians)
        {
            int x = point.X - center.X;
            int y = point.Y - center.Y;
            int rotatedX = (int)(x * Math.Cos(radians) - y * Math.Sin(radians)) + center.X;
            int rotatedY = (int)(x * Math.Sin(radians) + y * Math.Cos(radians)) + center.Y;
            return new Point(rotatedX, rotatedY);
        }

        private double GetArea(Point a, Point b, Point c)
        {
            return Math.Abs((a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y)) / 2.0);
        }

        private Rectangle GetHandleRect(Point vertex)
        {
            return new Rectangle(vertex.X - HandleSize / 2, vertex.Y - HandleSize / 2, HandleSize, HandleSize);
        }
        public int? GetVertexIndexAt(Point point)
        {
            if (GetHandleRect(Vertex1).Contains(point)) return 1;
            if (GetHandleRect(Vertex2).Contains(point)) return 2;
            if (GetHandleRect(Vertex3).Contains(point)) return 3;
            return null;
        }
        public void UpdateVertex(int index, Point newPosition)
        {
            switch (index)
            {
                case 1: Vertex1 = newPosition; break;
                case 2: Vertex2 = newPosition; break;
                case 3: Vertex3 = newPosition; break;
            }
        }

        public override void Reflect(bool horizontal)
        {
            Point center = GetCentroid();

            if (horizontal)
            {
                Vertex1 = new Point(Vertex1.X, center.Y - (Vertex1.Y - center.Y));
                Vertex2 = new Point(Vertex2.X, center.Y - (Vertex2.Y - center.Y));
                Vertex3 = new Point(Vertex3.X, center.Y - (Vertex3.Y - center.Y));
            }
            else
            {
                Vertex1 = new Point(center.X - (Vertex1.X - center.X), Vertex1.Y);
                Vertex2 = new Point(center.X - (Vertex2.X - center.X), Vertex2.Y);
                Vertex3 = new Point(center.X - (Vertex3.X - center.X), Vertex3.Y);
            }
        }

    }
}
