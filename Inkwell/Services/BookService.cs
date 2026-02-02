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

            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var bookList = await bookCollection.Find(u => u.UserId == userId).ToListAsync();

            return bookList;
        }


        public async Task<List<Book>> GetListOfBooksByAuthor(string author)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var bookList = await bookCollection
                .Find(u => u.UserId == userId && u.Author.ToLower() == author.ToLower())
                .ToListAsync();

            return bookList;
        }

        public async Task<bool> UpdateBookInfo(string bookId, BookDto bookDto)
        {
            HashSet<string>? genres = null;

            if (bookDto.Genres != null)
            {
                genres = new HashSet<string>(bookDto.Genres);
            }

            var filter = Builders<Book>.Filter.Eq(b => b.Id, bookId);
            var update = Builders<Book>.Update
                                .Set(b => b.Title, bookDto.Title)
                                .Set(b => b.Author, bookDto.Author)
                                .Set(b => b.Description, bookDto.Description)
                                .Set(b => b.IsRead, bookDto.IsRead)
                                .Set(b => b.IsFavourite, bookDto.IsFavourite)
                                .Set(b => b.Genres, genres);

            var result = await bookCollection.UpdateOneAsync(filter, update);

            // Return true if the book was found and modified
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteBook(string bookId)
        {         
            var result = await bookCollection.DeleteOneAsync(u => u.Id == bookId);

            // Returns true if a book was actually removed
            return result.DeletedCount > 0; 

        }
    }
}
