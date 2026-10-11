using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.Json;
using WpfBookstore.Models;

namespace WpfBookstore.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly BooksContext _db;

        public BookRepository(BooksContext db)
        {
            _db = db;
        }

        public Task<List<Book>> GetAllAsync()
        {
            return _db.Books.ToListAsync();
        }

        public async Task<(List<Book> Books, int PageCount)> GetFilteredPagedAsync(int pageNumber, int pageSize, BooksFilter filter)
        {
            IQueryable<Book> query = _db.Books;

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(book => book.Title == filter.Title);
            }

            if (!string.IsNullOrEmpty(filter.Author))
            {
                query = query.Where(book => book.Author == filter.Author);
            }

            if (filter.Year != null)
            {
                query = query.Where(book => book.Year == filter.Year);
            }

            if (!string.IsNullOrEmpty(filter.Genre))
            {
                query = query.Where(book => book.Genre == filter.Genre);
            }

            if (!string.IsNullOrEmpty(filter.ISBN))
            {
                query = query.Where(book => book.ISBN == filter.ISBN);
            }

            if (!string.IsNullOrEmpty(filter.Publisher))
            {
                query = query.Where(book => book.Publisher == filter.Publisher);
            }

            if (filter.Pages != null)
            {
                query = query.Where(book => book.Pages == filter.Pages);
            }

            if (filter.Price != null)
            {
                query = query.Where(book => book.Price == filter.Price);
            }

            if (filter.QuantityInStock != null)
            {
                query = query.Where(book => book.QuantityInStock == filter.QuantityInStock);
            }

            // “group-by-constant” trick to add a COUNT to an existing query - See https://www.reddit.com/r/dotnet/comments/1c8pgtb/in_ef_core_is_it_possible_to_retrieve_a_paginated/
            var pagedBooks = await query
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    Items = g.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
                    Total = g.Count()
                }).FirstOrDefaultAsync();

            int pageCount = pagedBooks != null ? (int)Math.Ceiling((double)pagedBooks.Total / pageSize) : 0;

            return (pagedBooks?.Items ?? new List<Book>(), pageCount);
        }

        public async Task<(List<Book> Books, int PageCount)> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            int count = await _db.Books.CountAsync();
            int pageCount = (int)Math.Ceiling((double)count / pageSize);
            var pagedBooks = await _db.Books.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return (pagedBooks, pageCount);
        }

        public Task<int> GetCountAsync()
        {
            return _db.Books.CountAsync();
        }

        public Task AddAsync(Book book)
        {
            _db.Books.Add(book);
            return _db.SaveChangesAsync();
        }

        public Task UpdateAsync(Book book)
        {
            _db.Books.Update(book);
            return _db.SaveChangesAsync();
        }

        public Task RemoveAsync(Book book)
        {
            _db.Books.Remove(book);
            return _db.SaveChangesAsync();
        }

        public Task AddSampleDataAsync(string jsonFileName)
        {
            if (File.Exists(jsonFileName))
            {
                string json = File.ReadAllText(jsonFileName);
                var books = JsonSerializer.Deserialize<Book[]>(json);
                if (books?.Length > 0)
                {
                    _db.Books.AddRange(books);
                    return _db.SaveChangesAsync();
                }
            }

            return Task.CompletedTask;
        }
    }
}
