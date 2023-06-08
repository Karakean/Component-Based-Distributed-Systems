using System;

namespace Library.WebApi.Models
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}