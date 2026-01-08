using MindMapApp.Entities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MindMapApp.Renderers
{
    public class BezierLineRenderer : ILineRenderer
    {
        public void Draw(Canvas canvas, Point start, Point end, Brush color)
        {
            double deltaX = Math.Abs(end.X - start.X);
            double deltaY = Math.Abs(end.Y - start.Y);

            Point p1, p2;

            if (deltaX > deltaY)
            {
                double offset = deltaX / 2;
                p1 = new Point(start.X + offset, start.Y);
                p2 = new Point(end.X - offset, end.Y);
            }
            else
            {
                double offset = deltaY / 2;
                p1 = new Point(start.X, start.Y + offset);
                p2 = new Point(end.X, end.Y - offset);
            }

            var bezierSegment = new BezierSegment(p1, p2, end, true);
            var pathFigure = new PathFigure(start, new[] { bezierSegment }, false);
            var geometry = new PathGeometry(new[] { pathFigure });

            var path = new Path
            {
                Data = geometry,
                Stroke = color,
                StrokeThickness = 2
            };

            Panel.SetZIndex(path, -1);
            canvas.Children.Add(path);
        }
    }
}