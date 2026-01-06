namespace MindMapApp.Entities
{
    public class Connection
    {
        public int Id { get; set; }

        public string LineStyle { get; set; } = "Solid"; // "Solid", "Dashed" і т.д.
        public double Thickness { get; set; } = 1.0;

        public int MindMapId { get; set; }
        public virtual MindMap MindMap { get; set; }

        public int FromNodeId { get; set; }
        public virtual Node FromNode { get; set; }

        public int ToNodeId { get; set; }
        public virtual Node ToNode { get; set; }
    }
}