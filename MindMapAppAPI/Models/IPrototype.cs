using MindMapApp.Entities;
namespace MindMapApp.Entities
{
    public interface IPrototype
    {
        Node Clone();
    }
}