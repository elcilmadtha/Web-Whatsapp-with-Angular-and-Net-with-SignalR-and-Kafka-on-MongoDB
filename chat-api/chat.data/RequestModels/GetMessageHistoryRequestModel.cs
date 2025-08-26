
namespace Chat.Data.RequestModels
{
    public class GetMessageHistoryRequestModel
    {
        public string senderId { get; set; }
        public string receiverId { get; set; }
    }
}
