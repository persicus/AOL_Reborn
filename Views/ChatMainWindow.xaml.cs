using AOL_Reborn.Data;
using AOL_Reborn.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace AOL_Reborn.Views
{
    public partial class ChatMainWindow : Window
    {
        private IChatService _chatService;
        private IMessageStorage _messageStorage;
        private User _currentUser;
        private string _chatPartner;
        public ObservableCollection<ChatMessage> Messages { get; set; } = []; // ✅ Ensure initialization

        public ChatMainWindow(User currentUser, string chatPartner, IChatService chatService, IMessageStorage messageStorage)
        {
            InitializeComponent();
            _chatService = chatService;
            _messageStorage = messageStorage;
            _currentUser = currentUser;
            _chatPartner = chatPartner;

            // ✅ Initialize Messages before setting DataContext
            var conversationMessages = _messageStorage.GetMessages(_currentUser.Username, _chatPartner);
            foreach (var message in conversationMessages)
            {
                Messages.Add(message);
            }

            DataContext = this; // ✅ Ensure DataContext is set **after** initializing Messages

            // Subscribe to incoming messages
            _chatService.MessageReceived += message =>

            {
                //if (_chatPartner != "EchoBot") return;// Only process messages when chatting with EchoBot
                Dispatcher.Invoke(() =>
                {
                    int conversationId = _messageStorage.GetOrCreateConversationId(_currentUser.Username, _chatPartner);
                    var senderName = _chatPartner == "EchoBot" ? "EchoBot" : _chatPartner;
                    var newMessage = ChatMessage.Create(senderName, _currentUser.Username, message, conversationId);

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
                using var db = new AppDbContext();
                // Pass the AppDbContext instance to the ChatRepository constructor
                ChatRepository chatRepo = new ChatRepository(db);
                int conversationId = chatRepo.GetOrCreateConversationId(_currentUser.Username, _chatPartner);

                var newMessage = ChatMessage.Create(_currentUser.Username, _chatPartner, MessageInput.Text, conversationId);

                db.Messages.Add(newMessage);
                db.SaveChanges(); // Save the message to the database

                Messages.Add(newMessage);
                await _chatService.SendMessageAsync(newMessage.Message);
                MessageInput.Clear();
            }
        }


        private void DeleteChatHistory_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"Are you sure you want to delete chat history with {_chatPartner}?",
                                         "Confirm Delete",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _messageStorage.DeleteChatHistory(_currentUser.Username, _chatPartner);
                Messages.Clear();
            }
        }
    }
}