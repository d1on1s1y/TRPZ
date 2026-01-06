using System.Collections.Generic;

namespace MindMapApp.Entities
{
    public class Region
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string BorderColor { get; set; } = "#FF0000";

        public int MindMapId { get; set; }
        public virtual MindMap MindMap { get; set; }

        public virtual ICollection<Node> Nodes { get; set; } = new List<Node>();
    }
}