using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfBookstore.Models
{
    public class Book : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetWithNotify(ref _id, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetWithNotify(ref _title, value);
        }

        private string _author;
        public string Author
        {
            get => _author;
            set => SetWithNotify(ref _author, value);
        }

        private short _year;
        public short Year
        {
            get => _year;
            set => SetWithNotify(ref _year, value);
        }

        private string _genre;
        public string Genre
        {
            get => _genre;
            set => SetWithNotify(ref _genre, value);
        }

        private string _isbn;
        public string ISBN
        {
            get => _isbn;
            set => SetWithNotify(ref _isbn, value);
        }

        private string _publisher;
        public string Publisher
        {
            get => _publisher;
            set => SetWithNotify(ref _publisher, value);
        }

        private int _pages;
        public int Pages
        {
            get => _pages;
            set => SetWithNotify(ref _pages, value);
        }

        private decimal _price;
        [Precision(10, 2)]
        public decimal Price
        {
            get => _price;
            set => SetWithNotify(ref _price, value);
        }

        private int _quantityInStock;
        public int QuantityInStock
        {
            get => _quantityInStock;
            set => SetWithNotify(ref _quantityInStock, value);
        }

        public Book(int id, string title, string author, short year, string genre, string iSBN, string publisher, int pages, decimal price, int quantityInStock)
        {
            _id = id;
            _title = title;
            _author = author;
            _year = year;
            _genre = genre;
            _isbn = iSBN;
            _publisher = publisher;
            _pages = pages;
            _price = price;
            _quantityInStock = quantityInStock;
        }

        public Book() : this(0, "", "", 0, "", "", "", 0, 0m, 0)
        {
        }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;

        private bool SetWithNotify<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
            field = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return true;
        }
    }
}
