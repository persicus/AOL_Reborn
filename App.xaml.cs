using AOL_Reborn.Data;
using AOL_Reborn.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using System.Windows;

namespace AOL_Reborn
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider? ServiceProvider { get; private set; }

        [LibraryImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool AllocConsole();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AllocConsole(); // Opens a console window for debugging

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IChatService, NetworkChatService>();
            serviceCollection.AddSingleton<IMessageStorage, DatabaseMessageStorage>();
            ServiceProvider = serviceCollection.BuildServiceProvider();

            using var db = new AppDbContext();
            db.Database.EnsureCreated();
        }
    }
}