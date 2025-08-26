namespace Chat.Data.ResponseModel
{
    public class GetMessageHistoryRepoResponse
    {
        public string Content { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
