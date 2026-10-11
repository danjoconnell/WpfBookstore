using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WpfBookstore.Models;
using WpfBookstore.ViewModels;
using WpfBookstore.Views;

namespace WpfBookstore.Services
{
    public class DialogService : IDialogService
    {
        public bool? ShowEditBookDialog(EditBookWindowMode editMode, Book book)
        {
            var editBookWindow = App.ServiceProvider.GetRequiredService<EditBookWindow>();
            editBookWindow.WindowStyle = WindowStyle.ToolWindow;
            editBookWindow.SizeToContent = SizeToContent.WidthAndHeight;

            var viewModel = (EditBookWindowViewModel)editBookWindow.DataContext;
            viewModel.EditMode = editMode;
            viewModel.Book = book;

            return editBookWindow.ShowDialog();
        }

        public bool ShowConfirmationDialog(string dialogText, string dialogCaption = "")
        {
            return MessageBox.Show(dialogText, dialogCaption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }
    }
}
