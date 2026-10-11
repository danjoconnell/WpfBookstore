using WpfBookstore.Models;

namespace WpfBookstore.Data
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);
        Task RemoveAsync(Book book);
        Task<List<Book>> GetAllAsync();
        Task<(List<Book> Books, int PageCount)> GetAllPagedAsync(int pageNumber, int pageSize);
        Task<(List<Book> Books, int PageCount)> GetFilteredPagedAsync(int pageNumber, int pageSize, BooksFilter filter);
        Task<int> GetCountAsync();
        Task UpdateAsync(Book book);
        Task AddSampleDataAsync(string jsonFileName);
    }
}