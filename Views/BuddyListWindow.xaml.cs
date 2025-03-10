using AOL_Reborn.Models;
using AOL_Reborn.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AOL_Reborn.Views
{
    public partial class BuddyListWindow : Window
    {
        private NetworkService _networkService;

        public BuddyListWindow()
        {
            InitializeComponent();
            _networkService = new NetworkService();

            // Properly subscribe to MessageReceived event
            _networkService.MessageReceived += (message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    Console.WriteLine($"Received message: {message}");
                });
            };

            Loaded += async (s, e) => await _networkService.ConnectAsync("127.0.0.1", 5000, 5001);
        }

        private void OpenSettingsWindow(object sender, MouseButtonEventArgs e)
        {
            SettingsWindow settingsWindow = new();
            settingsWindow.ShowDialog();
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
            return Properties.Settings.Default.AskSignOffConfirmation;
        }

        private static MessageBoxResult ShowSignOffConfirmation()
        {
            var messageBox = new SignOffConfirmationDialog();
            messageBox.ShowDialog();

            if (messageBox.DoNotAskAgain)
            {
                Properties.Settings.Default.AskSignOffConfirmation = false;
                Properties.Settings.Default.Save();
            }

            return messageBox.DialogResult == true ? MessageBoxResult.Yes : MessageBoxResult.No;
        }

        private void GoOffline()
        {
            _networkService.SendOfflineStatus();

            // Cleanup UDP client
            _networkService.Disconnect();
        }

        private void OpenLoginWindow()
        {
            LoginWindow loginWindow = new();
            loginWindow.Show();
            this.Close();
        }

        private void UpdateBuddyList(string friend, bool isOnline)
        {
            Dispatcher.Invoke(() =>
            {
                foreach (TreeViewItem group in BuddyTreeView.Items)
                {
                    foreach (TreeViewItem item in group.Items)
                    {
                        if (item.Header.ToString() == friend)
                        {
                            group.Items.Remove(item);
                            break;
                        }
                    }
                }

                var targetGroup = isOnline ? (TreeViewItem)BuddyTreeView.Items[0] : (TreeViewItem)BuddyTreeView.Items[1];
                targetGroup.Items.Add(new TreeViewItem { Header = friend });
            });
        }

        private void BuddyTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BuddyTreeView.SelectedItem is TreeViewItem selectedFriend)
            {
                string friendName = selectedFriend.Header?.ToString() ?? string.Empty;
                if (!string.IsNullOrEmpty(friendName))
                {
                    OpenChatWindow(friendName);
                }
            }
        }

        public async Task RestartNetworkService()
        {
            _networkService.Disconnect(); // Ensure we disconnect first
            _networkService = new NetworkService(); // Create a new instance
            _networkService.MessageReceived += (message) =>
            {
                Dispatcher.Invoke(() => Console.WriteLine($"Received message: {message}"));
            };

            _networkService.StatusUpdated += UpdateBuddyList;

            // Read updated settings and reconnect
            string serverIp = Properties.Settings.Default.ServerIp;
            int receivePort = int.Parse(Properties.Settings.Default.ReceivePort.ToString());
            int sendPort = int.Parse(Properties.Settings.Default.SendPort.ToString());

            await _networkService.ConnectAsync(serverIp, receivePort, sendPort); // Now using new settings
        }

  
        private static void OpenChatWindow(string friendName)
        {
            User currentUser = SessionManager.GetCurrentUser();
            IChatService chatService = new NetworkChatService();
            IMessageStorage messageStorage = new DatabaseMessageStorage();

            ChatMainWindow chatWindow = new(currentUser, friendName, chatService, messageStorage);
            chatWindow.Show();
        }
    }
}