using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using WpfBookstore.Data;
using WpfBookstore.Services;
using WpfBookstore.ViewModels;
using WpfBookstore.Views;

namespace WpfBookstore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            // Configure EF Core with SQLite DbContext
            services.AddDbContext<BooksContext>(options => options.UseSqlite("Data Source=books.db"));
            services.AddSingleton<IBookRepository, BookRepository>();

            // Register Services
            services.AddSingleton<IDialogService, DialogService>();

            // Register ViewModels and Windows
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
            services.AddTransient<EditBookWindowViewModel>();
            services.AddTransient<EditBookWindow>();

            ServiceProvider = services.BuildServiceProvider();

            // Ensure DB and tables exist
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BooksContext>();
                dbContext.Database.EnsureCreated();
            }

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }

}
