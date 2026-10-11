using WpfBookstore.Models;
using WpfBookstore.ViewModels;

namespace WpfBookstore.Services
{
    public interface IDialogService
    {
        bool? ShowEditBookDialog(EditBookWindowMode editMode, Book book);
        bool ShowConfirmationDialog(string dialogText, string dialogCaption = "");
    }
}