namespace MindMapApp.Tools
{
    public interface IMapTool
    {
        void HandleClick(MainWindow context, object item);
        void Cancel();
    }
}