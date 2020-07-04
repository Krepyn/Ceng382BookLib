using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceng382_Project.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Ceng382_Project.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Ceng382_Project.Models.DB.BookLibContext _context;

        public IndexModel(Ceng382_Project.Models.DB.BookLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public string Msg { get; set; }

        public IActionResult OnGet()
        {
            Email = null;
            Email = HttpContext.Session.GetString("email");

            if (SessionExists())
                return RedirectToPage("MainPage");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (UserExists(Email, Password))
            {
                var user = _context.Users.Single(x => x.Email == Email);
                HttpContext.Session.SetString("email", Email);
                return RedirectToPage("MainPage");
            }
            else
            {
                Msg = "Wrong Email or Password.";
                return Page();
            }
        }

        private bool UserExists(string email, string password)
        {
            bool usern = false, pass = false;
            usern = _context.Users.Any(e => e.Email == email);
            if (usern)
            {
                var user = _context.Users.Single(a => a.Email == email);
                if (user.Password == password)
                    pass = true;
            }

            return pass;

        }

        private bool SessionExists()
        {
            bool usern = false;
            if (!String.IsNullOrEmpty(Email) && !Email.Equals(""))
            {
               usern = _context.Users.Any(e => e.Email == Email);
            }
            return usern;
        }
    }
}
