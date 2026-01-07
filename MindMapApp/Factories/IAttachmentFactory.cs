using System.Windows;
using MindMapApp.Entities;
namespace MindMapApp.Factories
{
    public interface IAttachmentFactory
    {
        Attachment CreateAttachment(string filePath, int nodeId);
       UIElement CreatePreview(Attachment attachment);
    }
}