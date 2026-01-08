using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MindMapApp.Entities;

namespace MindMapApp.Renderers
{
    public class OrthogonalLineRenderer : ILineRenderer
    {
        public void Draw(Canvas canvas, Point start, Point end, Brush color)
        {
            var points = new PointCollection();
            points.Add(start);
            Point C = new Point(start.X, end.Y);
            double dx_SE = end.X - start.X;
            double dy_SE = end.Y - start.Y;
            double dx_SC = C.X - start.X; 
            double dy_SC = C.Y - start.Y;
            double denominator = (dx_SE * dx_SE) + (dy_SE * dy_SE);
            double intersectionY;

            if (denominator < 0.0001)
            {
                intersectionY = start.Y;
            }
            else
            { 
                double t = (dx_SC * dx_SE) + (dy_SC * dy_SE) / denominator;
                intersectionY = start.Y + t * dy_SE;
            }
            points.Add(new Point(start.X, intersectionY));
            points.Add(new Point(end.X, intersectionY));
            points.Add(end);
            var polyline = new Polyline
            {
                Stroke = color,
                StrokeThickness = 2,
                Points = points,
                StrokeLineJoin = PenLineJoin.Round
            };

            Panel.SetZIndex(polyline, -10);
            canvas.Children.Add(polyline);
        }
    }
}