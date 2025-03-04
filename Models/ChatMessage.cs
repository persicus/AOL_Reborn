namespace AOL_Reborn.Models
{
    public class ChatMessage
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; }

        public ChatMessage(string username, string message)
        {
            this.Username = username;
            this.Message = message;
            Timestamp = System.DateTime.Now.ToString("hh:mm tt");
        }
    }
}
