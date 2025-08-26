using Chat.Api.Handlers.Users.Responses;
using MediatR;

namespace Chat.Api.Handlers.Users.Commands
{
    public class UserLoginCommand : IRequest<UserLoginResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
