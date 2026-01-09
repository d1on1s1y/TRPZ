using System;
using System.Collections.Generic;

namespace MindMapApp.Entities
{
    public class MindMap
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Author { get; set; } = string.Empty;

        public virtual ICollection<Node> Nodes { get; set; } = new List<Node>();
        public virtual ICollection<Connection> Connections { get; set; } = new List<Connection>();
        public virtual ICollection<Region> Regions { get; set; } = new List<Region>();
    }
}