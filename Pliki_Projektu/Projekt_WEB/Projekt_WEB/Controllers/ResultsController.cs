using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt_WEB.Data;

namespace Projekt_WEB.Controllers
{
    public class ResultsController : Controller
    {
        private readonly AppDbContext _context;

        public ResultsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? disciplineId, int? eventId, string? sort)
        {
            var resultsQuery = _context.Results
                .Include(r => r.Athlete)
                    .ThenInclude(a => a.Discipline)
                .Include(r => r.CompetitionEvent)
                    .ThenInclude(e => e.Discipline)
                .AsQueryable();

            if (disciplineId.HasValue && disciplineId.Value > 0)
            {
                resultsQuery = resultsQuery.Where(r =>
                    r.CompetitionEvent != null &&
                    r.CompetitionEvent.DisciplineId == disciplineId.Value);
            }

            if (eventId.HasValue && eventId.Value > 0)
            {
                resultsQuery = resultsQuery.Where(r => r.CompetitionEventId == eventId.Value);
            }

            resultsQuery = sort switch
            {
                "place_desc" => resultsQuery.OrderByDescending(r => r.Place),
                "points_desc" => resultsQuery.OrderByDescending(r => r.Points),
                "points_asc" => resultsQuery.OrderBy(r => r.Points),
                "date_desc" => resultsQuery.OrderByDescending(r => r.CompetitionEvent!.EventDate),
                "date_asc" => resultsQuery.OrderBy(r => r.CompetitionEvent!.EventDate),
                _ => resultsQuery
                    .OrderBy(r => r.CompetitionEvent!.EventDate)
                    .ThenBy(r => r.Place)
            };

            ViewBag.DisciplineId = disciplineId;
            ViewBag.EventId = eventId;
            ViewBag.Sort = sort;

            ViewBag.Disciplines = new SelectList(
                _context.Disciplines.OrderBy(d => d.Name).ToList(),
                "DisciplineId",
                "Name",
                disciplineId
            );

            ViewBag.Events = new SelectList(
                _context.CompetitionEvents.OrderBy(e => e.EventDate).ToList(),
                "CompetitionEventId",
                "Name",
                eventId
            );

            var results = resultsQuery.ToList();

            return View(results);
        }
    }
}