using Chat.Data.Entity;
using Chat.Data.RequestModels;
using Chat.Data.ResponseModel;

namespace Chat.Data.Repositories.ChatMessageRepository
{
    public interface IChatMessageRepository
    {
        Task saveMessage(ChatMessageEntity chatMessageModel);
        Task<List<GetMessageHistoryRepoResponse>> GetHistoryMessages(GetMessageHistoryRequestModel getMessageHistoryRequestModel);
        Task<List<Users>> GetRecentChatUsers(GetRecentChatUsersRepoRequest getRecentChatUsersRepoRequest);
    }
}
