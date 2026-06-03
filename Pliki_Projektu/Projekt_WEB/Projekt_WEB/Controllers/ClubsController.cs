using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_WEB.Data;

namespace Projekt_WEB.Controllers
{
    public class ClubsController : Controller
    {
        private readonly AppDbContext _context;

        public ClubsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var clubs = _context.Clubs
                .Include(c => c.Athletes)
                    .ThenInclude(a => a.Discipline)
                .OrderBy(c => c.Name)
                .ToList();

            return View(clubs);
        }

        public IActionResult Details(int id)
        {
            var club = _context.Clubs
                .Include(c => c.Athletes)
                    .ThenInclude(a => a.Discipline)
                .Include(c => c.Athletes)
                    .ThenInclude(a => a.Results)
                .FirstOrDefault(c => c.ClubId == id);

            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }
    }
}