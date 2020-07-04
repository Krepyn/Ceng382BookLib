using System;
using System.Collections.Generic;

namespace Ceng382_Project.Models.DB
{
    public partial class Connection
    {
        public int Pkey { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public int? Rating { get; set; }

        public virtual Books Book { get; set; }
        public virtual Users User { get; set; }
    }
}
