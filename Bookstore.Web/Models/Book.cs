using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Web.Models
{
    public class Book : ModelBase
    {
        public string Title { get; set; }
        public string Discription { get; set; }
        public string ImagePath { get; set; }
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

    }
}
