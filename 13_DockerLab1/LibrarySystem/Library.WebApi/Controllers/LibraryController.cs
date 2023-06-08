using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.WebApi.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class LibraryController : Controller
    {
        private readonly IPublishEndpoint _publishEndpoint;

        private readonly List<LibraryResource> _libraryBooks = new List<LibraryResource>()
        {
            new LibraryResource()
            {
                Id = 1,
                Book = new Book()
                {
                     Title = "Miecz Przeznaczenia",
                     Author = "Andrzej Sapkowski",
                     ISBN = "9788375780642",
                     ReleaseDate = new DateTime(2014,1,01)
                     
                }
            },
            new LibraryResource()
            {
                Id = 2,
                Book = new Book()
                {
                    Title = "Malowany Czlowiek. Ksiega 1",
                    Author = "Peter V. Brett",
                    ISBN = "9788375781111",
                    ReleaseDate = new DateTime(2008,11,28)
                     
                }
            },
            new LibraryResource()
            {
                Id = 10,
                Book = new Book()
                {
                    Title = "Mikolajek",
                    Author = "Rene Goscinny, Jean-Jacques Sempe",
                    ISBN = "9788375780642",
                    ReleaseDate = new DateTime(2014,09,13)
                     
                }
            },
            new LibraryResource()
            {
                Id = 3,
                Book = new Book()
                {
                    Title = "Droga Cienia",
                    Author = "Brent Weeks",
                    ISBN = "9788375780281",
                    ReleaseDate = new DateTime(2017,5,31)
                     
                }
            },
            new LibraryResource()
            {
                Id = 4,
                Book = new Book()
                {
                    Title = "Praktyczny przewodnik. USA",
                    Author = "Monika Gruszczynska",
                    ISBN = "9788555780621",
                    ReleaseDate = new DateTime(2018,2,28)
                     
                }
            },
            new LibraryResource()
            {
                Id = 5,
                Book = new Book()
                {
                    Title = "Mikolajek",
                    Author = "Rene Goscinny, Jean-Jacques Sempe",
                    ISBN = "9788375780642",
                    ReleaseDate = new DateTime(2014,9,13)
                     
                }
            },
            new LibraryResource()
            {
                Id = 6,
                Book = new Book()
                {
                    Title = "Malowany Czlowiek. Ksiega 2",
                    Author = "Peter V. Brett",
                    ISBN = "9788375781221",
                    ReleaseDate = new DateTime(2009,1,28)
                     
                }
            }
        };

        public LibraryController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        
        [HttpGet("rented")]
        public IEnumerable<LibraryResource> GetRented()
        {
            return _libraryBooks;
        }
        
        [HttpGet("rent/{id}")]
        public async Task<ActionResult> Rent(int id)
        {
            var rentedBook = _libraryBooks.SingleOrDefault(r => r.Id == id);
            if (rentedBook == null)
            {
                return NotFound($"Book with id: {id} does not exist");
            }
            
            await _publishEndpoint.Publish<BookRented>(new {Id = id, Title = rentedBook.Book.Title});
            return Ok();
        }
    }
}