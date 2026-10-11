using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfBookstore.ViewModels;

namespace WpfBookstore.Views
{
    /// <summary>
    /// Interaction logic for EditBookWindow.xaml
    /// </summary>
    public partial class EditBookWindow : Window
    {
        public EditBookWindow(EditBookWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditBookWindowViewModel viewModel)
            {
                viewModel.CloseRequested += r =>
                {
                    DialogResult = r;
                    Close();
                };
            }
        }
    }
}
