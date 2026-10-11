using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfBookstore.Data
{
    public partial class BooksFilter : ObservableObject
    {
        [ObservableProperty]
        private string? _title = null;

        [ObservableProperty]
        private string? _author = null;

        [ObservableProperty]
        private short? _year = null;

        [ObservableProperty]
        private string? _genre = null;

        [ObservableProperty]
        private string? _iSBN = null;

        [ObservableProperty]
        private string? _publisher = null;

        [ObservableProperty]
        private int? _pages = null;

        [ObservableProperty]
        private decimal? _price = null;

        [ObservableProperty]
        private int? _quantityInStock = null;

        public BooksFilter()
        {
        }

        public void Reset()
        {
            Title = null;
            Author = null;
            Year = null;
            Genre = null;
            ISBN = null;
            Publisher = null;
            Pages = null;
            Price = null;
            QuantityInStock = null;
        }
    }
}
