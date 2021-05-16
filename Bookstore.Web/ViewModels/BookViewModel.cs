using Bookstore.Web.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Web.ViewModels
{
    public class BookViewModel : Book
    {
        public IFormFile BookImage { get; set; }
    }
}
