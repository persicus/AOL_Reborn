// LoginWindow.xaml.cs (Code-behind for login UI)
using System.IO;
using System.Windows;
using AOL_Reborn.ViewModels;

namespace AOL_Reborn.Views
{
    public partial class LoginWindow : Window
    {
        private static string settingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AOL_Reborn", "username.txt");

        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel(); //  Bind ViewModel to the UI
            LoadUsername();
        }

        private void LoadUsername()
        {
            try
            {
                if (File.Exists(settingsPath))
                {
                    string storedUsername = File.ReadAllText(settingsPath).Trim();
                    if (!string.IsNullOrWhiteSpace(storedUsername))
                    {
                        ((LoginViewModel)DataContext).Username = storedUsername; //  Set ViewModel property
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading username: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter a username.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string? directoryPath = Path.GetDirectoryName(settingsPath);
            if (!string.IsNullOrEmpty(directoryPath)) // Prevent null reference
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(settingsPath, username);

            MessageBox.Show($"Welcome, {username}!", "Login Successful", MessageBoxButton.OK, MessageBoxImage.Information);

            //Open Buddy List Window instead of Main Chat Window
            BuddyListWindow buddyListWindow = new BuddyListWindow();
            buddyListWindow.Show();

            this.Close();
        }

    }
}
