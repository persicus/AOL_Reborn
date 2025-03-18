using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using AOL_Reborn.Audio;
using AOL_Reborn.Data;
using AOL_Reborn.Models;
using WpfMessageBox = System.Windows.MessageBox;

namespace AOL_Reborn.Views
{
    public partial class ChatMainWindow : Window
    {
        IChatService _chatService;
        IMessageStorage _messageStorage;
        User _currentUser;
        string _chatPartner;
        public ObservableCollection<ChatMessage> Messages { get; set; } = new ObservableCollection<ChatMessage>();

        public ChatMainWindow(User currentUser, string chatPartner, IMessageStorage messageStorage)
        {
            InitializeComponent();
            Title = $"Chat with {chatPartner}";

            // Use the shared instance of NetworkChatService
            _chatService = NetworkChatService.Instance;
            _messageStorage = messageStorage;
            _currentUser = currentUser;
            _chatPartner = chatPartner;

            // Load previous conversation messages from storage
            var conversationMessages = _messageStorage.GetMessages(_currentUser.Username, _chatPartner);

            foreach (var message in conversationMessages)
                Messages.Add(message);
            

            DataContext = this;

            // Auto-scroll: scroll to bottom when a new message is added.
            Messages.CollectionChanged += Messages_CollectionChanged;

            // Auto-scroll on window loaded
            Loaded += (s, e) => ChatScrollViewer.ScrollToBottom();

            // Subscribe to incoming messages (for non-mock users)
            _chatService.MessageReceived += message =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (IsMockUser(_chatPartner)) return;

                    var conversationId = _messageStorage.GetOrCreateConversationId(_currentUser.Username, _chatPartner);
                    var newMessage = ChatMessage.Create(_chatPartner, _currentUser.Username, message, conversationId);
                    _messageStorage.SaveMessageAsync(newMessage);
                    Messages.Add(newMessage);
                });
            };

            // Connect once (if not already connected)
            _chatService.ConnectAsync("127.0.0.1", 5001, 5000);
        }

        void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ChatScrollViewer.ScrollToBottom();
            }
        }

        async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MessageInput.Text))
            {
                using var db = new AppDbContext();
                var chatRepo = new ChatRepository(db);
                var conversationId = chatRepo.GetOrCreateConversationId(_currentUser.Username, _chatPartner);

                var newMessage = ChatMessage.Create(_currentUser.Username, _chatPartner, MessageInput.Text, conversationId);
                db.Messages.Add(newMessage);
                db.SaveChanges();

                Messages.Add(newMessage);
                await _chatService.SendMessageAsync(newMessage.Message);
                var userMessage = MessageInput.Text;
                MessageInput.Clear();

                // For mock users, simulate an auto-reply after a delay.
                if (IsMockUser(_chatPartner))
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    AudioManager.PlaySound("Voicy_Instant Message.mp3");
                    SimulateAutoReply(userMessage);
                }
            }
        }

        void DeleteChatHistory_Click(object sender, RoutedEventArgs e)
        {
            var result = WpfMessageBox.Show($"Are you sure you want to clear chat history with {_chatPartner}?",
                                          "Confirm Clear", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _messageStorage.DeleteChatHistory(_currentUser.Username, _chatPartner);
                Messages.Clear();
            }
        }

        // Determines if the chat partner is one of our mock users.
        bool IsMockUser(string chatPartner)
        {
            return chatPartner.Equals("EchoBot", StringComparison.OrdinalIgnoreCase) ||
                   chatPartner.Equals("Scott", StringComparison.OrdinalIgnoreCase) ||
                   chatPartner.Equals("Jared", StringComparison.OrdinalIgnoreCase);
        }

        // Simulates an auto-reply from the chat partner.
        void SimulateAutoReply(string userMessage)
        {
            var reply = string.Empty;

            if (_chatPartner.Equals("EchoBot", StringComparison.OrdinalIgnoreCase))
            {
                reply = $"You said \"{userMessage}\"?....,C'mon dudenothing better to say?";
            }
            else if (_chatPartner.Equals("Scott", StringComparison.OrdinalIgnoreCase))
            {
                reply = "Hi, I'm Scott. Thanks for your message!";
            }
            else if (_chatPartner.Equals("Jared", StringComparison.OrdinalIgnoreCase))
            {
                reply = "Hey, it's Jared. I'll get back to you soon.";
            }
            else
            {
                reply = "Auto-reply: Message received.";
            }

            var conversationId = _messageStorage.GetOrCreateConversationId(_currentUser.Username, _chatPartner);
            var autoReplyMessage = ChatMessage.Create(_chatPartner, _currentUser.Username, reply, conversationId);
            _messageStorage.SaveMessageAsync(autoReplyMessage);
            Messages.Add(autoReplyMessage);
        }
    }

    // Converter: Returns Red if the message sender is the current user, Blue otherwise.
    public class MessageSenderToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var senderName = value as string;
            var currentUsername = SessionManager.GetCurrentUser().Username;

            if (string.Equals(senderName, currentUsername, StringComparison.OrdinalIgnoreCase))
            {
                return System.Windows.Media.Brushes.Red;
            }
            else
            {
                return System.Windows.Media.Brushes.Blue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}