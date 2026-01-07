using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MindMapApp.Entities;

namespace MindMapApp.Factories
{
    public class ImageFactory : IAttachmentFactory
    {
        public Attachment CreateAttachment(string filePath, int nodeId)
        {
            return new Attachment
            {
                FilePath = filePath,
                FileName = System.IO.Path.GetFileName(filePath),
                Type = "IMAGE",
                NodeId = nodeId
            };
        }
        public UIElement CreatePreview(Attachment attachment)
        {
            try
            {
                var image = new Image();
                image.Width = 100; 
                image.Height = 100;
                image.Margin = new Thickness(5);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(attachment.FilePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                image.Source = bitmap;
                return image;
            }
            catch
            {
                return new TextBlock { Text = "[Помилка зображення]", Foreground = System.Windows.Media.Brushes.Red };
            }
        }
    }
}