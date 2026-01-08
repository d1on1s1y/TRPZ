using System.Windows;

namespace MindMapApp.Entities
{
    public interface IMapComponent
    {
        void Move(double dx, double dy);
        Rect GetBounds();
    }
}