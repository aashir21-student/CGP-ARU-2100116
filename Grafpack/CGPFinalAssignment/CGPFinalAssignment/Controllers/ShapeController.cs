using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GrafPack.Models;

namespace GrafPack.Controllers
{
    public class ShapeController
    {
        private readonly List<Shape> shapes = new List<Shape>();
        public IEnumerable<Shape> Shapes => shapes.AsReadOnly();

        public void AddShape(Shape shape)
        {
            if (shape == null) throw new ArgumentNullException(nameof(shape));
            shapes.Add(shape);
        }

        public bool RemoveShape(Shape shape)
        {
            if (shape == null) throw new ArgumentNullException(nameof(shape));
            return shapes.Remove(shape);
        }

        public void ClearShapes()
        {
            shapes.Clear();
        }

        public void DrawShapes(Graphics graphics)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));
            foreach (var shape in shapes)
            {
                using (Pen pen = new Pen(shape.IsSelected ? Color.Red : shape.StrokeColor))
                {
                    shape.Draw(graphics, pen);
                }
            }
        }

        public Shape GetShapeAtPoint(Point point)
        {
            for (int i = shapes.Count - 1; i >= 0; i--)
            {
                if (shapes[i].Contains(point))
                    return shapes[i];
            }
            return null;
        }

        public void DeselectAll()
        {
            foreach (var shape in shapes)
            {
                shape.IsSelected = false;
            }
        }

        public bool SelectShapeAtPoint(Point point)
        {
            DeselectAll();
            Shape shape = GetShapeAtPoint(point);
            if (shape != null)
            {
                shape.IsSelected = true;
                return true;
            }
            return false;
        }

        public Shape GetSelectedShape()
        {
            return shapes.FirstOrDefault(s => s.IsSelected);
        }

        public void MoveSelectedShape(int deltaX, int deltaY)
        {
            var selectedShape = GetSelectedShape();
            if (selectedShape != null)
            {
                selectedShape.Move(deltaX, deltaY);
            }
        }

        public void RotateSelectedShape(double angle)
        {
            var selectedShape = GetSelectedShape();
            if (selectedShape != null)
            {
                selectedShape.Rotate(angle);
            }
        }
        public void ReflectSelectedShape(bool horizontal)
        {
            var selectedShape = GetSelectedShape();
            if (selectedShape != null)
            {
                selectedShape.Reflect(horizontal);
            }
        }

    }
}