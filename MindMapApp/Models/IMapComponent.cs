using System.Windows; // Потрібно для Rect

namespace MindMapApp.Entities
{
    public interface IMapComponent
    {
        // Метод переміщення на dx, dy
        void Move(double dx, double dy);

        // Отримати межі об'єкта (щоб знати, де малювати рамку виділення)
        Rect GetBounds();
    }
}