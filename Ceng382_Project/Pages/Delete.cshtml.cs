using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ceng382_Project.Models.DB;
using Microsoft.AspNetCore.Http;

namespace Ceng382_Project.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly Ceng382_Project.Models.DB.BookLibContext _context;

        public DeleteModel(Ceng382_Project.Models.DB.BookLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Connection Connection { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string email = HttpContext.Session.GetString("email");

            if (!SessionExists(email))
                return RedirectToPage("Index");

            if (id == null || !BookExists(id))
                return RedirectToPage("MainPage");

            var Book = _context.Books.Single(m => m.Id == id);
            var usr = _context.Users.Single(a => a.Email == email);
            var Connect = _context.Connection.Single(a => (a.UserId == usr.Id) && (a.BookId == Book.Id));

            if(Connect != null)
            {
                _context.Connection.Remove(Connect);
                await _context.SaveChangesAsync();
            }

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
