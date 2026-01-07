using MindMapApp.Entities;
using System.Linq;
using System.Windows;

namespace MindMapApp.Tools
{
    public class DeletionTool : IMapTool
    {
        public void HandleClick(MainWindow context, object item)
        {
            if (item is Node node)
            {
                var result = MessageBox.Show($"Видалити вузол '{node.Text}'?", "Видалення", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    if (context.CurrentMap.Connections != null)
                    {
                        var linksToRemove = context.CurrentMap.Connections
                            .Where(c => c.FromNodeId == node.Id || c.ToNodeId == node.Id)
                            .ToList();
                        foreach (var link in linksToRemove)
                        {
                            context.CurrentMap.Connections.Remove(link);
                        }
                    }
                    context.CurrentMap.Nodes.Remove(node);
                    context.Repository.Update(context.CurrentMap);
                    context.DrawCurrentMap();
                }
            }
        }

        public void Cancel() { }
    }
}