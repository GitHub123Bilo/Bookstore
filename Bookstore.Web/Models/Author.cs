using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Web.Models
{
    public class Author : ModelBase
    {
        public string Name { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
