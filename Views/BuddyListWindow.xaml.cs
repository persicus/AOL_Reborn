using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AOL_Reborn.Data;
using AOL_Reborn.Models;
using AOL_Reborn.Services;
using AOL_Reborn.Properties;

namespace AOL_Reborn.Views
{
    public partial class BuddyListWindow : Window
    {
        private NetworkService _networkService;

        public BuddyListWindow()
        {
            InitializeComponent();

            var currentUser = SessionManager.GetCurrentUser();
            this.Title = $"{currentUser.Username}'s Buddy List Window";

            // Initialize the network service and subscribe to its events
            _networkService = new NetworkService();
            _networkService.MessageReceived += message =>
            {
                Dispatcher.Invoke(() =>
                {
                    Console.WriteLine($"Received message: {message}");
                });
            };

            // Connect when the window loads
            Loaded += async (s, e) =>
            {
                string serverIp = Settings.Default.ServerIp;
                int receivePort = Settings.Default.ReceivePort;
                int sendPort = Settings.Default.SendPort;
                await _networkService.ConnectAsync(serverIp, receivePort, sendPort);
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Optionally load buddy groups here from your data or repository
        }

        private void AddBuddyButton_Click(object sender, RoutedEventArgs e)
        {
            // Prompt for buddy's username
            string buddyUsername = Microsoft.VisualBasic.Interaction.InputBox("Enter the buddy's username:", "Add Buddy", "");
            if (string.IsNullOrWhiteSpace(buddyUsername)) return;

            using var db = new AppDbContext();
            var friend = db.Users.FirstOrDefault(u => u.Username.Equals(buddyUsername, StringComparison.OrdinalIgnoreCase));
            if (friend == null)
            {
                MessageBox.Show("User not found in the system.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // TODO: Insert logic to add to the DB (e.g., using a FriendshipRepository)

            // For now, just add a TreeViewItem under the "Buddies" group in the UI:
            var buddiesItem = BuddyTreeView.Items.OfType<TreeViewItem>()
                .FirstOrDefault(i => i.Header.ToString().StartsWith("Buddies"));
            if (buddiesItem != null)
            {
                buddiesItem.Items.Add(new TreeViewItem { Header = friend.Username });
            }
        }

        private void RemoveBuddyButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove the selected buddy from the "Buddies" group
            if (BuddyTreeView.SelectedItem is TreeViewItem selectedItem)
            {
                if (selectedItem.Parent is TreeViewItem parentGroup &&
                    parentGroup.Header.ToString().StartsWith("Buddies"))
                {
                    parentGroup.Items.Remove(selectedItem);
                    // TODO: Also remove from DB (using a FriendshipRepository)
                }
            }
            else
            {
                MessageBox.Show("Please select a buddy to remove.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Restarts the network service by disconnecting, re-instantiating, re-subscribing to events,
        /// reading updated settings, and reconnecting.
        /// </summary>
        public async Task RestartNetworkService()
        {
            _networkService.Disconnect();
            _networkService = new NetworkService();
            _networkService.MessageReceived += message =>
            {
                Dispatcher.Invoke(() =>
                {
                    Console.WriteLine($"Received message: {message}");
                });
            };
            _networkService.StatusUpdated += (friendName, isOnline) =>
            {
                Dispatcher.Invoke(() =>
                {
                    UpdateBuddyList(friendName, isOnline);
                });
            };
            string serverIp = Settings.Default.ServerIp;
            int receivePort = Settings.Default.ReceivePort;
            int sendPort = Settings.Default.SendPort;
            await _networkService.ConnectAsync(serverIp, receivePort, sendPort);
            MessageBox.Show("Network service restarted successfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateBuddyList(string friendName, bool isOnline)
        {
            Console.WriteLine($"UpdateBuddyList: {friendName} is {(isOnline ? "online" : "offline")}");
        }

        /// <summary>
        /// Event handler for the Restart Network button.
        /// </summary>
        private async void RestartNetworkButton_Click(object sender, RoutedEventArgs e)
        {
            await RestartNetworkService();
        }

        /// <summary>
        /// Signs off the current user after confirming.
        /// </summary>
        private void SignOff_Click(object sender, RoutedEventArgs e)
        {
            if (ShouldAskForSignOffConfirmation())
            {
                var result = ShowSignOffConfirmation();
                if (result != MessageBoxResult.Yes)
                    return;
            }
            GoOffline();
            OpenLoginWindow();
        }

        private static bool ShouldAskForSignOffConfirmation()
        {
            return Settings.Default.AskSignOffConfirmation;
        }

        private static MessageBoxResult ShowSignOffConfirmation()
        {
            // Assuming you have a SignOffConfirmationDialog implemented
            var messageBox = new SignOffConfirmationDialog();
            messageBox.ShowDialog();
            if (messageBox.DoNotAskAgain)
            {
                Settings.Default.AskSignOffConfirmation = false;
                Settings.Default.Save();
            }
            return messageBox.DialogResult == true ? MessageBoxResult.Yes : MessageBoxResult.No;
        }

        private void GoOffline()
        {
            _networkService.SendOfflineStatus();
            _networkService.Disconnect();
        }

        private void OpenLoginWindow()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

       
    }
}
