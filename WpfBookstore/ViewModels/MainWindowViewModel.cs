using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WpfBookstore.Data;
using WpfBookstore.Models;
using WpfBookstore.Services;

namespace WpfBookstore.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IBookRepository _repository;
        private readonly IDialogService _dialogService;
        private const int _pageSize = 10;

        [ObservableProperty]
        private ObservableCollection<Book> _books = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EditSelectedBookCommand))]
        [NotifyCanExecuteChangedFor(nameof(RemoveSelectedBookCommand))]
        private Book? _selectedBook = null;


        [ObservableProperty]
        private BooksFilter _filter = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FirstPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(LastPageCommand))]
        private int _currentPage = 1;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(LastPageCommand))]
        private int _totalPages = 1;

        public IAsyncRelayCommand LoadAllBooksCommand { get; }

        public IAsyncRelayCommand ApplyFilterCommand { get; }
        public IAsyncRelayCommand ResetFilterCommand { get; }

        public IAsyncRelayCommand AddBookCommand { get; }
        public IAsyncRelayCommand EditSelectedBookCommand { get; }
        public IAsyncRelayCommand RemoveSelectedBookCommand { get; }

        public IAsyncRelayCommand FirstPageCommand { get; }
        public IAsyncRelayCommand PreviousPageCommand { get; }
        public IAsyncRelayCommand NextPageCommand { get; }
        public IAsyncRelayCommand LastPageCommand { get; }

        public MainWindowViewModel(IBookRepository repository, IDialogService dialogService)
        {
            _repository = repository;
            _dialogService = dialogService;

            LoadAllBooksCommand = new AsyncRelayCommand(LoadAllBooksAsync);

            ApplyFilterCommand = new AsyncRelayCommand(ApplyFilterAsync);
            ResetFilterCommand = new AsyncRelayCommand(ResetFilterAsync);

            AddBookCommand = new AsyncRelayCommand(AddBookAsync);
            EditSelectedBookCommand = new AsyncRelayCommand(EditSelectedBookAsync, () => SelectedBook != null);
            RemoveSelectedBookCommand = new AsyncRelayCommand(RemoveSelectedBookAsync, () => SelectedBook != null);

            FirstPageCommand = new AsyncRelayCommand(FirstPageAsync, () => CurrentPage > 1);
            PreviousPageCommand = new AsyncRelayCommand(PreviousPageAsync, () => CurrentPage > 1);
            NextPageCommand = new AsyncRelayCommand(NextPageAsync, () => CurrentPage < TotalPages);
            LastPageCommand = new AsyncRelayCommand(LastPageAsync, () => CurrentPage < TotalPages);
        }

        private Task LoadAllBooksAsync()
        {
            return LoadAllPagedBooksAsync();
        }

        private Task ApplyFilterAsync()
        {
            CurrentPage = 1;
            return LoadFilteredPagedBooksAsync();
        }

        private Task ResetFilterAsync()
        {
            Filter.Reset();
            CurrentPage = 1;
            return LoadFilteredPagedBooksAsync();
        }

        private async Task AddBookAsync()
        {
            Book bookToBeAdded = new Book();
            if (_dialogService.ShowEditBookDialog(EditBookWindowMode.Add, bookToBeAdded) == true)
            {
                await _repository.AddAsync(bookToBeAdded);
                await LoadFilteredPagedBooksAsync();
            }
        }

        private async Task EditSelectedBookAsync()
        {
            if (SelectedBook != null && _dialogService.ShowEditBookDialog(EditBookWindowMode.Edit, SelectedBook) == true)
            {
                await _repository.UpdateAsync(SelectedBook);
            }
        }

        private async Task RemoveSelectedBookAsync()
        {
            if (SelectedBook != null)
            {
                string dialogMessage = $"Do you really want to remove the book \"{SelectedBook.Title}\" from the database?";
                if (_dialogService.ShowConfirmationDialog(dialogMessage, "Remove Book") == true)
                {
                    await _repository.RemoveAsync(SelectedBook);
                    await LoadFilteredPagedBooksAsync();
                }
            }
        }

        private Task FirstPageAsync()
        {
            CurrentPage = 1;
            return LoadFilteredPagedBooksAsync();
        }

        private Task PreviousPageAsync()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                return LoadFilteredPagedBooksAsync();
            }

            return Task.CompletedTask;
        }

        private Task NextPageAsync()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                return LoadFilteredPagedBooksAsync();
            }

            return Task.CompletedTask;
        }

        private Task LastPageAsync()
        {
            CurrentPage = TotalPages;
            return LoadFilteredPagedBooksAsync();
        }

        private async Task LoadAllPagedBooksAsync()
        {
            int count = await _repository.GetCountAsync();
            if (count == 0)
            {
                // Load sample data from json file
                await _repository.AddSampleDataAsync("books_dataset.json");
            }

            var pagedBooks = await _repository.GetAllPagedAsync(CurrentPage, _pageSize);
            Books.Clear();
            foreach (var book in pagedBooks.Books)
            {
                Books.Add(book);
            }

            TotalPages = pagedBooks.PageCount;
            SelectedBook = Books.FirstOrDefault();
        }

        private async Task LoadFilteredPagedBooksAsync()
        {
            var pagedBooks = await _repository.GetFilteredPagedAsync(CurrentPage, _pageSize, Filter);
            Books.Clear();
            foreach (var book in pagedBooks.Books)
            {
                Books.Add(book);
            }

            TotalPages = pagedBooks.PageCount;
            SelectedBook = Books.FirstOrDefault();
        }
    }
}
