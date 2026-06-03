using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_WEB.Data;

namespace Projekt_WEB.Controllers
{
    public class DisciplinesController : Controller
    {
        private readonly AppDbContext _context;

        public DisciplinesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var disciplines = _context.Disciplines
                .Include(d => d.Athletes)
                .Include(d => d.CompetitionEvents)
                .OrderBy(d => d.Name)
                .ToList();

            return View(disciplines);
        }
    }
}