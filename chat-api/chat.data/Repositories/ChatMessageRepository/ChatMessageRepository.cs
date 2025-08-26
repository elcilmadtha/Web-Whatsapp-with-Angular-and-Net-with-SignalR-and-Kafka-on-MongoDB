using Chat.Api.Models.MongoDB;
using Chat.Data.Entity;
using Chat.Data.RequestModels;
using Chat.Data.ResponseModel;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Chat.Data.Repositories.ChatMessageRepository
{
    public class ChatMessageRepository: IChatMessageRepository
    {
        private readonly IMongoCollection<ChatMessageEntity> _chatMessagesCollection;
        private readonly IMongoCollection<UserCreationEntity> _usersCollection;

        public ChatMessageRepository(IOptions<MongoDbSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _chatMessagesCollection = database.GetCollection<ChatMessageEntity>(settings.Value.MessagesCollection);
            _usersCollection = database.GetCollection<UserCreationEntity>(settings.Value.UsersCollection);
        }

        public async Task saveMessage(ChatMessageEntity chatMessageModel)
        {
            await _chatMessagesCollection.InsertOneAsync(chatMessageModel);
        }

        public async Task<List<GetMessageHistoryRepoResponse>> GetHistoryMessages(GetMessageHistoryRequestModel getMessageHistoryRequestModel)
        {
            var result = await _chatMessagesCollection.Find(x => 
            (x.Sender == getMessageHistoryRequestModel.senderId && x.Receiver == getMessageHistoryRequestModel.receiverId) ||
            (x.Sender == getMessageHistoryRequestModel.receiverId && x.Receiver == getMessageHistoryRequestModel.senderId)
            )
                .SortBy(y => y.Timestamp).Project(z => new GetMessageHistoryRepoResponse
                {
                    Content = z.Content,
                    Timestamp = z.Timestamp,
                    Sender = z.Sender,
                    Receiver = z.Receiver
                }).ToListAsync();
            return result;
        }

        public async Task<List<Users>> GetRecentChatUsers(GetRecentChatUsersRepoRequest getRecentChatUsersRepoRequest)
        { 
        var recentMessages = await _chatMessagesCollection.Find(msg => msg.Sender == getRecentChatUsersRepoRequest.userId ||
        msg.Receiver == getRecentChatUsersRepoRequest.userId).SortByDescending(m => m.Timestamp).Limit(100).ToListAsync();

        var userIds = recentMessages.Select(m => m.Sender == getRecentChatUsersRepoRequest.userId ? m.Receiver : m.Sender).Distinct().ToList();

        var distrinctUsers = await _usersCollection.Find(u => userIds.Contains(u.Id)).Project(x => new Users
            { Id = x.Id, Name = x.Name, Username = x.Username, Email = x.Email }).ToListAsync();

            return distrinctUsers;
        }
    }
}
