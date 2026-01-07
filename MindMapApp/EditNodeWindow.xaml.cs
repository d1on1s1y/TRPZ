using Microsoft.Win32;
using MindMapApp.Entities;
using MindMapApp.Factories;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MindMapApp
{
    public partial class EditNodeWindow : Window
    {
        public Node EditingNode { get; private set; }

        public EditNodeWindow(Node nodeToEdit)
        {
            InitializeComponent();
            EditingNode = nodeToEdit;

            NodeTextBox.Text = EditingNode.Text;
            foreach (ComboBoxItem item in ColorComboBox.Items)
            {
                if (item.Content.ToString() == EditingNode.Color)
                {
                    ColorComboBox.SelectedItem = item;
                    break;
                }
            }

            if (EditingNode.Attachments != null)
            {
                foreach (var att in System.Linq.Enumerable.ToList(EditingNode.Attachments))
                {
                    RenderAttachment(att);
                }
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            EditingNode.Text = NodeTextBox.Text;
            //тут буду міняти координати
            bool isXValid = int.TryParse(NodeXBox.Text, out int x);
            bool isYValid = int.TryParse(NodeYBox.Text, out int y);
            if (isXValid && isYValid)
            {
                EditingNode.PosX = x;
                EditingNode.PosY = y;
            }
            if (ColorComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                EditingNode.Color = selectedItem.Content.ToString();
            }

            DialogResult = true;
            Close();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.,-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private IAttachmentFactory GetFactory(string filePath)
        {
            string ext = System.IO.Path.GetExtension(filePath).ToLower();
            if (ext == ".jpg" || ext == ".png" || ext == ".bmp" || ext == ".jpeg")
                return new ImageFactory();
            else
                return new DocumentFactory();
        }
        private void RenderAttachment(Attachment attachment)
        {
            var factory = GetFactory(attachment.FilePath);
            var previewElement = factory.CreatePreview(attachment);
            var container = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(5),
                Width = 110
            };
            var deleteBtn = new Button
            {
                Content = "Видалити",
                FontSize = 10,
                Height = 20,
                Background = Brushes.IndianRed,
                Foreground = Brushes.White,
                Margin = new Thickness(0, 2, 0, 0),
                Cursor = System.Windows.Input.Cursors.Hand
            };
            deleteBtn.Click += (s, e) =>
            {
                AttachmentsPanel.Children.Remove(container);
                EditingNode.Attachments.Remove(attachment);
            };
            container.Children.Add(previewElement);
            container.Children.Add(deleteBtn);
            AttachmentsPanel.Children.Add(container);
        }

        private void AddAttachment_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                string path = dlg.FileName;
                var factory = GetFactory(path);
                var attachment = factory.CreateAttachment(path, EditingNode.Id);
                EditingNode.Attachments.Add(attachment);
                RenderAttachment(attachment);
            }
        }

    }
}