using System.Windows;
using System;
using AOL_Reborn.Data;
using Microsoft.Extensions.DependencyInjection;

namespace AOL_Reborn
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AllocConsole(); // Opens a console window for debugging

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IChatService, NetworkChatService>();
            serviceCollection.AddSingleton<IMessageStorage, DatabaseMessageStorage>();
            ServiceProvider = serviceCollection.BuildServiceProvider();


            using (var db = new AppDbContext())
            {
                db.Database.EnsureCreated();
            }

           

        }
    }
}

