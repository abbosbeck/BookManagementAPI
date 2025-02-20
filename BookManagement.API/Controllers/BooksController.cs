using BookManagement.Core.Services;
using BookManagement.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/books/paginated?page=1&pageSize=10
        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedBooksTitles([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var titles = await _bookService.GetBookTitlesAsync(page, pageSize);
            return Ok(titles);
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookDetails(int id)
        {
            try
            {
                var book = await _bookService.GetBookDetailsAsync(id);
                if (book == null)
                    return NotFound();


                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST: api/books
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookViewModel bookViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newBookId = await _bookService.AddBookAsync(bookViewModel);

                return CreatedAtAction(nameof(GetBookDetails), new { id = newBookId }, new { BookId = newBookId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST: api/books/bulk-add
        [HttpPost("bulk-add")]
        public async Task<IActionResult> AddBooksBulk([FromBody] IEnumerable<BookViewModel> books)
        {
            try
            {
                var result = await _bookService.AddBooksBulkAsync(books);
                return StatusCode(201, new
                {
                    Message = "Books added successfully",
                    AddedCount = result.addedCount,
                    SkippedTitles = result.skippedTitles
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT: api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookViewModel bookViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _bookService.UpdateBookAsync(id, bookViewModel);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteBook(int id)
        {
            try
            {
                await _bookService.SoftDeleteBookAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/books/bulk-delete
        [HttpDelete("bulk-delete")]
        public async Task<IActionResult> SoftDeleteBooksBulk([FromBody] IEnumerable<int> bookIds)
        {
            try
            {
                await _bookService.SoftDeleteBooksBulkAsync(bookIds);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
