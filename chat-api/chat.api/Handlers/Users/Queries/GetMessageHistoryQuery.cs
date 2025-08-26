using Chat.Api.Handlers.Users.Responses;
using MediatR;

namespace Chat.Api.Handlers.Users.Queries
{
    public class GetMessageHistoryQuery: IRequest<List<GetMessageHistoryResponse>>
    {
        public string senderId { get; set; }
        public string receiverId { get; set; }
    }
}
