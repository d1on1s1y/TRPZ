using MindMapApp.Entities;
using MindMapApp.Renderers;
using MindMapApp.Repositories;
using MindMapApp.Tools;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MindMapApp
{
    public partial class MainWindow : Window
    {
        public MindMapRepository Repository { get; private set; }
        public MindMap CurrentMap { get; private set; }
        private IMapTool _currentTool;
        private ILineRenderer _currentRenderer = new StraightLineRenderer();

        private bool _isDragging = false;        // Чи тягнемо ми зараз щось?
        private Point _lastMousePosition;        // Де була мишка в минулому кадрі?
        private Node _selectedNode = null;       // Який вузол ми тягнемо?
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
            if (CurrentMap == null || DrawingCanvas == null) return;
            DrawingCanvas.Children.Clear();

            if (CurrentMap.Regions != null)
            {
                foreach (var region in CurrentMap.Regions)
                {
                    // Використовуємо наш метод Composite, щоб дізнатися розмір групи
                    var bounds = region.GetBounds();

                    if (bounds.IsEmpty) continue;

                    var rect = new System.Windows.Shapes.Rectangle
                    {
                        Width = bounds.Width,
                        Height = bounds.Height,
                        Stroke = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom(region.BorderColor),
                        StrokeThickness = 2,
                        StrokeDashArray = new DoubleCollection { 4, 2 }, // Пунктир
                        Fill = System.Windows.Media.Brushes.Transparent,
                        IsHitTestVisible = false // Щоб крізь рамку можна було клікати по канвасу
                    };

                    Canvas.SetLeft(rect, bounds.X);
                    Canvas.SetTop(rect, bounds.Y);

                    // Додаємо підпис регіону (опціонально)
                    var label = new TextBlock
                    {
                        Text = region.Title,
                        Foreground = System.Windows.Media.Brushes.Gray,
                        FontSize = 10,
                        FontWeight = FontWeights.Bold
                    };
                    Canvas.SetLeft(label, bounds.X);
                    Canvas.SetTop(label, bounds.Y - 15); // Трохи вище рамки

                    DrawingCanvas.Children.Add(rect);
                    DrawingCanvas.Children.Add(label);
                }
            }
            if (CurrentMap.Connections != null)
            {
                foreach (var conn in CurrentMap.Connections)
                {
                    var fromNode = CurrentMap.Nodes.FirstOrDefault(n => n.Id == conn.FromNodeId);
                    var toNode = CurrentMap.Nodes.FirstOrDefault(n => n.Id == conn.ToNodeId);

                    if (fromNode != null && toNode != null)
                    {
                        Point startPoint = new Point(fromNode.PosX + 40, fromNode.PosY + 20);
                        Point endPoint = new Point(toNode.PosX + 40, toNode.PosY + 20);
                        _currentRenderer.Draw(DrawingCanvas, startPoint, endPoint, Brushes.Gray);
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

                    
                    string displayText = node.Text;
                    if (node.Attachments != null && node.Attachments.Count > 0)
                    {
                        displayText = "📎 " + displayText;
                    }
                    var textBlock = new TextBlock
                    {
                        Text = displayText, 
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextTrimming = TextTrimming.CharacterEllipsis,
                        MaxWidth = 70
                    };

                    var grid = new Grid { Width = 80, Height = 40, Cursor = Cursors.Hand };
                    grid.DataContext = node;
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
        private void LineStyleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LineStyleComboBox == null) return;

            switch (LineStyleComboBox.SelectedIndex)
            {
                case 0:
                    _currentRenderer = new StraightLineRenderer();
                    break;
                case 1:
                    _currentRenderer = new BezierLineRenderer();
                    break;
                case 2:
                    _currentRenderer = new OrthogonalLineRenderer();
                    break;
            }

            DrawCurrentMap();
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

        private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Перевіряємо, що це саме СЕРЕДНЯ кнопка (коліщатко)
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
            {
                if (e.OriginalSource is FrameworkElement element && element.DataContext is Node node)
                {
                    _isDragging = true;
                    _selectedNode = node;
                    _lastMousePosition = e.GetPosition(DrawingCanvas);

                    DrawingCanvas.CaptureMouse();
                    e.Handled = true;
                }
            }
        }
        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && _selectedNode != null)
            {
                // Перевіряємо, чи затиснуте коліщатко
                if (e.MiddleButton == MouseButtonState.Pressed)
                {
                    Point currentPoint = e.GetPosition(DrawingCanvas);
                    double dx = currentPoint.X - _lastMousePosition.X;
                    double dy = currentPoint.Y - _lastMousePosition.Y;

                    // === COMPOSITE LOGIC ===
                    if (_selectedNode.Region != null)
                    {
                        _selectedNode.Region.Move(dx, dy); // Рухаємо групу
                    }
                    else
                    {
                        _selectedNode.Move(dx, dy); // Рухаємо вузол
                    }
                    // =======================

                    _lastMousePosition = currentPoint;
                    DrawCurrentMap();
                }
            }
        }
        private void DrawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Реагуємо тільки на відпускання середньої кнопки
            if (e.ChangedButton == MouseButton.Middle)
            {
                if (_isDragging)
                {
                    _isDragging = false;
                    _selectedNode = null;
                    DrawingCanvas.ReleaseMouseCapture();
                    Repository.Update(CurrentMap);
                }
            }
        }
    }
}