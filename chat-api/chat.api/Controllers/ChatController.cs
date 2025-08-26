using Chat.Api.Handlers.Users.Queries;
using Chat.Api.Handlers.Users.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediatr;
        public ChatController(IMediator mediatr) {
            _mediatr = mediatr;
        }

        [HttpGet("GetHistoryMessages/{senderId}/{receiverId}")]
        public async Task<List<GetMessageHistoryResponse>> GetMessageHistory(string senderId, string receiverId)
        {
            GetMessageHistoryQuery getMessageHistoryCommand = new GetMessageHistoryQuery
            {
                senderId = senderId,
                receiverId = receiverId
            };
            var result = await _mediatr.Send(getMessageHistoryCommand);
            return result;
        }
    }
}
