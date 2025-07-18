using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MidTermProject.Models;

namespace MidTermProject.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly MidTermDBContext _context;

        public AuthorsController(MidTermDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Authors.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            return author == null ? NotFound() : View(author);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Update(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            return author == null ? NotFound() : View(author);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            return author == null ? NotFound() : View(author);
        }
    }
}