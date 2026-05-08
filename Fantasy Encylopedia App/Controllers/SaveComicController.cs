using Fantasy_Encylopedia_App.Data;
using Fantasy_Encylopedia_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fantasy_Encylopedia_App.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class SaveComicController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SaveComicController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("SaveSelected")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSelected(
            string title,
            string author,
            string genre,
            string isbn,
            string publisher,
            string dateOfPublication,
            string language,
            string nameType)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(title))
            {
                return RedirectToAction("Index", "CsvImporter");
            }

            bool alreadySaved = await _context.SaveComic.AnyAsync(x =>
                x.UserId == userId &&
                x.Title == title &&
                x.Author == author);

            if (!alreadySaved)
            {
                var savedComic = new SaveComic
                {
                    UserId = userId,
                    Title = title ?? "",
                    Author = author ?? "",
                    Genre = genre ?? "",
                    ISBN = isbn ?? "",
                    Publisher = publisher ?? "",
                    DateOfPublication = dateOfPublication ?? "",
                    Language = language ?? "",
                    NameType = nameType ?? ""
                };

                _context.SaveComic.Add(savedComic);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "CsvImporter");
        }

        [HttpGet("MySavedComics")]
        public async Task<IActionResult> MySavedComics()
        {
            var userId = _userManager.GetUserId(User);

            var comics = await _context.SaveComic
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Title)
                .ToListAsync();

            return View(comics);
        }
    }
}
