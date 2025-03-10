// LoginWindow.xaml.cs (Code-behind for login UI)
using AOL_Reborn.Data;
using AOL_Reborn.Models;
using AOL_Reborn.Services;
using AOL_Reborn.ViewModels;
using System.IO;
using System.Windows;

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
            string username = UsernameBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter a screen name.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var db = new AppDbContext();

            // ✅ Check if the user already exists
            User? existingUser = db.Users.FirstOrDefault(u => u.Username == username);

            if (existingUser == null)
            {
                // ✅ Create a new user if it doesn't exist
                existingUser = new User
                {
                    Username = username,
                    DisplayName = username, // Default display name is the same as username
                    IsOnline = true
                };

                db.Users.Add(existingUser);
                db.SaveChanges(); // ✅ This must be here to persist changes!
            }

            // ✅ Store user session
            SessionManager.SetCurrentUser(existingUser);

            // ✅ Open the Buddy List Window
            BuddyListWindow buddyList = new BuddyListWindow();
            buddyList.Show();
            this.Close();
        }



    }
}