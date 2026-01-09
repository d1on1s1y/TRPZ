using System.Collections.Generic;
using System.Net.Mail;

namespace MindMapApp.Entities
{
    public class Node
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;    
        public double PosX { get; set; }
        public double PosY { get; set; }
        public string Color { get; set; } = "#000000";
        public bool IsUrgent { get; set; } 
        public int MindMapId { get; set; }
        public virtual MindMap MindMap { get; set; }
        public int? RegionId { get; set; }
        public virtual Region Region { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

     
   
    }
}