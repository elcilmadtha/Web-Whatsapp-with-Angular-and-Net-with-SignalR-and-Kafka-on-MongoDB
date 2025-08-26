using Chat.Api.Handlers.Users.Responses;
using MediatR;

namespace Chat.Api.Handlers.Users.Commands
{
    public class CreateUserCommand:  IRequest<CreateUserResponse>
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
