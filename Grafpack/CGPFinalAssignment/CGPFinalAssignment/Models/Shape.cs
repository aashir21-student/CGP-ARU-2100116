using System;
using System.Drawing;

namespace GrafPack.Models
{
    public abstract class Shape
    {
        public Guid ID { get; private set; }
        public Color StrokeColor { get; set; }
        public bool IsSelected { get; set; }

        public Shape()
        {
            ID = Guid.NewGuid();
            StrokeColor = Color.Black;
            IsSelected = false;
        }

        public abstract void Draw(Graphics graphics, Pen pen);
        public abstract void Move(int deltaX, int deltaY);
        public abstract void Rotate(double angle);
        public abstract bool Contains(Point point);
        public abstract void Reflect(bool horizontal); 

    }
}