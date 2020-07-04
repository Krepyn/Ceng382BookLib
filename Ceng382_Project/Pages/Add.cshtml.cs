using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceng382_Project.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Ceng382_Project.Pages
{
    public class AddModel : PageModel
    {
        private readonly Ceng382_Project.Models.DB.BookLibContext _context;

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public Books NewBook { get; set; }
        [BindProperty]
        public Connection NewConnection { get; set; }

        public string Msg { get; set; }

        public AddModel(Ceng382_Project.Models.DB.BookLibContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            string email = HttpContext.Session.GetString("email");

            if (!SessionExists(email))
                return RedirectToPage("Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {

            if(!(_context.Books.Any(a => a.Title == NewBook.Title))){
                _context.Books.Add(NewBook);
                await _context.SaveChangesAsync();
            }

            string email = HttpContext.Session.GetString("email");
            var usr = _context.Users.Single(a => a.Email == email);
            var booku = _context.Books.Single(a => a.Title == NewBook.Title);

            if(!_context.Connection.Any(a => (a.BookId == booku.Id) && (a.UserId == usr.Id)))
            {

                NewConnection.BookId = booku.Id;
                NewConnection.UserId = usr.Id;

                _context.Connection.Add(NewConnection);

                await _context.SaveChangesAsync();

                return RedirectToPage("Add");
            } else
            {
                Msg = "This book already exists in your library.";

                return Page();
            }


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