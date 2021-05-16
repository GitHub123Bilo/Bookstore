using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Web.Models
{
    public abstract class ModelBase
    {
        public ModelBase()
        {
            AddedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
