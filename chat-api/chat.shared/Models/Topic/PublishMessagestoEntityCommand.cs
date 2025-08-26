using MediatR;

namespace Chat.Shared.Kafka.Topic
{
    public class PublishMessagestoEntityCommand: IRequest<bool>
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
