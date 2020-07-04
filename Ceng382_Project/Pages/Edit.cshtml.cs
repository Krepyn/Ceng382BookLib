using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ceng382_Project.Models.DB;
using Microsoft.AspNetCore.Http;

namespace Ceng382_Project.Pages
{
    public class EditModel : PageModel
    {
        private readonly Ceng382_Project.Models.DB.BookLibContext _context;

        public EditModel(Ceng382_Project.Models.DB.BookLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Books Book { get; set; }
        [BindProperty]
        public Connection Connect { get; set; }

        public IActionResult OnGet(int? id)
        {
            string email = HttpContext.Session.GetString("email");

            if (!SessionExists(email))
                return RedirectToPage("Index");

            if (id == null || !BookExists(id))
                return RedirectToPage("MainPage");

            Book = _context.Books.Single(m => m.Id == id);
            var usr = _context.Users.Single(a => a.Email == email);
            Connect = _context.Connection.Single(a => (a.UserId == usr.Id) && (a.BookId == Book.Id));
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string email = HttpContext.Session.GetString("email");

            var usr = _context.Users.Single(a => a.Email == email);
            var booku = _context.Books.Single(m => m.Id == Book.Id);
            booku.Title = Book.Title;
            booku.Author = Book.Author;
            booku.Categories = Book.Categories;
            booku.Translator = Book.Translator;
            booku.Publisher = Book.Publisher;
            booku.Description = Book.Description;
            booku.Cover = Book.Cover;

            var connectu = _context.Connection.Single(a => (a.UserId == usr.Id) && (a.BookId == Book.Id));
            connectu.Status = Connect.Status;
            connectu.Rating = Connect.Rating;

            await _context.SaveChangesAsync();

            return RedirectToPage("MainPage");
        }

        private bool BookExists(int? id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        private bool SessionExists(string email)
        {
            if (String.IsNullOrEmpty(email) || !(_context.Users.Any(a => a.Email == email)))
            {
                return false;
            }
            return true;
        }
    }
}
