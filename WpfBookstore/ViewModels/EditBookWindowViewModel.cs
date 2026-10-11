using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WpfBookstore.Models;

namespace WpfBookstore.ViewModels
{
    public enum EditBookWindowMode
    {
        Add,
        Edit
    }

    public partial class EditBookWindowViewModel : ObservableObject
    {
        public event Action<bool?>? CloseRequested;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(WindowTitle))]
        private EditBookWindowMode _editMode = EditBookWindowMode.Edit;

        public string WindowTitle
        {
            get
            {
                return (EditMode == EditBookWindowMode.Edit) ? "Edit Book" : "Add Book";
            }
        }

        [ObservableProperty]
        private Book? _book;

        public IRelayCommand AddOrUpdateBookCommand { get; }
        public IRelayCommand CancelCommand { get; }

        public EditBookWindowViewModel()
        {
            AddOrUpdateBookCommand = new RelayCommand(AddOrUpdateBook);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void AddOrUpdateBook()
        {
            RequestClose(true);
        }

        private void Cancel()
        {
            RequestClose(false);
        }

        private void RequestClose(bool? result)
        {
            CloseRequested?.Invoke(result);
        }
    }
}
