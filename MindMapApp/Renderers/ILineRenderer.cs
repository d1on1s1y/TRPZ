using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MindMapApp.Entities;

namespace MindMapApp.Renderers
{
    public interface ILineRenderer
    {
        void Draw(Canvas canvas, Point start, Point end, Brush color);
    }
}