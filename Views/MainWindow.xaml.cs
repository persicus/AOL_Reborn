using System.Collections.ObjectModel;
using System.Windows;
using AOL_Reborn.Models;
using AOL_Reborn.Services;
using AOL_Reborn.Data;

namespace AOL_Reborn.Views
{
    public partial class MainWindow : Window
    {
        private readonly IChatService _chatService;
        private readonly IMessageStorage _messageStorage;
        private readonly string _currentUser;
        private readonly string _chatPartner;
        public ObservableCollection<ChatMessage> Messages { get; set; }

        public MainWindow(string currentUser, string chatPartner, IChatService chatService, IMessageStorage messageStorage)
        {
            InitializeComponent();
            _chatService = chatService;
            _messageStorage = messageStorage;
            _currentUser = currentUser;
            _chatPartner = chatPartner;

            Messages = new ObservableCollection<ChatMessage>(_messageStorage.GetMessages(_currentUser, _chatPartner));
            DataContext = this;

            // Subscribe to incoming messages
            _chatService.MessageReceived += message =>
            {
                Dispatcher.Invoke(() =>
                {
                    var newMessage = new ChatMessage("EchoBot", _currentUser, message);
                    _messageStorage.SaveMessageAsync(newMessage);
                    Messages.Add(newMessage);
                });
            };

            // Connect to chat service
            _chatService.ConnectAsync("127.0.0.1", 5001, 5000);
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MessageInput.Text))
            {
                var userMessage = new ChatMessage(_currentUser, _chatPartner, MessageInput.Text);
                _messageStorage.SaveMessage(userMessage);
                Messages.Add(userMessage);
                await _chatService.SendMessageAsync(userMessage.Message);
                MessageInput.Clear();
            }
        }

        private void DeleteChatHistory_Click(object sender, RoutedEventArgs e)
        {
            _messageStorage.DeleteChatHistory(_currentUser, _chatPartner);
            Messages.Clear();
        }
    }
}
