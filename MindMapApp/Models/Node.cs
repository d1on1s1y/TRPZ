using System.Collections.Generic;
using System.Net.Mail;
using System.Windows;

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

        public void Move(double dx, double dy)
        {
            this.PosX += dx;
            this.PosY += dy;
        }

        // Повертаємо прямокутник вузла (розмір 80x40 фіксований у нас)
        public Rect GetBounds()
        {
            return new Rect(PosX, PosY, 80, 40);
        }
        public Node Clone()
        {
            return new Node
            {
                Id = 0,
                Text = this.Text + " (Копія)",
                Color = this.Color,
                PosX = this.PosX + 20,
                PosY = this.PosY + 20,
                MindMapId = this.MindMapId,
                RegionId = this.RegionId
            };
        }
    }
}