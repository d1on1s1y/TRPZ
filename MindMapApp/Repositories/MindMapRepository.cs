using MindMapApp.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MindMapApp.Repositories
{
    public class MindMapRepository : IRepository<MindMap>
    {
        public void Add(MindMap entity)
        {
        }

        public void Delete(int id)
        {}

        public IEnumerable<MindMap> GetAll()
        {
            return new List<MindMap>();
        }

        public MindMap GetById(int id)
        {
            return null;
        }

        public void Update(MindMap entity)
        {
        }
    }
}