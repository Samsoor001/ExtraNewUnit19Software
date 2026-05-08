using Fantasy_Encylopedia_App.Data;
using Fantasy_Encylopedia_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fantasy_Encylopedia_App.Controllers
{
    [Authorize(Roles = "FBZStaff")]
    [Route("[controller]")]
    public class FlagController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FlagController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("FlagSelected")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FlagSelected(
            string title,
            string author,
            string genre,
            string isbn,
            string publisher,
            string dateOfPublication,
            string language,
            string nameType,
            string flagReason = "Marked for review")
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(title))
            {
                return RedirectToAction("Index", "CsvImporter");
            }

            bool alreadyFlagged = await _context.FlaggedComics.AnyAsync(x =>
                x.Title == title &&
                x.Author == author &&
                x.StaffUserId == userId);

            if (!alreadyFlagged)
            {
                var flaggedComic = new FlaggedComic
                {
                    Title = title ?? "",
                    Author = author ?? "",
                    Genre = genre ?? "",
                    ISBN = isbn ?? "",
                    Publisher = publisher ?? "",
                    DateOfPublication = dateOfPublication ?? "",
                    Language = language ?? "",
                    NameType = nameType ?? "",
                    FlagReason = flagReason ?? "Marked for review",
                    StaffUserId = userId
                };

                _context.FlaggedComics.Add(flaggedComic);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "CsvImporter");
        }

        [HttpGet("ReviewList")]
        public async Task<IActionResult> ReviewList()
        {
            var flagged = await _context.FlaggedComics
                .OrderByDescending(x => x.FlaggedAt)
                .ToListAsync();

            return View(flagged);
        }
    }
}