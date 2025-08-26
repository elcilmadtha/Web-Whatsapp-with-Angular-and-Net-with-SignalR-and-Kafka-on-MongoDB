namespace Chat.Api.Hubs.Model
{
    public class ChatMessageModel
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
