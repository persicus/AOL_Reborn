using System.ComponentModel.DataAnnotations;

public class ChatMessage
{
    [Key]
    public int Id { get; set; }
    public string Sender { get; set; }
    public string Receiver { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;

    // Ensure the correct constructor exists
    public ChatMessage(string sender, string receiver, string message)
    {
        Sender = sender;
        Receiver = receiver;
        Message = message;
        Timestamp = DateTime.Now;
    }

    public ChatMessage() { }
}




