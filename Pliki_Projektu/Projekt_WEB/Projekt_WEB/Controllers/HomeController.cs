using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt_WEB.Data;
using Projekt_WEB.Models;
using System.Diagnostics;

namespace Projekt_WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CompetitionMapData()
        {
            var eventsData = _context.CompetitionEvents
                .Include(e => e.Discipline)
                .Include(e => e.Results)
                    .ThenInclude(r => r.Athlete)
                .Where(e => e.Latitude.HasValue && e.Longitude.HasValue)
                .OrderBy(e => e.EventDate)
                .ToList()
                .Select(e => new
                {
                    id = e.CompetitionEventId,
                    name = e.Name,
                    date = e.EventDate.ToString("dd.MM.yyyy"),
                    venueName = e.VenueName,
                    city = e.City,
                    country = e.Country,
                    latitude = e.Latitude,
                    longitude = e.Longitude,
                    discipline = e.Discipline != null ? e.Discipline.Name : "Brak dyscypliny",
                    topResults = e.Results
                        .OrderBy(r => r.Place)
                        .ThenByDescending(r => r.Points)
                        .Take(3)
                        .Select(r => new
                        {
                            place = r.Place,
                            points = r.Points,
                            score = r.ScoreText,
                            athlete = r.Athlete != null
                                ? r.Athlete.FirstName + " " + r.Athlete.LastName
                                : "Brak danych"
                        })
                        .ToList()
                })
                .ToList();

            return Json(eventsData);
        }

        [HttpGet]
        public IActionResult Statistics()
        {
            var disciplinesData = _context.Disciplines
                .Include(d => d.Athletes)
                .Select(d => new
                {
                    Name = d.Name,
                    Count = d.Athletes.Count
                })
                .ToList();

            var topAthletesData = _context.Athletes
                .OrderByDescending(a => a.Points)
                .Take(10)
                .Select(a => new
                {
                    Name = a.FirstName + " " + a.LastName,
                    Points = a.Points
                })
                .ToList();

            var chartData = new
            {
                disciplines = new
                {
                    labels = disciplinesData.Select(d => d.Name).ToArray(),
                    values = disciplinesData.Select(d => d.Count).ToArray()
                },
                top = new
                {
                    labels = topAthletesData.Select(a => a.Name).ToArray(),
                    values = topAthletesData.Select(a => a.Points).ToArray()
                }
            };

            return View(chartData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}