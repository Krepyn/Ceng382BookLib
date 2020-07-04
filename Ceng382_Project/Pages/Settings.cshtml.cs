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
    public class SettingsModel : PageModel
    {
        private readonly Ceng382_Project.Models.DB.BookLibContext _context;

        public SettingsModel(Ceng382_Project.Models.DB.BookLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Users Users { get; set; }
        public string Msg { get; set; }


        public IActionResult OnGet()
        {
            string email = HttpContext.Session.GetString("email");

            if (!SessionExists(email))
                return RedirectToPage("Index");

            Users = _context.Users.Single(a => a.Email == email);

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            string email = HttpContext.Session.GetString("email");

            var usr = _context.Users.Single(a => a.Email == email);

            usr.Fname = Users.Fname;
            usr.Lname = Users.Lname;
            usr.Bday = Users.Bday;
            usr.Email = Users.Email;
            usr.Password = Users.Password;
            usr.Photo = Users.Photo;

            await _context.SaveChangesAsync();

            return RedirectToPage("MainPage");
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