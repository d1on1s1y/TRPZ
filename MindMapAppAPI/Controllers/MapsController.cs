using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MindMapApp.Entities;
using MindMapAppAPI.Data;

namespace MindMapApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MapsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MapsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetMaps()
        {
            var maps = await _context.MindMaps
                .Select(m => new { m.Id, m.Title })
                .ToListAsync();

            return Ok(maps);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMapDetails(int id)
        {
            var map = await _context.MindMaps
                .Include(m => m.Nodes)
                .Include(m => m.Regions).ThenInclude(r => r.Nodes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (map == null) return NotFound();
            return Ok(map);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMap([FromBody] MindMap map)
        {
            _context.MindMaps.Add(map);
            await _context.SaveChangesAsync();
            return Ok(map);
        }
    }
}