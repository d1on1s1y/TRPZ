using System.Windows;
using MindMapApp.Entities;
using System.Linq;

namespace MindMapApp.Tools
{
    public class ConnectionTool : IMapTool
    {
        private Node _sourceNode;
        public void HandleClick(MainWindow context, object item)
        {
            if (item is Node clickedNode)
            {
                if (_sourceNode == null)
                {
                    _sourceNode = clickedNode;
                    MessageBox.Show($"Початок лінії: {_sourceNode.Text}. Тепер оберіть другий вузол.");
                }
                else
                {
                    if (_sourceNode == clickedNode) return;
                    var newConnection = new Connection
                    {
                        FromNodeId = _sourceNode.Id,
                        ToNodeId = clickedNode.Id,
                        MindMapId = context.CurrentMap.Id
                    };
                    if (context.CurrentMap.Connections == null)
                        context.CurrentMap.Connections = new System.Collections.Generic.List<Connection>();

                    context.CurrentMap.Connections.Add(newConnection);
                    context.Repository.Update(context.CurrentMap);
                    _sourceNode = null;
                    context.DrawCurrentMap();
                }
            }
        }
        public void Cancel()
        {
            _sourceNode = null;
        }
    }
}