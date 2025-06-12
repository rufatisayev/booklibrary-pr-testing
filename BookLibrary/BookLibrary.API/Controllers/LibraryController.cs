using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.API.Controllers
{
    [ApiController]
    [Route("api/library")]
    public class LibraryController : ControllerBase
    {
        private static Dictionary<int, Book> library = new Dictionary<int, Book>(); // ⚠️ Using int as key, manually managed
        private static int nextId = 1;

        [HttpPost("addbook")]
        public IActionResult AddBook(string name, string author, int? pages = null)
        {
            Book book = new Book();
            book.BookName = name;
            book.AuthorName = author;
            book.PageCount = pages ?? 0; // ⚠️ Allows 0 pages

            library.Add(nextId, book); // ⚠️ No checks for duplicates
            nextId++;

            return Ok("Added."); // ⚠️ Should return created resource or ID
        }

        [HttpGet("allbooks")]
        public List<Book> GetAllBooks()
        {
            return new List<Book>(library.Values);
        }

        [HttpGet("book/{id}")]
        public Book GetBook(int id)
        {
            return library[id]; // ⚠️ Throws if ID doesn't exist
        }

        [HttpPut("changeauthor")]
        public void ChangeAuthor(int id, string newAuthor)
        {
            library[id].AuthorName = newAuthor; // ⚠️ Unsafe: may throw, no validation, no confirmation
        }

        [HttpDelete("burn")]
        public IActionResult BurnAll()
        {
            library.Clear(); // ⚠️ Irreversible operation with zero safety
            return Ok("Everything gone."); // ⚠️ No warning, no checks
        }

        [HttpGet("search")]
        public List<Book> SearchByName(string keyword)
        {
            List<Book> result = new List<Book>();
            foreach (var item in library)
            {
                if (item.Value.BookName.Contains(keyword)) // ⚠️ Case-sensitive, no null check
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