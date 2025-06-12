using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.API.Controllers
{
    [ApiController]
    [Route("api/library")]
    public class LibraryController : ControllerBase
    {
        private static Dictionary<int, Book> library = new Dictionary<int, Book>();
        private static int nextId = 1;

        [HttpPost("addbook")]
        public IActionResult AddBook(string name, string author, int? pages = null)
        {
            Book book = new Book();
            book.BookName = name;
            book.AuthorName = author;
            book.PageCount = pages ?? 0; 

            library.Add(nextId, book);
            nextId++;

            return Ok("Added.");
        }

        [HttpGet("allbooks")]
        public List<Book> GetAllBooks()
        {
            return new List<Book>(library.Values);
        }

        [HttpGet("book/{id}")]
        public Book GetBook(int id)
        {
            return library[id]; 
        }

        [HttpPut("changeauthor")]
        public void ChangeAuthor(int id, string newAuthor)
        {
            library[id].AuthorName = newAuthor;
        }

        [HttpDelete("burn")]
        public IActionResult BurnAll()
        {
            library.Clear();
            return Ok("Everything gone."); 
        }

        [HttpGet("search")]
        public List<Book> SearchByName(string keyword)
        {
            List<Book> result = new List<Book>();
            foreach (var item in library)
            {
                if (item.Value.BookName.Contains(keyword)) 
                    result.Add(item.Value);
            }
            return result;
        }
    }

    public class Book
    {
        public string BookName;
        public string AuthorName;
        public int PageCount;
    }
}