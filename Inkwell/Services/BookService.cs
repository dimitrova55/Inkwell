using fmi.Services;
using Inkwell.Models;
using MongoDB.Driver;
using System.Security.Claims;


namespace Inkwell.Services
{
    public class BookService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMongoCollection<Book> bookCollection;
        public BookService(MongoDbService mongoDbService, IHttpContextAccessor httpContextAccessor)
        {
            bookCollection = mongoDbService.GetCollection<Book>(Config.Constants.INKWELL_DB, Config.Constants.INKWELL_DB_BOOK_COLLECTION);
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SaveBook(BookDto bookDto)
        {
            var newBook = new Book();

            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            newBook.UserId = userId;
            newBook.Title = bookDto.Title;
            newBook.Author = bookDto.Author;
            newBook.Description = bookDto.Description;
            newBook.IsRead = bookDto.IsRead;
            newBook.IsFavourite = bookDto.IsFavourite;

            if(null != bookDto.Genres)
            {
                newBook.Genres = new List<string>(bookDto.Genres);
            }

            await bookCollection.InsertOneAsync(newBook);
        }
    }
}
