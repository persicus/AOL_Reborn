using System.Windows;
using System.Windows.Controls;
using AOL_Reborn.Services;
using AOL_Reborn.Properties;
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

        private bool ShouldAskForSignOffConfirmation()
        {
            return Properties.Settings.Default.AskSignOffConfirmation;
        }

        private MessageBoxResult ShowSignOffConfirmation()
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
            LoginWindow loginWindow = new LoginWindow();
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
                string friendName = selectedFriend.Header.ToString();
                if (!string.IsNullOrEmpty(friendName))
                {
                    OpenChatWindow(friendName);
                }
            }
        }

        private void OpenChatWindow(string friendName)
        {
            MainWindow chatWindow = new MainWindow();
            chatWindow.Title = $"Chat with {friendName}";
            chatWindow.Show();
        }


    }
}
