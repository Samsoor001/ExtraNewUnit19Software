using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fantasy_Encylopedia_App.Models;

namespace Fantasy_Encylopedia_App.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<SaveComic> SaveComic { get; set; }
        public DbSet<SearchLog> SearchLogs { get; set; }
        public DbSet<SearchLogResult> SearchResultLogs { get; set; }
        public DbSet<FlaggedComic> FlaggedComics { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }
    }
}
