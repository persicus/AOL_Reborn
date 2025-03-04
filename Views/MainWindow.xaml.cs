using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using AOL_Reborn.Models;

namespace AOL_Reborn.Views
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<ChatMessage> Messages { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Messages = new ObservableCollection<ChatMessage>();
            DataContext = this;
            MessageInput.KeyDown += MessageInput_KeyDown; // Attach key event
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void MessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
                e.Handled = true; // Prevents new line in TextBox
            }
        }

        private void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(MessageInput.Text))
            {
                Messages.Add(new ChatMessage("You", MessageInput.Text));
                MessageInput.Clear();
                ScrollToBottom(); // Auto-scroll after adding a new message
            }
        }

        private void ScrollToBottom()
        {
            ChatScrollViewer.ScrollToEnd();
        }
    }
}
