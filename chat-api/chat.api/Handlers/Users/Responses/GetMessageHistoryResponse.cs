namespace Chat.Api.Handlers.Users.Responses
{
    public class GetMessageHistoryResponse
    {
        public string Content { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
