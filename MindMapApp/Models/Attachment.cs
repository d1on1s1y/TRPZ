using System.ComponentModel.DataAnnotations.Schema;

namespace MindMapApp.Entities
{
    public class Attachment
    {
        public int Id { get; set; }
        public string FilePath { get; set; } // Шлях до файлу на диску
        public string FileName { get; set; } // Просто назва (наприклад "cat.png")
        public string Type { get; set; }     // "IMAGE" або "FILE"

        // Зовнішній ключ до вузла
        public int NodeId { get; set; }
        [ForeignKey("NodeId")]
        public virtual Node Node { get; set; }
    }
}