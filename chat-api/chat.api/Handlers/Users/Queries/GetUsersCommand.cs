using Chat.Api.Handlers.Users.Responses;
using MediatR;

namespace Chat.Api.Handlers.Users.Queries
{
    public class GetUsersQuery: IRequest<GetUsersResponse>
    {
        public string UserName { get; set; }
    }
}
