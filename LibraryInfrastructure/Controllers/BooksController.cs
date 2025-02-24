using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryDomain.Model;
using LibraryInfrastracture;

namespace LibraryInfrastructure.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(int? id, string? name, string? type)
        {
            if(type == "publishers")
            {
                if (id == null) return RedirectToAction("Publishers", "Index");
                ViewBag.Id = id;
                ViewBag.Name = " видавництвом "+name;
                var bookByPublisher = _context.Books.Where(b => b.PublisherId == id).
                    Include(b => b.Publisher).
                    Include(b => b.BookAuthors).
                    ThenInclude(ba => ba.Author).
                    Include(b => b.BookGenres).
                    ThenInclude(g => g.Genre);
                return View(await bookByPublisher.ToListAsync());
            }
            if(type == "authors")
            {
                if (id == null) return RedirectToAction("Authors", "Index");
                ViewBag.Id = id;
                ViewBag.Name = "автором "+name;
                var bookByAuthor = _context.Books.Where(b => b.BookAuthors.Any(ba => ba.AuthorId == id)) 
                    .Include(b => b.Publisher)
                    .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author).
                    Include(b => b.BookGenres).
                    ThenInclude(g => g.Genre);
                return View(await bookByAuthor.ToListAsync());
            }
            var bookBy = _context.Books.
                    Include(b => b.Publisher).
                    Include(b => b.BookAuthors).
                    ThenInclude(ba => ba.Author).
                    Include(b=>b.BookGenres).
                    ThenInclude(g=>g.Genre);
            return View(await bookBy.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Publisher).
                Include(b => b.BookAuthors).
                ThenInclude(ba => ba.Author).
                Include(b => b.BookGenres).
                ThenInclude(g => g.Genre).
                FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "PublisherName");
            ViewBag.Authors = new MultiSelectList(
                _context.Authors.Select(a => new { 
                    a.Id, FullName = a.LastName + " " + a.FirstName 
                }),
                "Id",
                "FullName"
            );
            ViewBag.Genres = new MultiSelectList(_context.Genre.Select(g => new
            {
                g.Id,
                g.GenreName
            }),
            "Id",
            "GenreName");
            return View(new Book());
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Isbn,PublisherId,Id")] Book book, List<long> selectAuthorsforBook, List<long> selectGenresforBook)
        {
            //if (!ModelState.IsValid)
            //{
            //    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            //    {
            //        Console.WriteLine(error.ErrorMessage);
            //    }
            //}
            ModelState.Remove("Publisher");
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                foreach (var authorId in selectAuthorsforBook)
                {
                    _context.BookAuthors.Add(new BookAuthor
                    {
                        BookId = book.Id, 
                        AuthorId = authorId
                    });
                }
                await _context.SaveChangesAsync();
                foreach (var genreId in selectGenresforBook)
                {
                    _context.BookGenres.Add(new BookGenre
                    {
                        bookid = book.Id,
                        genreid = genreId
                    });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "PublisherName", book.PublisherId);
            ViewBag.Authors = new MultiSelectList(
                _context.Authors.Select(a => new {
                    a.Id,
                    FullName = a.LastName + " " + a.FirstName
                }),
                "Id",
                "FullName"
            );
            ViewBag.Genres = new MultiSelectList(_context.Genre.Select(g => new
            {
                g.Id,
                g.GenreName
            }),
            "Id",
            "GenreName");
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(long? id)
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
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "PublisherName", book.PublisherId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Title,Isbn,PublisherId,Id")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "PublisherName", book.PublisherId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Publisher)
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
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(long id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
