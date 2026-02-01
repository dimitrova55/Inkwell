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
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var savedBook = await bookCollection
                .Find(u => u.UserId == userId 
                    && u.Title.ToLower() == bookDto.Title.ToLower()
                    && u.Author.ToLower() == bookDto.Author.ToLower())
                .FirstOrDefaultAsync();  
            
            if(null != savedBook)
            {
                throw new ArgumentException("This book already exists in your library.");
            }

            var newBook = new Book();
            newBook.UserId = userId;
            newBook.Title = bookDto.Title;
            newBook.Author = bookDto.Author;
            newBook.Description = bookDto.Description;
            newBook.IsRead = bookDto.IsRead;
            newBook.IsFavourite = bookDto.IsFavourite;

            if(null != bookDto.Genres)
            {
                newBook.Genres = new HashSet<string>(bookDto.Genres);
            }

            await bookCollection.InsertOneAsync(newBook);
        }

        public async Task<List<Book>> GetListOfBooksByUser()
        {
            // throw new NotImplementedException();

            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var bookList = await bookCollection.Find(u => u.UserId == userId).ToListAsync();

            return bookList;
        }


        public async Task<List<Book>> GetListOfBooksByAuthor(string author)
        {
            // throw new NotImplementedException();

            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var bookList = await bookCollection
                .Find(u => u.UserId == userId && u.Author.ToLower() == author.ToLower())
                .ToListAsync();

            return bookList;
        }

        //public async Task<Book> UpdateBookInfo(string bookId, BookDto bookDto)
        //{
        //    // throw new NotImplementedException();

        //    // var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var savedBook = await bookCollection
        //        .Find(u => u.Id == bookId)
        //        .FirstOrDefaultAsync();

        //    return savedBook;
        //}
    }
}
