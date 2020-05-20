using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.Models
{
    public class Book
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public double  Price { get; set; }

        public string Year { get; set; }

        public string Binding { get; set; }

        public bool InStock { get; set; }

        public string Description { get; set; }


    }

}
