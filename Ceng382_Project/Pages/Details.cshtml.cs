using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceng382_Project.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ceng382_Project.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly Ceng382_Project.Models.DB.BookLibContext _context;

        public DetailsModel(Ceng382_Project.Models.DB.BookLibContext context)
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

            return Page();
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