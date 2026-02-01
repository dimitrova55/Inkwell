using Inkwell.Models;
using Inkwell.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace Inkwell.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("book/")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly BookService bookService;

        public BookController(BookService bookService)
        {
            this.bookService = bookService;
        }


        [Authorize]
        [HttpPost("save")]
        public async Task<IActionResult> SaveBook([FromBody] BookDto bookDto)
        {
            try
            {
                await bookService.SaveBook(bookDto);

                return Ok(bookDto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
