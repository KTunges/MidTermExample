using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MidTermProject.Models;

namespace MidTermProject.Controllers
{
    public class BooksController : Controller
    {
        private readonly MidTermDBContext _context;
        private readonly IWebHostEnvironment _env;

        public BooksController(MidTermDBContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(string search)
        {
            var books = _context.Books.Include(b => b.Author);

            if (!string.IsNullOrEmpty(search))
                return View(await books.Where(b => b.Title.Contains(search)).ToListAsync());

            return View(await books.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book, IFormFile CoverImage)
        {
            if (CoverImage != null && CoverImage.Length > 0)
            {
                var ext = Path.GetExtension(CoverImage.FileName);
                var fileName = Guid.NewGuid() + ext;
                var filePath = Path.Combine(_env.WebRootPath, "images/covers", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await CoverImage.CopyToAsync(stream);
                }

                book.CoverImagePath = "/images/covers/" + fileName;
            }

            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "Name", book.AuthorId);
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book book, IFormFile CoverImage)
        {
            if (CoverImage != null && CoverImage.Length > 0)
            {
                var ext = Path.GetExtension(CoverImage.FileName);
                var fileName = Guid.NewGuid() + ext;
                var filePath = Path.Combine(_env.WebRootPath, "images/covers", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await CoverImage.CopyToAsync(stream);
                }

                book.CoverImagePath = "/images/covers/" + fileName;
            }

            _context.Update(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.BookId == id);
            return book == null ? NotFound() : View(book);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.BookId == id);
            return book == null ? NotFound() : View(book);
        }

        public IActionResult Statistics()
        {
            var stats = _context.Authors
                .Select(a => new AuthorStatsViewModel
                {
                    AuthorId = a.AuthorId,
                    Name = a.Name,
                    BookCount = a.Books.Count
                }).ToList();
            return View(stats);
        }
    }

    public class AuthorStatsViewModel
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public int BookCount { get; set; }
    }
}
