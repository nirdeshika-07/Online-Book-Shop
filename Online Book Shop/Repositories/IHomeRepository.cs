using Online_Book_Shop.Models;

namespace Online_Book_Shop
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Book>> DisplayBooks(string sTerm = "", int genreId = 0);
        Task<IEnumerable<Genre>> Genres();
    }
}