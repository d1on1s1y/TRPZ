using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MindMapApp.Entities;

namespace MindMapApp.Factories
{
    public class DocumentFactory : IAttachmentFactory
    {
        public Attachment CreateAttachment(string filePath, int nodeId)
        {
            return new Attachment
            {
                FilePath = filePath,
                FileName = System.IO.Path.GetFileName(filePath),
                Type = "FILE",
                NodeId = nodeId
            };
        }

        public UIElement CreatePreview(Attachment attachment)
        {
            var border = new Border
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(5),
                Margin = new Thickness(5),
                Background = Brushes.WhiteSmoke,
                Width = 100,
                Height = 100
            };

            var textBlock = new TextBlock
            {
                Text = "📄\n" + attachment.FileName,
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center
            };

            border.Child = textBlock;
            return border;
        }
    }
}