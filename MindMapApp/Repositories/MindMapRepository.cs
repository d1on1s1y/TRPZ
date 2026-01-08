using MindMapApp.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MindMapApp.Repositories
{
    public class MindMapRepository : IRepository<MindMap>
    {
        private readonly AppDbContext _context;

        public MindMapRepository()
        {
            _context = new AppDbContext();
        }

        public void Add(MindMap entity)
        {
            _context.MindMaps.Add(entity);
            _context.SaveChanges(); // запит в БД
        }

        public void Delete(int id)
        {
            var item = _context.MindMaps.Find(id);
            if (item != null)
            {
                _context.MindMaps.Remove(item);
                _context.SaveChanges();
            }
        }

        public IEnumerable<MindMap> GetAll()
        {
            return _context.MindMaps.Include(m => m.Nodes).ToList();
        }

        public MindMap GetById(int id)
        {
            return _context.MindMaps
        .Include(m => m.Connections)
        .Include(m => m.Nodes)
            .ThenInclude(n => n.Attachments)
        .Include(m => m.Regions)       
            .ThenInclude(r => r.Nodes) 
        .FirstOrDefault(m => m.Id == id);
        }

        public void Update(MindMap entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}