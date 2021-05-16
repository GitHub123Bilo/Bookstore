using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bookstore.Web;
using Bookstore.Web.Models;
using Bookstore.Web.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Bookstore.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookstoreDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BooksController(BookstoreDbContext context , IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var bookstoreDbContext = _context.Books.Include(b => b.Author);
            var books = await bookstoreDbContext.ToListAsync();
            return View(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.BookImage != null && model.BookImage.Length > 0)
                {
                   var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", model.BookImage.FileName);
                    using var stream = new FileStream(fullPath, FileMode.Create);
                    model.BookImage.CopyTo(stream);
                }

                var book = new Book
                {
                    Title = model.Title,
                    Discription = model.Discription,
                    AuthorId = model.AuthorId,
                    ImagePath = model.BookImage.FileName
                };

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Create));
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            
            var model = new BookViewModel
            {
                Id = book.Id,
                AuthorId = book.AuthorId,
                Author = book.Author,
                Title = book.Title,
                Discription = book.Discription,
                ImagePath = book.ImagePath
            };

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
            return View(model);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var imageName = string.Empty;
                    if (model.BookImage != null && model.BookImage.Length > 0)
                    {
                        imageName = model.BookImage.FileName;
                        var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", imageName);
                        using var stream = new FileStream(fullPath, FileMode.Create);
                        model.BookImage.CopyTo(stream);
                    }

                    var book = new Book
                    {
                        Id = model.Id,
                        Title = model.Title,
                        Discription = model.Discription,
                        AuthorId = model.AuthorId,
                        ImagePath = string.IsNullOrWhiteSpace(imageName) ? model.ImagePath : imageName
                    };

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch 
                {
                    return RedirectToAction(nameof(Edit));
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Edit));
            
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
