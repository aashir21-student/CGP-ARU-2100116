using System;
using System.Drawing;

namespace Week7
{
    public class Tmatrix
    {
        public static PointF MatrixRotate(float angle, PointF pt)
        {
            float radians = (float)(angle * Math.PI / 180.0);
            float cosA = (float)Math.Cos(radians);
            float sinA = (float)Math.Sin(radians);

            float x = pt.X * cosA - pt.Y * sinA;
            float y = pt.X * sinA + pt.Y * cosA;

            return new PointF(x, y);
        }

        public static PointF[] MatrixRotate(float angle, PointF[] points, PointF centre)
        {
            PointF[] rotatedPoints = new PointF[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                float translatedX = points[i].X - centre.X;
                float translatedY = points[i].Y - centre.Y;

                PointF rotated = MatrixRotate(angle, new PointF(translatedX, translatedY));

                rotatedPoints[i] = new PointF(rotated.X + centre.X, rotated.Y + centre.Y);
            }

            return rotatedPoints;
        }
    }
}