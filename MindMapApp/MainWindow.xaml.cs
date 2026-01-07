using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MindMapApp.Entities;
using MindMapApp.Repositories;
using MindMapApp.Tools;

namespace MindMapApp
{
    public partial class MainWindow : Window
    {
        public MindMapRepository Repository { get; private set; }
        public MindMap CurrentMap { get; private set; }
        private IMapTool _currentTool;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Repository = new MindMapRepository();
                LoadMapsList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критична помилка запуску:\n{ex.Message}\n\nДеталі:\n{ex.InnerException?.Message}");
            }
            _currentTool = new SelectionTool();
            SetTool(new SelectionTool(), BtnSelect);
        }
        private void LoadMapsList()
        {
            var maps = Repository.GetAll();
            MapsListBox.ItemsSource = maps;
        }
        private void CreateMap_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewMapTitleBox.Text)) return;

            var newMap = new MindMap { Title = NewMapTitleBox.Text };
            Repository.Add(newMap);

            NewMapTitleBox.Text = "";
            LoadMapsList();
        }
        private void MapsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MapsListBox.SelectedItem is MindMap selectedMap)
            {
                CurrentMap = Repository.GetById(selectedMap.Id);

                CurrentMapLabel.Text = CurrentMap.Title;
                BtnAddNode.IsEnabled = true;
                ToolsPanel.IsEnabled = true;
                DrawCurrentMap();
            }
        }
        private void AddNode_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentMap == null) return;

            var newNode = new Node
            {
                Text = "Новий вузол",
                PosX = 100,
                PosY = 100,
                Color = "#ADD8E6",
                MindMapId = CurrentMap.Id
            };
            CurrentMap.Nodes.Add(newNode);
            Repository.Update(CurrentMap);

            DrawCurrentMap();
        }

        public void DrawCurrentMap()
        {
            DrawingCanvas.Children.Clear();

            if (CurrentMap == null) return;

            if (CurrentMap.Connections != null)
            {
                foreach (var conn in CurrentMap.Connections)
                {
                    var fromNode = CurrentMap.Nodes.FirstOrDefault(n => n.Id == conn.FromNodeId);
                    var toNode = CurrentMap.Nodes.FirstOrDefault(n => n.Id == conn.ToNodeId);

                    if (fromNode != null && toNode != null)
                    {
                        var line = new System.Windows.Shapes.Line
                        {
                            X1 = fromNode.PosX + 40,
                            Y1 = fromNode.PosY + 20,
                            X2 = toNode.PosX + 40,
                            Y2 = toNode.PosY + 20,
                            Stroke = System.Windows.Media.Brushes.Gray,
                            StrokeThickness = 2
                        };
                        DrawingCanvas.Children.Add(line);
                    }
                }
            }

            if (CurrentMap.Nodes != null)
            {
                foreach (var node in CurrentMap.Nodes)
                {
                    var ellipse = new System.Windows.Shapes.Ellipse
                    {
                        Width = 80,
                        Height = 40,
                        Fill = (System.Windows.Media.SolidColorBrush)new System.Windows.Media.BrushConverter().ConvertFrom(node.Color),
                        Stroke = System.Windows.Media.Brushes.Black,
                        StrokeThickness = 1
                    };

                    // --- ЛОГІКА ІНДИКАТОРА ---
                    string displayText = node.Text;

                    // Перевіряємо, чи є вкладення (і чи список не null)
                    if (node.Attachments != null && node.Attachments.Count > 0)
                    {
                        displayText = "📎 " + displayText;
                    }
                    // -------------------------

                    var textBlock = new TextBlock
                    {
                        Text = displayText, // Використовуємо змінну з іконкою
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextTrimming = TextTrimming.CharacterEllipsis,
                        MaxWidth = 70
                    };

                    var grid = new Grid { Width = 80, Height = 40, Cursor = Cursors.Hand };
                    grid.Children.Add(ellipse);
                    grid.Children.Add(textBlock);

                    Canvas.SetLeft(grid, node.PosX);
                    Canvas.SetTop(grid, node.PosY);
                    // контекстне меню
                    var contextMenu = new ContextMenu();
                    var cloneItem = new MenuItem { Header = "Дублювати" };
                    cloneItem.Click += (s, e) =>
                    {
                        var clonedNode = node.Clone();
                        CurrentMap.Nodes.Add(clonedNode);
                        Repository.Update(CurrentMap);
                        DrawCurrentMap();
                    };

                    contextMenu.Items.Add(cloneItem);
                    grid.ContextMenu = contextMenu;
                    grid.MouseLeftButtonDown += (sender, e) =>
                    {
                        e.Handled = true;
                        HandleGlobalClick(node); 
                    };

                    DrawingCanvas.Children.Add(grid);
                }
            }

        }
        private void Node_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var grid = sender as Grid;
                var node = grid.Tag as Node;

                if (node != null)
                {
                    var editWindow = new EditNodeWindow(node);
                    editWindow.Owner = this;

                    if (editWindow.ShowDialog() == true)
                    {
                        Repository.Update(CurrentMap);
                        DrawCurrentMap();
                    }
                }
            }
        }
        // методи для патерну Стратегія
        private void SetTool(IMapTool tool, Button activeButton)
        {
            _currentTool?.Cancel();
            _currentTool = tool;
            BtnSelect.Background = System.Windows.Media.Brushes.LightGray;
            BtnConnect.Background = System.Windows.Media.Brushes.LightGray;
            BtnDelete.Background = System.Windows.Media.Brushes.LightGray;
            activeButton.Background = System.Windows.Media.Brushes.LightGreen;
        }
        private void SelectTool_Click(object sender, RoutedEventArgs e)
        {
            SetTool(new SelectionTool(), BtnSelect);
        }

        private void ConnectTool_Click(object sender, RoutedEventArgs e)
        {
            SetTool(new ConnectionTool(), BtnConnect);
        }

        private void DeleteTool_Click(object sender, RoutedEventArgs e)
        {
            SetTool(new DeletionTool(), BtnDelete);
        }
        public void HandleGlobalClick(object clickedItem)
        {
            _currentTool.HandleClick(this, clickedItem);
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _currentTool.HandleClick(this, null);
        }
    }
}