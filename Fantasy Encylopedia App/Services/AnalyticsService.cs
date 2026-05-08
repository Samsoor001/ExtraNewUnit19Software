using Fantasy_Encylopedia_App.Data;
using Fantasy_Encylopedia_App.Models;
namespace Fantasy_Encylopedia_App.Services
{
    public class AnalyticsService
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogSearchAsync(
            string queryText,
            IEnumerable<string> returnedTitles,
            string? userId = null)
        {
            if (!string.IsNullOrWhiteSpace(queryText))
            {
                _context.SearchLogs.Add(new SearchLog
                {
                    QueryText = queryText,
                    UserId = userId,
                    SearchedAt = DateTime.UtcNow
                });
            }

            foreach (var title in returnedTitles.Where(t => !string.IsNullOrWhiteSpace(t)).Distinct())
            {
                _context.SearchResultLogs.Add(new SearchLogResult
                {
                    ComicTitle = title,
                    UserId = userId,
                    ReturnedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
