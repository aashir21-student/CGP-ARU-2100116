using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace GrafPack.Utilities
{
    /// <summary>
    /// Custom graphics rendering engine implementing drawing algorithms from first principles.
    /// Handles edge cases: degenerate shapes, boundary conditions, performance optimization.
    /// Uses Bresenham's line algorithm and Midpoint circle algorithm.
    /// </summary>
    public class CustomGraphicsEngine : IDisposable
    {
        private Bitmap bitmap;
        private int width;
        private int height;
        private const int MinimumLineLength = 1;
        private const int MinimumCircleRadius = 1;
        private bool disposed = false;

        public CustomGraphicsEngine(Bitmap bitmap)
        {
            this.bitmap = bitmap ?? throw new ArgumentNullException(nameof(bitmap));
            this.width = bitmap.Width;
            this.height = bitmap.Height;
        }

        /// <summary>
        /// Bresenham's Line Algorithm with endpoint handling
        /// Edge cases: handles all octants, vertical/horizontal lines, single pixels
        /// </summary>
        public void DrawLine(int x0, int y0, int x1, int y1, Color color)
        {
            // Edge case: single pixel
            if (x0 == x1 && y0 == y1)
            {
                SetPixelSafe(x0, y0, color);
                return;
            }

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            int x = x0;
            int y = y0;
            int colorValue = color.ToArgb();

            while (true)
            {
                SetPixelUnsafe(x, y, colorValue);

                if (x == x1 && y == y1) break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y += sy;
                }
            }
        }

        /// <summary>
        /// Midpoint Circle Algorithm with fill
        /// Edge cases: radius = 0, very small radius (1-3px), fills gaps
        /// </summary>
        public void DrawCircle(int centerX, int centerY, int radius, Color color)
        {
            // Edge case: invalid radius
            if (radius <= 0) return;

            int x = radius;
            int y = 0;
            int decisionParameter = 3 - 2 * radius;
            int colorValue = color.ToArgb();

            while (x >= y)
            {
                // Draw 8 symmetrical points with gap filling
                DrawCirclePoints(centerX, centerY, x, y, colorValue);

                y++;

                if (decisionParameter <= 0)
                {
                    decisionParameter = decisionParameter + 4 * y + 6;
                }
                else
                {
                    x--;
                    decisionParameter = decisionParameter + 4 * (y - x) + 10;
                }
            }

            // Edge case: fill potential gaps in very small circles
            if (radius <= 3)
            {
                FillCircleSmall(centerX, centerY, radius, colorValue);
            }
        }

        /// <summary>
        /// Draws all 8 symmetrical points of a circle
        /// </summary>
        private void DrawCirclePoints(int centerX, int centerY, int x, int y, int colorValue)
        {
            SetPixelUnsafe(centerX + x, centerY + y, colorValue);
            SetPixelUnsafe(centerX - x, centerY + y, colorValue);
            SetPixelUnsafe(centerX + x, centerY - y, colorValue);
            SetPixelUnsafe(centerX - x, centerY - y, colorValue);
            SetPixelUnsafe(centerX + y, centerY + x, colorValue);
            SetPixelUnsafe(centerX - y, centerY + x, colorValue);
            SetPixelUnsafe(centerX + y, centerY - x, colorValue);
            SetPixelUnsafe(centerX - y, centerY - x, colorValue);
        }

        /// <summary>
        /// Fill small circles to prevent gaps and aliasing
        /// Edge case handling for radius 1-3
        /// </summary>
        private void FillCircleSmall(int centerX, int centerY, int radius, int colorValue)
        {
            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    if (x * x + y * y <= radius * radius)
                    {
                        SetPixelUnsafe(centerX + x, centerY + y, colorValue);
                    }
                }
            }
        }

        /// <summary>
        /// Draws a filled circle using scanline algorithm
        /// </summary>
        public void FillCircle(int centerX, int centerY, int radius, Color color)
        {
            if (radius <= 0) return;

            int colorValue = color.ToArgb();
            
            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    if (x * x + y * y <= radius * radius)
                    {
                        SetPixelUnsafe(centerX + x, centerY + y, colorValue);
                    }
                }
            }
        }

        /// <summary>
        /// Draws a polygon by connecting vertices with lines
        /// Edge case: handles collinear vertices, self-intersecting polygons
        /// </summary>
        public void DrawPolygon(Point[] vertices, Color color)
        {
            if (vertices == null || vertices.Length < 2) return;

            // Edge case: filter out duplicate consecutive vertices
            var filtered = FilterDuplicateVertices(vertices);
            if (filtered.Length < 2) return;

            for (int i = 0; i < filtered.Length; i++)
            {
                int nextIndex = (i + 1) % filtered.Length;
                DrawLine(filtered[i].X, filtered[i].Y, 
                        filtered[nextIndex].X, filtered[nextIndex].Y, color);
            }
        }

        /// <summary>
        /// Removes consecutive duplicate vertices
        /// </summary>
        private Point[] FilterDuplicateVertices(Point[] vertices)
        {
            var result = new List<Point>();
            
            foreach (var vertex in vertices)
            {
                if (result.Count == 0 || result[result.Count - 1] != vertex)
                {
                    result.Add(vertex);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Draws a line with configurable thickness
        /// Edge case: thickness = 0 or 1 handled correctly
        /// </summary>
        public void DrawLineThick(int x0, int y0, int x1, int y1, Color color, int thickness)
        {
            // Edge case: clamp thickness
            if (thickness <= 0) thickness = 1;
            if (thickness == 1)
            {
                DrawLine(x0, y0, x1, y1, color);
                return;
            }

            int dx = x1 - x0;
            int dy = y1 - y0;
            double length = Math.Sqrt(dx * dx + dy * dy);

            // Edge case: handle zero-length lines
            if (length < 0.1)
            {
                FillRectangle(x0 - thickness / 2, y0 - thickness / 2, thickness, thickness, color);
                return;
            }

            // Draw perpendicular lines to create thickness
            double perpX = -dy / length;
            double perpY = dx / length;

            int halfThickness = thickness / 2;
            for (int offset = -halfThickness; offset <= halfThickness; offset++)
            {
                int offsetX = (int)(perpX * offset);
                int offsetY = (int)(perpY * offset);
                DrawLine(x0 + offsetX, y0 + offsetY, x1 + offsetX, y1 + offsetY, color);
            }
        }

        /// <summary>
        /// Draws a filled rectangle
        /// Edge case: negative dimensions handled correctly
        /// </summary>
        public void FillRectangle(int x, int y, int width, int height, Color color)
        {
            // Edge case: normalize negative dimensions
            if (width < 0)
            {
                x += width;
                width = -width;
            }
            if (height < 0)
            {
                y += height;
                height = -height;
            }

            if (width <= 0 || height <= 0) return;

            int colorValue = color.ToArgb();

            for (int row = y; row < y + height; row++)
            {
                for (int col = x; col < x + width; col++)
                {
                    SetPixelUnsafe(col, row, colorValue);
                }
            }
        }

        /// <summary>
        /// Sets pixel with bounds checking using SetPixel
        /// </summary>
        private void SetPixelSafe(int x, int y, Color color)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                bitmap.SetPixel(x, y, color);
            }
        }

        /// <summary>
        /// Sets pixel without bounds checking (assumes bounds already checked)
        /// </summary>
        private void SetPixelUnsafe(int x, int y, int colorValue)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                bitmap.SetPixel(x, y, Color.FromArgb(colorValue));
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
            }
        }
    }
}
