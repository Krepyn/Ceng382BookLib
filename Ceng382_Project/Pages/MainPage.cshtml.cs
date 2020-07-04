using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceng382_Project.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Ceng382_Project.Pages
{
    public class MainPageModel : PageModel
    {

        private readonly Ceng382_Project.Models.DB.BookLibContext _context;

        public MainPageModel(Ceng382_Project.Models.DB.BookLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Users Usr { get; set; }
        public IList<Books> Bookies { get; set; }
        public IList<Models.DB.Connection> Connects { get; set; }
        [BindProperty]
        public string X { get; set; }
        [BindProperty]
        public string SearchVal { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string email = HttpContext.Session.GetString("email");

            if (!SessionExists(email))
                return RedirectToPage("Index");

            Usr = _context.Users.Single(a => a.Email == email);
            Connects = await _context.Connection.ToListAsync();
            Bookies = await _context.Books.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string email = HttpContext.Session.GetString("email");

            Usr = _context.Users.Single(a => a.Email == email);
            Connects = await _context.Connection.ToListAsync();
            Bookies = await _context.Books.ToListAsync();

            if (SearchVal == null)
                X = "";

            return Page();
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