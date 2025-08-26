using Chat.Data.Entity;
using Chat.Data.Repositories.ChatMessageRepository;
using Chat.Shared.Kafka.Topic;
using MediatR;

namespace Chat.Shared.Models.Topic
{
    public class PublishMessagestoEntityCommandHandler : IRequestHandler<PublishMessagestoEntityCommand, bool>
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        public PublishMessagestoEntityCommandHandler(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }
        public Task<bool> Handle(PublishMessagestoEntityCommand request, CancellationToken cancellationToken)
        {
            var chatMessageEntity = new ChatMessageEntity
            {
                Sender = request.Sender,
                Receiver = request.Receiver,
                Content = request.Content,
                Timestamp = request.Timestamp
            };
            _chatMessageRepository.saveMessage(chatMessageEntity);
            return Task.FromResult(true);
        }
    }
}
