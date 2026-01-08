using System.Collections.Generic;
using System.Windows; 
using System.Linq;  

namespace MindMapApp.Entities
{
    public class Region : IMapComponent
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string BorderColor { get; set; } = "#FF0000";

        public int MindMapId { get; set; }
        public virtual MindMap MindMap { get; set; }

        public virtual ICollection<Node> Nodes { get; set; } = new List<Node>();
        public void Move(double dx, double dy)
        {
            if (Nodes == null) return;

            foreach (var node in Nodes)
            {
                node.Move(dx, dy);
            }
        }
        public Rect GetBounds()
        {
            if (Nodes == null || Nodes.Count == 0) return Rect.Empty;
            Rect totalBounds = Nodes.First().GetBounds();
            foreach (var node in Nodes.Skip(1))
            {
                totalBounds.Union(node.GetBounds());
            }
            totalBounds.Inflate(10, 10);

            return totalBounds;
        }
    }
}