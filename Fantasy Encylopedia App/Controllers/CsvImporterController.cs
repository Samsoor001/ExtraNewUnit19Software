using Fantasy_Encylopedia_App.Models;
using Fantasy_Encyclopedia.Core.Models;
using Fantasy_Encyclopedia.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Fantasy_Encylopedia_App.Services;
using Microsoft.AspNetCore.Identity;

namespace Fantasy_Encylopedia_App.Controllers
{
    [Route("[controller]")]
    public class CsvImporterController : Controller
    {
        private readonly CsvService _csvService;
        private readonly IMemoryCache _cache;
        private readonly ComicQueryService _queryService;
        private readonly AnalyticsService _analyticsService;
        private readonly UserManager<IdentityUser> _userManager;

        private const string CacheKey = "CurrentCsvRecords";
        private const int MaxRowsToDisplay = 500;

        public CsvImporterController(
            CsvService csvService,
            IMemoryCache cache,
            ComicQueryService queryService,
            AnalyticsService analyticsService,
            UserManager<IdentityUser> userManager)
        {
            _csvService = csvService;
            _cache = cache;
            _queryService = queryService;
            _analyticsService = analyticsService;
            _userManager = userManager;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index([FromQuery] ComicSearchViewModel vm)
        {
            if (!_cache.TryGetValue(CacheKey, out List<Books>? all) || all == null)
            {
                return View(new ComicSearchViewModel());
            }

            var options = _queryService.BuildOptions(all);
            vm.Genres = options.Genres;
            vm.Years = options.Years;
            vm.Languages = options.Languages;
            vm.NameTypes = options.NameTypes;

            var input = new ComicQueryInput
            {
                TitleQuery = vm.TitleQuery,
                AuthorQuery = vm.AuthorQuery,
                SelectedGenre = vm.SelectedGenre,
                SelectedYear = vm.SelectedYear,
                SelectedLanguage = vm.SelectedLanguage,
                SelectedNameType = vm.SelectedNameType,
                SortTitle = vm.SortTitle,
                GroupBy = vm.GroupBy
            };

            var result = _queryService.Query(all, input, skip: 0, take: MaxRowsToDisplay);

            ViewBag.TotalMatches = result.TotalMatches;
            ViewBag.DisplayCap = MaxRowsToDisplay;

            if (vm.GroupBy == "year")
            {
                vm.Grouped = result.GroupedByKey;
                vm.Results = new List<Books>();
            }
            else
            {
                vm.Results = result.Page;
                vm.Grouped = new Dictionary<string, List<Books>>();
            }

            bool hasSearchCriteria =
                !string.IsNullOrWhiteSpace(vm.TitleQuery) ||
                !string.IsNullOrWhiteSpace(vm.AuthorQuery) ||
                !string.IsNullOrWhiteSpace(vm.SelectedGenre) ||
                vm.SelectedYear.HasValue ||
                !string.IsNullOrWhiteSpace(vm.SelectedLanguage) ||
                !string.IsNullOrWhiteSpace(vm.SelectedNameType);

            if (hasSearchCriteria)
            {
                var queryParts = new List<string>();

                if (!string.IsNullOrWhiteSpace(vm.TitleQuery))
                    queryParts.Add($"Title:{vm.TitleQuery}");

                if (!string.IsNullOrWhiteSpace(vm.AuthorQuery))
                    queryParts.Add($"Author:{vm.AuthorQuery}");

                if (!string.IsNullOrWhiteSpace(vm.SelectedGenre))
                    queryParts.Add($"Genre:{vm.SelectedGenre}");

                if (vm.SelectedYear.HasValue)
                    queryParts.Add($"Year:{vm.SelectedYear.Value}");

                if (!string.IsNullOrWhiteSpace(vm.SelectedLanguage))
                    queryParts.Add($"Language:{vm.SelectedLanguage}");

                if (!string.IsNullOrWhiteSpace(vm.SelectedNameType))
                    queryParts.Add($"Type:{vm.SelectedNameType}");

                var queryText = string.Join(", ", queryParts);

                await _analyticsService.LogSearchAsync(
                    queryText: queryText,
                    returnedTitles: vm.GroupBy == "year"
                        ? vm.Grouped.SelectMany(g => g.Value).Select(b => b.Title)
                        : vm.Results.Select(r => r.Title),
                    userId: _userManager.GetUserId(User));
            }

            return View(vm);
        }

        [HttpPost("")]
        [HttpPost("Index")]
        public IActionResult Index(IFormFile csvFile)
        {
            var vm = new ComicSearchViewModel();

            if (csvFile == null || csvFile.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a valid CSV file.");
                return View(vm);
            }

            try
            {
                using var stream = csvFile.OpenReadStream();
                var records = _csvService.ReadCsvFile(stream).ToList();

                _cache.Set(CacheKey, records, TimeSpan.FromHours(2));

                return RedirectToAction("Index");
            }
            catch (ApplicationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
                return View(vm);
            }
        }

        [HttpGet("More")]
        public IActionResult More(
            string? titleQuery,
            string? authorQuery,
            string? selectedGenre,
            int? selectedYear,
            string? selectedLanguage,
            string? selectedNameType,
            string sortTitle = "az",
            int skip = 0,
            int take = 200)
        {
            if (!_cache.TryGetValue(CacheKey, out List<Books>? all) || all == null)
                return BadRequest("No CSV cached. Please upload again.");

            var input = new ComicQueryInput
            {
                TitleQuery = titleQuery,
                AuthorQuery = authorQuery,
                SelectedGenre = selectedGenre,
                SelectedYear = selectedYear,
                SelectedLanguage = selectedLanguage,
                SelectedNameType = selectedNameType,
                SortTitle = sortTitle,
                GroupBy = "none"
            };

            var result = _queryService.Query(all, input, skip, take);
            return Json(result.Page);
        }
    }
}