using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ceng382_Project.Models.DB;
using Microsoft.AspNetCore.Http;

namespace Ceng382_Project.Pages
{
    public class SignUpModel : PageModel
    {
        private readonly Ceng382_Project.Models.DB.BookLibContext _context;

        public SignUpModel(Ceng382_Project.Models.DB.BookLibContext context)
        {
            _context = context;
        }


        public string Email { get; set; }
        public string Msg { get; set; }
        [BindProperty]
        public Users User { get; set; }

        public IActionResult OnGet()
        {
            Email = null;
            Email = HttpContext.Session.GetString("email");

            if (SessionExists())
                return RedirectToPage("MainPage");


            return Page();
        }


        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(!_context.Users.Any(x => x.Email == this.User.Email))
            {
                _context.Users.Add(User);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            } else
            {
                Msg = "This email already exists.";
                return Page();
            }
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
