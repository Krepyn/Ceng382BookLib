using System;
using System.Collections.Generic;

namespace Ceng382_Project.Models.DB
{
    public partial class Books
    {
        public Books()
        {
            Connection = new HashSet<Connection>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Translator { get; set; }
        public string Publisher { get; set; }
        public string Description { get; set; }
        public string Cover { get; set; }
        public string Categories { get; set; }

        public virtual ICollection<Connection> Connection { get; set; }

        public bool ShouldSerializeId()
        {
            return false;
        }

        public bool ShouldSerializeConnection()
        {
            return false;
        }
    }
}
