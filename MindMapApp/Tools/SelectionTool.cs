using MindMapApp.Entities;
using System.Windows.Controls;

namespace MindMapApp.Tools
{
    public class SelectionTool : IMapTool
    {
        public void HandleClick(MainWindow context, object item)
        {
            if (item is Node node)
            {
                var editWindow = new EditNodeWindow(node);
                editWindow.Owner = context;

                if (editWindow.ShowDialog() == true)
                {
                    context.Repository.Update(context.CurrentMap);
                    context.DrawCurrentMap();
                }
            }
        }
        public void Cancel() { }
    }
}