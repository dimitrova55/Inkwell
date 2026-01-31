using MongoDB.Driver;
using Inkwell.Models;
using fmi.Services;


namespace Inkwell.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> bookCollection;
        public BookService(MongoDbService mongoDbService) 
        {
            bookCollection = mongoDbService.GetCollection<Book>(Config.Constants.INKWELL_DB, Config.Constants.INKWELL_DB_BOOK_COLLECTION);

        }

        public async Task SaveBook(BookDto bookDto)
        {
            var newBook = new Book();
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
