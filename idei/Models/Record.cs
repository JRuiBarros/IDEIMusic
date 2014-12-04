using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace idei.Models
{

    public class Record
    {
        public int RecordId { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }
        public int FormatId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int ShopSales { get; set; }
        public string AlbumArtUrl { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual Format Format { get; set; }
    }
}