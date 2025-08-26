using AutoMapper;
using Chat.Api.Handlers.Users.Responses;
using Chat.Data.Repositories.ChatMessageRepository;
using Chat.Data.RequestModels;
using MediatR;

namespace Chat.Api.Handlers.Users.Queries
{
    public class GetRecentChatUsersQueryHandler : IRequestHandler<GetRecentChatUsersQuery, List<UsersDto>>
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IMapper _mapper;
        public GetRecentChatUsersQueryHandler(IChatMessageRepository chatMessageRepository, IMapper mapper) {
            _chatMessageRepository = chatMessageRepository;
            _mapper = mapper;
        }

        public async Task<List<UsersDto>> Handle(GetRecentChatUsersQuery request, CancellationToken cancellationToken)
        {
            var mappedRequestModel = _mapper.Map<GetRecentChatUsersRepoRequest>(request);
            var result = await _chatMessageRepository.GetRecentChatUsers(mappedRequestModel);
            var mappedResponseModel = _mapper.Map<List<UsersDto>>(result);
            return mappedResponseModel;
        }
    }
}
