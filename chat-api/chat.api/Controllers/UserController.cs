using Chat.Api.Handlers.Users.Commands;
using Chat.Api.Handlers.Users.Queries;
using Chat.Api.Handlers.Users.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpPost("CreateUser")]
        public async Task<CreateUserResponse> CreateUser(CreateUserCommand userCreationModel)
        {
            var result = await _mediator.Send(userCreationModel);
            return result;
        }

        [HttpPost("Login")]
        public async Task<UserLoginResponse> Login(UserLoginCommand userLoginCommand)
        {
            var result = await _mediator.Send(userLoginCommand);
            return result;
        }

        [HttpGet("SearchUser")]
        public async Task<GetUsersResponse> GetUserList(string username)
        {
            GetUsersQuery getUsersCommand = new GetUsersQuery()
            {
                UserName = username
            };
            var result = await _mediator.Send(getUsersCommand);
            return result;
        }

        [HttpGet("GetRecentChatUsers/{userId}")]
        public async Task<List<UsersDto>> GetRecentChatUsers(string userId)
        {
            GetRecentChatUsersQuery getRecentChatUsersCommand = new GetRecentChatUsersQuery
            {
                userId = userId
            };
            var result = await _mediator.Send(getRecentChatUsersCommand);
            return result;
        }
    }
}
