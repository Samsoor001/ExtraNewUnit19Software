using Fantasy_Encylopedia_App.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fantasy_Encylopedia_App.Controllers
{
    [Authorize(Roles = "FBZStaff")]
    [Route("[controller]")]
    public class AnalyticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var topQueries = await _context.SearchLogs
                .GroupBy(x => x.QueryText)
                .Select(g => new AnalyticsItemViewModel
                {
                    Name = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToListAsync();

            var topReturnedComics = await _context.SearchResultLogs
                .GroupBy(x => x.ComicTitle)
                .Select(g => new AnalyticsItemViewModel
                {
                    Name = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToListAsync();

            var comicsOver100 = await _context.SearchResultLogs
                .GroupBy(x => x.ComicTitle)
                .Select(g => new AnalyticsItemViewModel
                {
                    Name = g.Key,
                    Count = g.Count()
                })
                .Where(x => x.Count > 100)
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            var vm = new AnalyticsDashboardViewModel
            {
                TopQueries = topQueries,
                TopReturnedComics = topReturnedComics,
                ComicsOver100 = comicsOver100
            };

            return View(vm);
        }
    }

    public class AnalyticsItemViewModel
    {
        public string Name { get; set; } = "";
        public int Count { get; set; }
    }

    public class AnalyticsDashboardViewModel
    {
        public List<AnalyticsItemViewModel> TopQueries { get; set; } = new();
        public List<AnalyticsItemViewModel> TopReturnedComics { get; set; } = new();
        public List<AnalyticsItemViewModel> ComicsOver100 { get; set; } = new();
    }
}