using System.ComponentModel.DataAnnotations.Schema;

namespace MindMapApp.Entities
{
    public class Attachment
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }

        public int NodeId { get; set; }
        [ForeignKey("NodeId")]
        public virtual Node Node { get; set; }
    }
}