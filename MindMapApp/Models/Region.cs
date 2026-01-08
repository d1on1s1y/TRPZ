using System.Collections.Generic;
using System.Windows; // <--- ДОДАЙ ЦЕЙ USING
using System.Linq;    // <--- ДОДАЙ ЦЕЙ USING (для Min/Max/Union)

namespace MindMapApp.Entities
{
    // 1. Додаємо спадкування від інтерфейсу
    public class Region : IMapComponent
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string BorderColor { get; set; } = "#FF0000";

        public int MindMapId { get; set; }
        public virtual MindMap MindMap { get; set; }

        public virtual ICollection<Node> Nodes { get; set; } = new List<Node>();

        // === 2. РЕАЛІЗАЦІЯ ІНТЕРФЕЙСУ ===

        // Рухаємо регіон = рухаємо всі його вузли
        public void Move(double dx, double dy)
        {
            if (Nodes == null) return;

            foreach (var node in Nodes)
            {
                // Викликаємо Move у кожного вузла
                node.Move(dx, dy);
            }
        }

        // Межі регіону = об'єднання меж всіх його вузлів
        public Rect GetBounds()
        {
            if (Nodes == null || Nodes.Count == 0) return Rect.Empty;

            // Беремо межі першого вузла
            Rect totalBounds = Nodes.First().GetBounds();

            // Об'єднуємо з усіма іншими
            foreach (var node in Nodes.Skip(1))
            {
                totalBounds.Union(node.GetBounds());
            }

            // Можна додати трохи відступу (padding), щоб рамка була ширша за вузли
            totalBounds.Inflate(10, 10);

            return totalBounds;
        }
        // ================================
    }
}