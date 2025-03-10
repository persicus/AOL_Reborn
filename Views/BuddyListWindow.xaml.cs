using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

            // Initialize the network service and subscribe to events
            _networkService = new NetworkService();
            _networkService.MessageReceived += message =>
            {
                Dispatcher.Invoke(() =>
                {
                    Console.WriteLine($"Received message: {message}");
                });
            };

            // When the window loads, connect the network service and update mock friend data
            Loaded += async (s, e) =>
            {
                string serverIp = Settings.Default.ServerIp;
                int receivePort = Settings.Default.ReceivePort;
                int sendPort = Settings.Default.SendPort;
                await _networkService.ConnectAsync(serverIp, receivePort, sendPort);

                // Add mock friends ("Scott", "Jared", and "EchoBot")
                AddMockFriends();
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Additional loading logic if needed.
        }

        private void AddBuddyButton_Click(object sender, RoutedEventArgs e)
        {
            // Prompt for a buddy's username
            string buddyUsername = Microsoft.VisualBasic.Interaction.InputBox("Enter the buddy's username:", "Add Buddy", "");
            if (string.IsNullOrWhiteSpace(buddyUsername)) return;

            using var db = new AppDbContext();
            var friend = db.Users.FirstOrDefault(u => u.Username.ToLower() == buddyUsername.ToLower());
            if (friend == null)
            {
                MessageBox.Show("User not found in the system.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // For simplicity, add the friend to the "Buddies" group in the UI.
            AddFriendToBuddies(friend);
        }

        private void RemoveBuddyButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove the selected buddy from whichever group it belongs to.
            if (BuddyTreeView.SelectedItem is TreeViewItem selectedItem)
            {
                if (selectedItem.Parent is TreeViewItem parentGroup)
                {
                    parentGroup.Items.Remove(selectedItem);
                    // Update the group header count.
                    string groupName = parentGroup.Header.ToString().Split('(')[0].Trim();
                    parentGroup.Header = $"{groupName} ({parentGroup.Items.Count})";
                }
            }
            else
            {
                MessageBox.Show("Please select a buddy to remove.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

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
                Dispatcher.Invoke(() => UpdateBuddyList(friendName, isOnline));
            };

            string serverIp = Settings.Default.ServerIp;
            int receivePort = Settings.Default.ReceivePort;
            int sendPort = Settings.Default.SendPort;
            await _networkService.ConnectAsync(serverIp, receivePort, sendPort);
            MessageBox.Show("Network service restarted successfully.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateBuddyList(string friendName, bool isOnline)
        {
            // Update UI elements for a buddy's online status (placeholder for now).
            Console.WriteLine($"UpdateBuddyList: {friendName} is {(isOnline ? "online" : "offline")}");
        }

        private async void RestartNetworkButton_Click(object sender, RoutedEventArgs e)
        {
            await RestartNetworkService();
        }

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

        private void NetworkSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            if (settingsWindow.ShowDialog() == true)
            {
                RestartNetworkButton_Click(sender, e);
            }
        }

        private void AddMockFriends()
        {
            var currentUser = SessionManager.GetCurrentUser();
            using (var db = new AppDbContext())
            {
                // Ensure friend "Scott" exists.
                var scott = db.Users.FirstOrDefault(u => u.Username.ToLower() == "scott");
                if (scott == null)
                {
                    scott = new User { Username = "Scott", DisplayName = "Scott", IsOnline = false };
                    db.Users.Add(scott);
                    db.SaveChanges();
                }
                // Ensure friend "Jared" exists.
                var jared = db.Users.FirstOrDefault(u => u.Username.ToLower() == "jared");
                if (jared == null)
                {
                    jared = new User { Username = "Jared", DisplayName = "Jared", IsOnline = false };
                    db.Users.Add(jared);
                    db.SaveChanges();
                }
                // Ensure EchoBot exists.
                var echoBot = db.Users.FirstOrDefault(u => u.Username.ToLower() == "echobot");
                if (echoBot == null)
                {
                    echoBot = new User { Username = "EchoBot", DisplayName = "EchoBot", IsOnline = true };
                    db.Users.Add(echoBot);
                    db.SaveChanges();
                }
                // Use ChatRepository to create/retrieve conversation entries.
                var chatRepo = new ChatRepository(db);
                chatRepo.GetOrCreateConversationId(currentUser.Username, scott.Username);
                chatRepo.GetOrCreateConversationId(currentUser.Username, jared.Username);
                chatRepo.GetOrCreateConversationId(currentUser.Username, echoBot.Username);

                // Add friends to the buddy list
                AddFriendToBuddies(scott);
                AddFriendToBuddies(jared);
                AddFriendToBuddies(echoBot);
            }
        }

        private void AddFriendToBuddies(User friend)
        {
            var buddiesItem = BuddyTreeView.Items.OfType<TreeViewItem>()
                .FirstOrDefault(i => i.Header.ToString().StartsWith("Buddies"));
            if (buddiesItem != null)
            {
                // Create a TreeViewItem for the friend and store the User object in Tag
                TreeViewItem friendItem = new TreeViewItem
                {
                    Header = friend.Username,
                    Tag = friend
                };
                buddiesItem.Items.Add(friendItem);
                buddiesItem.Header = $"Buddies ({buddiesItem.Items.Count})";
            }
        }

        private void BuddyTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BuddyTreeView.SelectedItem is TreeViewItem selectedItem &&
                selectedItem.Parent is TreeViewItem)
            {
                if (selectedItem.Tag is User friendUser)
                {
                    OpenChatWindow(friendUser.Username);
                }
                else
                {
                    OpenChatWindow(selectedItem.Header.ToString());
                }
            }
        }

        private void OpenChatWindow(string friendName)
        {
            // Get the current user from session
            var currentUser = SessionManager.GetCurrentUser();

            // Create or retrieve IChatService and IMessageStorage instances
            IChatService chatService = NetworkChatService.Instance;
            IMessageStorage messageStorage = new DatabaseMessageStorage();

            // Instantiate and show the ChatMainWindow
            // New code using the shared instance and 3-parameter constructor:
            ChatMainWindow chatWindow = new ChatMainWindow(currentUser, friendName, messageStorage);
            chatWindow.Show();
        }
    }
}
