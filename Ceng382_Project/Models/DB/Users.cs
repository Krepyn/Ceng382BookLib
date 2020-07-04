using System;
using System.Collections.Generic;

namespace Ceng382_Project.Models.DB
{
    public partial class Users
    {
        public Users()
        {
            Connection = new HashSet<Connection>();
        }

        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Bday { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }

        public virtual ICollection<Connection> Connection { get; set; }
    }
}
