namespace MindMapApp.Entities
{
    public enum AttachmentType
    {
        Image,
        Video,
        Document
    }

    public class Attachment
    {
        public int Id { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public AttachmentType Type { get; set; }
        public int NodeId { get; set; }
        public virtual Node Node { get; set; }
    }
}