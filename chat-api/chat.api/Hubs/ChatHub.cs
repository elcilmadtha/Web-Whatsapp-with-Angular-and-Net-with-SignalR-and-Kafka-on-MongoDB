using Chat.Data.Entity;
using Chat.Shared.Kafka.Topic;
using Chat.Shared.Publisher;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Hubs
{
    public class ChatHub:Hub
    {
        private readonly IKafkaPublisher _kafkaPublisher;
        public ChatHub(IKafkaPublisher kafkaPublisher)
        {
            _kafkaPublisher = kafkaPublisher;
        }

        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext?.Request.Query["userId"].ToString();
            if (!string.IsNullOrEmpty(userId))
            {
                Context.Items["UserId"] = userId; 
            }
            Console.WriteLine($"SignalR: User connected with ID: {userId}");
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(ChatMessageEntity chatMessage)
        {
            try
            {
                await Clients.Users(chatMessage.Sender, chatMessage.Receiver).SendAsync("ReceiveMessage", chatMessage);
                var message = new PublishMessagestoEntityCommand
                {
                    Sender = chatMessage.Sender,
                    Receiver = chatMessage.Receiver,
                    Content = chatMessage.Content,
                    Timestamp = chatMessage.Timestamp,
                };
                await _kafkaPublisher.PublishAsync("PublishMessagestoEntityCommand", message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignalR error: {ex.Message}");
                Console.WriteLine("🔍 StackTrace: " + ex.StackTrace);
                throw;
            }
        }
    }
}
