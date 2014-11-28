using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace idei.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<Record> Records { get; set; }
    }
}