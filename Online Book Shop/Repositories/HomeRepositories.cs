using Microsoft.EntityFrameworkCore;

namespace Online_Book_Shop.Repositories
{
    public class HomeRepositories: IHomeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        //ctor
        public HomeRepositories(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _dbContext.Genres.ToListAsync();
        }
        public async Task<IEnumerable<Book>> DisplayBooks(string sTerm="",int genreId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Book> books =await (from book in _dbContext.Books
                         join genre in _dbContext.Genres
                         on book.GenreId equals genre.Id
                         where string.IsNullOrWhiteSpace(sTerm) ||(book!=null && book.BookName.ToLower().StartsWith(sTerm))
                         select new Book
                         {
                             Id = book.Id,
                             BookImage = book.BookImage,
                             BookName = book.BookName,
                             Price = book.Price,
                             AuthorName = book.AuthorName,
                             GenreId = book.GenreId,
                             GenreName = genre.GenreName
                         }
                       ).ToListAsync();
            if (genreId > 0)
            {
                books = books.Where(a=>a.GenreId==genreId).ToList();
            }
            return books;
        }
    }
}
