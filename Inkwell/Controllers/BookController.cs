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
        [HttpPost("save-book-info")]
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


        [Authorize]
        [HttpGet("books-by-user")]
        public async Task<IActionResult> GetListOfBooksByUser()
        {
            try
            {
                var result = await bookService.GetListOfBooksByUser();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("books-by-author/{author}")]
        public async Task<IActionResult> GetListOfBooksByAuthor(String author)
        {
            try
            {
                var result = await bookService.GetListOfBooksByAuthor(author);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize]
        //[HttpPut("update/book-info/{bookId}")]
        //public async Task<IActionResult> UpdateBookInfo(string bookId, [FromBody] BookDto bookDto)
        //{
        //    try
        //    {
        //        var result = await bookService.UpdateBookInfo(bookId, bookDto);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[Authorize]
        //[HttpDelete("delete/{bookId}")]
        //public async Task<IActionResult> DeleteBookInfo(String bookId)
        //{
        //    try
        //    {
        //        // await bookService.SaveBook(bookDto);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
