using GrafPack.Controllers;
using GrafPack.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GrafPack
{
    public partial class GrafPack : Form
    {
        private ShapeController shapeController;
        private string currentMode = "None";
        private Point? firstClick = null;
        private List<Point> trianglePoints = new List<Point>();
        private bool isDragging = false;
        private bool isResizing = false;
        private bool overResizeHandle = false;
        private Point dragStart;
        private int? resizingVertexIndex = null;
        private bool isCreating = false;
        private Point creationStart;

        public GrafPack()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            shapeController = new ShapeController();

            drawingPanel.MouseClick += DrawingPanel_MouseClick;
            drawingPanel.Paint += DrawingPanel_Paint;
            drawingPanel.MouseDown += DrawingPanel_MouseDown;
            drawingPanel.MouseMove += DrawingPanel_MouseMove;
            drawingPanel.MouseUp += DrawingPanel_MouseUp;

            rotateMenuItem.Click += RotateMenuItem_Click;

            squareMenuItem.Click += (s, e) => SetMode("CreateSquare", "Click two points to draw a square.");
            circleMenuItem.Click += (s, e) => SetMode("CreateCircle", "Click center and then edge to draw a circle.");
            triangleMenuItem.Click += (s, e) => SetMode("CreateTriangle", "Click three points to draw a triangle.");
            selectMenuItem.Click += (s, e) => SetMode("Select", "Click on a shape to select it.");
            deleteMenuItem.Click += (s, e) => DeleteSelectedShape();
            exitMenuItem.Click += (s, e) => this.Close();

            reflectHorizontalMenuItem.Click += ReflectHorizontalMenuItem_Click;
            reflectVerticalMenuItem.Click += ReflectVerticalMenuItem_Click;

        }

        private void ReflectHorizontalMenuItem_Click(object sender, EventArgs e)
        {
            if (currentMode != "Select")
            {
                MessageBox.Show("Please select a shape first.");
                return;
            }

            shapeController.ReflectSelectedShape(true);
            drawingPanel.Invalidate();
            statusLabel.Text = "Shape reflected horizontally.";
        }

        private void ReflectVerticalMenuItem_Click(object sender, EventArgs e)
        {
            if (currentMode != "Select")
            {
                MessageBox.Show("Please select a shape first.");
                return;
            }

            shapeController.ReflectSelectedShape(false);
            drawingPanel.Invalidate();
            statusLabel.Text = "Shape reflected vertically.";
        }


        private void SetMode(string mode, string statusText)
        {
            currentMode = mode;
            firstClick = null;
            trianglePoints.Clear();
            resizingVertexIndex = null;
            statusLabel.Text = statusText;
        }

        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            shapeController.DrawShapes(e.Graphics);

            // Draw rubber-band preview for creation
            if (isCreating)
            {
                using (Pen previewPen = new Pen(Color.Gray) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                {
                    if (currentMode == "CreateSquare" && firstClick.HasValue)
                    {
                        Point start = creationStart;
                        Point mousePos = drawingPanel.PointToClient(Cursor.Position);
                        int side = Math.Min(Math.Abs(mousePos.X - start.X), Math.Abs(mousePos.Y - start.Y));
                        int left = Math.Min(start.X, mousePos.X);
                        int top = Math.Min(start.Y, mousePos.Y);
                        e.Graphics.DrawRectangle(previewPen, left, top, side, side);
                    }
                    else if (currentMode == "CreateCircle" && firstClick.HasValue)
                    {
                        Point center = creationStart;
                        Point mousePos = drawingPanel.PointToClient(Cursor.Position);
                        int radius = (int)Math.Sqrt(Math.Pow(mousePos.X - center.X, 2) + Math.Pow(mousePos.Y - center.Y, 2));
                        e.Graphics.DrawEllipse(previewPen, center.X - radius, center.Y - radius, radius * 2, radius * 2);
                    }
                    else if (currentMode == "CreateTriangle")
                    {
                        // Draw triangle preview based on locked first vertex and current mouse
                        Point mousePos = drawingPanel.PointToClient(Cursor.Position);
                        if (trianglePoints.Count >= 1)
                        {
                            // Compute a base perpendicular to the drag direction with vertex1 fixed at creationStart
                            Point v1 = trianglePoints[0];
                            // Direction vector from v1 to mouse
                            int dx = mousePos.X - v1.X;
                            int dy = mousePos.Y - v1.Y;
                            // Create two base points symmetric around the drag line perpendicular to direction
                            // Normalize perpendicular vector
                            int px = -dy;
                            int py = dx;
                            double len = Math.Sqrt(px * px + py * py);
                            if (len < 1) len = 1;
                            double scale = Math.Min(100, Math.Sqrt(dx * dx + dy * dy)) / len; // base half-length scales with drag
                            int halfBaseX = (int)(px * scale);
                            int halfBaseY = (int)(py * scale);

                            Point base1 = new Point(v1.X + halfBaseX, v1.Y + halfBaseY);
                            Point base2 = new Point(v1.X - halfBaseX, v1.Y - halfBaseY);
                            Point apex = mousePos;

                            Point[] pts = { base1, base2, apex };
                            e.Graphics.DrawPolygon(previewPen, pts);
                        }
                    }
                }
            }
        }

        private void DrawingPanel_MouseClick(object sender, MouseEventArgs e)
        {
            // If the user clicked on an existing shape, select it immediately
            var hitShape = shapeController.GetShapeAtPoint(e.Location);
            if (hitShape != null)
            {
                // Switch to Select mode and select the shape so subsequent clicks act on the shape
                SetMode("Select", "Shape selected.");
                shapeController.SelectShapeAtPoint(e.Location);
                drawingPanel.Invalidate();
                return;
            }

            // Triangle creation is drag-based: first click locks the first vertex; drag defines the other two vertices.
            // For now, handle only Select and discrete click modes not using rubber-band.

            if (currentMode == "Select")
            {
                shapeController.SelectShapeAtPoint(e.Location);
                drawingPanel.Invalidate();
                statusLabel.Text = "Shape selected.";
            }
        }

        private void DrawingPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentMode == "Select")
            {
                var shape = shapeController.GetSelectedShape();

                if (shape is Square square && square.IsOverResizeHandle(e.Location))
                {
                    isResizing = true;
                    dragStart = e.Location;
                    statusLabel.Text = "Resizing square...";
                }
                else if (shape is Circle circle && circle.IsOverResizeHandle(e.Location))
                {
                    isResizing = true;
                    dragStart = e.Location;
                    statusLabel.Text = "Resizing circle...";
                }
                else if (shape is Triangle triangle)
                {
                    int? vertexIndex = triangle.GetVertexIndexAt(e.Location);
                    if (vertexIndex.HasValue)
                    {
                        isResizing = true;
                        resizingVertexIndex = vertexIndex.Value;
                        dragStart = e.Location;
                        statusLabel.Text = $"Resizing vertex {vertexIndex.Value}...";
                    }
                    else
                    {
                        var found = shapeController.SelectShapeAtPoint(e.Location);
                        if (found)
                        {
                            isDragging = true;
                            dragStart = e.Location;
                            statusLabel.Text = "Dragging started...";
                        }
                    }
                }
                else
                {
                    var found = shapeController.SelectShapeAtPoint(e.Location);
                    if (found)
                    {
                        isDragging = true;
                        dragStart = e.Location;
                        statusLabel.Text = "Dragging started...";
                    }
                }
            }
            else if (currentMode == "CreateSquare" || currentMode == "CreateCircle")
            {
                // if clicking on an existing shape while in create mode, prefer selection instead of starting creation
                var hit = shapeController.GetShapeAtPoint(e.Location);
                if (hit != null)
                {
                    SetMode("Select", "Shape selected.");
                    shapeController.SelectShapeAtPoint(e.Location);
                    drawingPanel.Invalidate();
                    return;
                }

                // begin rubber-band creation
                isCreating = true;
                creationStart = e.Location;
                firstClick = e.Location;
                drawingPanel.Invalidate();
                statusLabel.Text = "Creating... drag to size, release to finish.";
            }
            else if (currentMode == "CreateTriangle")
            {
                // Start triangle creation: lock first vertex and begin dragging to define the base and apex
                var hit = shapeController.GetShapeAtPoint(e.Location);
                if (hit != null)
                {
                    SetMode("Select", "Shape selected.");
                    shapeController.SelectShapeAtPoint(e.Location);
                    drawingPanel.Invalidate();
                    return;
                }

                isCreating = true;
                creationStart = e.Location; // vertex1 locked
                trianglePoints.Clear();
                trianglePoints.Add(e.Location);
                statusLabel.Text = "Creating triangle... drag to size, release to finish.";
            }
        }

        private void DrawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            // Keep rubber-band preview updating while creating
            if (isCreating)
            {
                drawingPanel.Invalidate();
            }

            var selectedShape = shapeController.GetSelectedShape();

            if (currentMode == "Select")
            {
                if (!isDragging && !isResizing)
                {
                    if (selectedShape is Square square)
                    {
                        overResizeHandle = square.IsOverResizeHandle(e.Location);
                        drawingPanel.Cursor = overResizeHandle ? Cursors.SizeNWSE : Cursors.Default;
                    }
                    else if (selectedShape is Circle circle)
                    {
                        overResizeHandle = circle.IsOverResizeHandle(e.Location);
                        drawingPanel.Cursor = overResizeHandle ? Cursors.SizeWE : Cursors.Default;
                    }
                    else if (selectedShape is Triangle triangle)
                    {
                        int? vertexHit = triangle.GetVertexIndexAt(e.Location);
                        overResizeHandle = vertexHit.HasValue;
                        drawingPanel.Cursor = overResizeHandle ? Cursors.Hand : Cursors.Default;
                    }
                }
            }

            if (isDragging && selectedShape != null)
            {
                int dx = e.X - dragStart.X;
                int dy = e.Y - dragStart.Y;
                shapeController.MoveSelectedShape(dx, dy);
                dragStart = e.Location;
                drawingPanel.Invalidate();
            }

            if (isResizing)
            {
                int dx = e.X - dragStart.X;

                if (selectedShape is Square s)
                    s.Resize(dx);
                else if (selectedShape is Circle c)
                    c.Resize(dx);
                else if (selectedShape is Triangle t && resizingVertexIndex.HasValue)
                    t.UpdateVertex(resizingVertexIndex.Value, e.Location);

                dragStart = e.Location;
                drawingPanel.Invalidate();
            }
        }

        private void DrawingPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (isCreating)
            {
                // finalize creation based on mode
                if (currentMode == "CreateSquare")
                {
                    Point start = creationStart;
                    Point end = e.Location;
                    int side = Math.Min(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y));

                    if (side <= 0)
                    {
                        statusLabel.Text = "Square creation cancelled: drag to create a valid square.";
                    }
                    else
                    {
                        int left = Math.Min(start.X, end.X);
                        int top = Math.Min(start.Y, end.Y);
                        Square square = new Square(new Point(left, top), side);
                        shapeController.AddShape(square);
                        statusLabel.Text = "Square created.";
                    }
                }
                else if (currentMode == "CreateCircle")
                {
                    Point center = creationStart;
                    Point edge = e.Location;
                    int radius = (int)Math.Sqrt(Math.Pow(edge.X - center.X, 2) + Math.Pow(edge.Y - center.Y, 2));

                    if (radius <= 0)
                    {
                        statusLabel.Text = "Circle creation cancelled: drag to create a valid circle.";
                    }
                    else
                    {
                        Circle circle = new Circle(center, radius);
                        shapeController.AddShape(circle);
                        statusLabel.Text = "Circle created.";
                    }
                }
                else if (currentMode == "CreateTriangle")
                {
                    // For triangle, creationStart is v1, at mouse up compute base points and apex as in preview
                    Point v1 = creationStart;
                    Point apex = e.Location;
                    int dx = apex.X - v1.X;
                    int dy = apex.Y - v1.Y;

                    // Check if triangle is too small (essentially a point)
                    if (dx == 0 && dy == 0)
                    {
                        statusLabel.Text = "Triangle creation cancelled: drag to create a valid triangle.";
                    }
                    else
                    {
                        int px = -dy;
                        int py = dx;
                        double len = Math.Sqrt(px * px + py * py);
                        if (len < 1) len = 1;
                        double scale = Math.Min(100, Math.Sqrt(dx * dx + dy * dy)) / len;
                        int halfBaseX = (int)(px * scale);
                        int halfBaseY = (int)(py * scale);

                        Point base1 = new Point(v1.X + halfBaseX, v1.Y + halfBaseY);
                        Point base2 = new Point(v1.X - halfBaseX, v1.Y - halfBaseY);

                        Triangle triangle = new Triangle(base1, base2, apex);
                        shapeController.AddShape(triangle);
                        statusLabel.Text = "Triangle created.";
                    }
                }

                isCreating = false;
                firstClick = null;
                drawingPanel.Invalidate();
                return;
            }

            if (isDragging)
            {
                isDragging = false;
                statusLabel.Text = "Shape moved.";
            }

            if (isResizing)
            {
                isResizing = false;
                resizingVertexIndex = null;
                statusLabel.Text = "Shape resized.";
            }
        }

        private void RotateMenuItem_Click(object sender, EventArgs e)
        {
            if (currentMode != "Select")
            {
                MessageBox.Show("Please select a shape first using Select mode.");
                return;
            }

            var selectedShape = shapeController.GetSelectedShape();
            if (selectedShape == null)
            {
                MessageBox.Show("No shape is currently selected.");
                return;
            }

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter rotation angle in degrees (e.g., 45 or -30):",
                "Rotate Shape",
                "45");

            if (double.TryParse(input, out double angle))
            {
                shapeController.RotateSelectedShape(angle);
                drawingPanel.Invalidate();
                statusLabel.Text = $"Shape rotated by {angle}°.";
            }
            else
            {
                MessageBox.Show("Invalid angle entered.");
            }
        }

        private void DeleteSelectedShape()
        {
            Shape selected = shapeController.GetSelectedShape();
            if (selected != null)
            {
                shapeController.RemoveShape(selected);
                statusLabel.Text = "Shape deleted.";
                drawingPanel.Invalidate();
            }
            else
            {
                statusLabel.Text = "No shape selected to delete.";
            }
        }

        private void GrafPack_Load(object sender, EventArgs e)
        {
        }
    }
}
