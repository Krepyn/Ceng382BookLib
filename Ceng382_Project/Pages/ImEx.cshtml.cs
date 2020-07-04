using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ceng382_Project.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.SignalR.Protocol;
using Newtonsoft.Json.Linq;

namespace Ceng382_Project.Pages
{
    public class ImExModel : PageModel
    {
        private readonly Ceng382_Project.Models.DB.BookLibContext _context;

        public ImExModel(Ceng382_Project.Models.DB.BookLibContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Msg { get; set; }
        [BindProperty]
        public string Op { get; set; }

        public IActionResult OnGet()
        {
            string email = HttpContext.Session.GetString("email");

            if (!SessionExists(email))
                return RedirectToPage("Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(Op == "1")
            {
                await ExportAsync();
                Msg = "Data exported successfully.";
            } else if (Op == "0")
            {
                await ImportAsync();
                Msg = "Data imported successfully.";
            }
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

        private async Task ExportAsync()
        {
            string email = HttpContext.Session.GetString("email");
            var user = _context.Users.Single(a => a.Email == email);
            var connectList = await _context.Connection.ToListAsync();
            Books newBook;
            JArray ja = new JArray();
            JObject jo;

            string filePath = $"wwwroot/json/{user.Id}_{user.Fname}.json";

            foreach (var item in connectList)
            {
                if (item.UserId == user.Id)
                {
                    newBook = _context.Books.Single(a => a.Id == item.BookId);
                    jo = JObject.FromObject(newBook);
                    jo.Add("Status", item.Status);
                    jo.Add("Rating", item.Rating);
                    ja.Add(jo);
                }
            }

            if (!System.IO.File.Exists(filePath))
            {
                var dump = System.IO.File.Create(filePath);
                dump.Close();
            }

            System.IO.File.WriteAllText(filePath, ja.ToString());
        }

        private async Task ImportAsync()
        {
            string json, email = HttpContext.Session.GetString("email");
            var user = _context.Users.Single(a => a.Email == email);
            var connectList = await _context.Connection.ToListAsync();
            string filePath = $"wwwroot/json/{user.Id}_{user.Fname}.json";
            Connection newConnection = new Connection();
            Books newBook = new Books();

            foreach (var item in connectList)
            {
                if (item.UserId == user.Id)
                    _context.Connection.Remove(item);
            }

            await _context.SaveChangesAsync();

            using (StreamReader r = new StreamReader(filePath))
            {
                json = r.ReadToEnd();

            }
            JArray ja = JArray.Parse(json);

            foreach (var item in ja) {
                if (!_context.Books.Any(a => a.Title == item.Value<string>("Title")))
                {
                    newBook = new Books();
                    newBook.Title = item.Value<string>("Title");
                    newBook.Author = item.Value<string>("Author");
                    newBook.Translator = item.Value<string>("Translator");
                    newBook.Publisher = item.Value<string>("Publisher");
                    newBook.Description = item.Value<string>("Description");
                    newBook.Cover = item.Value<string>("Cover");
                    newBook.Categories = item.Value<string>("Categories");
                    _context.Books.Add(newBook);
                    await _context.SaveChangesAsync();
                }

                newConnection = new Connection();

                var book = _context.Books.Single(x => x.Title == item.Value<string>("Title"));
                newConnection.UserId = user.Id;
                newConnection.BookId = book.Id;
                var status = item.Value<string>("Status");

                if (status == "Completed" || status == "Reading" || status == "Not started")
                {
                    newConnection.Status = status;
                }
                else
                    newConnection.Status = "Not started";

                var rating = item.Value<int>("Rating");

                if(rating <= 10 && rating >= 0)
                    newConnection.Rating = rating;
                else
                    newConnection.Rating = 0;

                _context.Connection.Add(newConnection);

                await _context.SaveChangesAsync();
            }

        }

    }
}