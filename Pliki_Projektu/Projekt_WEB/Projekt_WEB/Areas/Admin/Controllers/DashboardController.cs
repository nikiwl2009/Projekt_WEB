using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projekt_WEB.Data;
using Projekt_WEB.ViewModels;

namespace Projekt_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                AthletesCount = _context.Athletes.Count(),
                ClubsCount = _context.Clubs.Count(),
                DisciplinesCount = _context.Disciplines.Count(),
                ResultsCount = _context.Results.Count(),
                CompetitionEventsCount = _context.CompetitionEvents.Count()
            };
            return View(model);
        }
    }
}