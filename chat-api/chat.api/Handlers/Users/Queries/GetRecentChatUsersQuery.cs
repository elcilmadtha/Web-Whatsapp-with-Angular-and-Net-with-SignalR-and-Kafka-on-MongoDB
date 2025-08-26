using Chat.Api.Handlers.Users.Responses;
using MediatR;

namespace Chat.Api.Handlers.Users.Queries
{
    public class GetRecentChatUsersQuery: IRequest<List<UsersDto>>
    {
        public string userId { get; set; }
    }
}
