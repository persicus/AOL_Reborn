using System.IO;
using System.Windows;
using System.Windows.Input;
using AOL_Reborn.Audio;
using AOL_Reborn.Data;
using AOL_Reborn.Models;
using AOL_Reborn.ViewModels;
using WpfMessageBox = System.Windows.MessageBox;

namespace AOL_Reborn.Views
{
    public partial class LoginWindow : Window
    {

        // Path to store the last logged username
        private static readonly string settingsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AOL_Reborn"
        );

        private static readonly string settingsPath = Path.Combine(settingsDirectory, "lastuser.txt");

        /* private void TestSoundButton_Click(object sender, RoutedEventArgs e)
        {
            // Log to the console (or Visual Studio's Output window)
            Console.WriteLine("TestSoundButton was clicked.");

            // Also, using Debug.WriteLine is a good option
            //Debug.WriteLine("TestSoundButton was clicked.");
            AudioManager.PlaySound("Voicy_Buddy In.mp3");
        }*/

        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
            LoadUsernames();
        }

        /// <summary>
        /// Loads usernames from the database into the ComboBox, inserts "<New User>",
        /// and selects the last logged user if found.
        /// </summary>
        private void LoadUsernames()
        {
            try
            {
                // Ensure directory exists
                if (!Directory.Exists(settingsDirectory))
                {
                    Directory.CreateDirectory(settingsDirectory);
                }

                using var db = new AppDbContext();

                // Get sorted list of all existing usernames
                var userNames = db.Users
                    .Select(u => u.Username)
                    .OrderBy(u => u)
                    .ToList();

                // Insert <New User> at the top
                userNames.Insert(0, "<New User>");

                // Set ItemsSource
                UsernameBox.ItemsSource = userNames;

                // Attempt to read the last logged user from file
                string? lastUser = null;
                if (File.Exists(settingsPath))
                {
                    lastUser = File.ReadAllText(settingsPath).Trim();
                }

                // If we have a last user and it exists in the list, select it.
                // Otherwise, default to index 0 (<New User>).
                if (!string.IsNullOrWhiteSpace(lastUser) && userNames.Contains(lastUser))
                {
                    UsernameBox.SelectedItem = lastUser;
                }
                else
                {
                    UsernameBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show($"Error loading usernames: {ex.Message}",
                                   "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Opens the network settings window so the user can change the IP and ports.
        /// If settings are saved, the network service is restarted.
        /// </summary>
        private void NetworkSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            if (settingsWindow.ShowDialog() == true)
            {
                // The updated settings will be used when BuddyListWindow loads.
                WpfMessageBox.Show("Network settings updated. They will take effect on next login.",
                                    "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            // The final text in the ComboBox is the typed or selected username
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password; // Potentially used later

            // If the user left <New User> in place or typed nothing, prompt them
            if (string.IsNullOrWhiteSpace(username) || username == "<New User>")
            {
                WpfMessageBox.Show("Please enter a valid screen name.", "Login Error",
                                   MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var db = new AppDbContext();

            // Check if the user already exists
            var existingUser = db.Users.FirstOrDefault(u => u.Username == username);

            if (existingUser == null)
            {
                // Create a new user record
                existingUser = new User
                {
                    Username = username,
                    DisplayName = username,
                    IsOnline = true
                };

                db.Users.Add(existingUser);
                db.SaveChanges();
            }
            else
            {
                // Mark user as online if needed
                existingUser.IsOnline = true;
                db.SaveChanges();
            }

            // Write the last logged user to file
            File.WriteAllText(settingsPath, username);

            // Store the user in the session
            SessionManager.SetCurrentUser(existingUser);

            //Play dialup sound :)
            AudioManager.PlaySound("dial_up.mp3");

            // Wait for 5 seconds
            await Task.Delay(TimeSpan.FromSeconds(5));

            // Open Buddy List window
            BuddyListWindow buddyList = new BuddyListWindow();
            buddyList.Show();
            this.Close();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement help functionality
            WpfMessageBox.Show("Help button clicked!");
        }

        // Custom title bar event handlers

        // Allow dragging the window when the user clicks the title bar
        private void CustomTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        // Minimize the window
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // Close the window
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
