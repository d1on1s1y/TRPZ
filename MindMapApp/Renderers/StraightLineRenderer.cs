using MindMapApp.Entities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MindMapApp.Renderers
{
    public class StraightLineRenderer : ILineRenderer
    {
        public void Draw(Canvas canvas, Point start, Point end, Brush color)
        {
            var line = new Line
            {
                X1 = start.X,
                Y1 = start.Y,
                X2 = end.X,
                Y2 = end.Y,
                Stroke = color,
                StrokeThickness = 2
            };
            Panel.SetZIndex(line, -1);
            canvas.Children.Add(line);
        }
    }
}