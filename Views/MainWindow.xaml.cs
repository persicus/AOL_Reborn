using System.Collections.ObjectModel;
using System.Windows;
using AOL_Reborn.Models;
using AOL_Reborn.Services;

namespace AOL_Reborn.Views
{
    public partial class MainWindow : Window
    {
        private NetworkService _networkService;
        public ObservableCollection<ChatMessage> Messages { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Messages = new ObservableCollection<ChatMessage>();
            DataContext = this;
            _networkService = new NetworkService();

            // Connect to EchoBot when the window loads
            Loaded += async (s, e) => await _networkService.ConnectAsync("127.0.0.1", 0, 1);

            // Subscribe to incoming messages from EchoBot
            _networkService.MessageReceived += message =>
            {
                Dispatcher.Invoke(() =>
                {
                    Messages.Add(new ChatMessage("EchoBot", message));
                });
            };
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MessageInput.Text))
            {
                string userMessage = MessageInput.Text;
                Messages.Add(new ChatMessage("You", userMessage));
                await _networkService.SendMessageAsync(userMessage);
                MessageInput.Clear();
            }
        }
    }
}
