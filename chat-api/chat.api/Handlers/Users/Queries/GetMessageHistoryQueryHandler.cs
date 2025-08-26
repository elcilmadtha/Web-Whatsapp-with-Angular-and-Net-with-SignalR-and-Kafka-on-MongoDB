using AutoMapper;
using Chat.Api.Handlers.Users.Responses;
using Chat.Data.Repositories.ChatMessageRepository;
using Chat.Data.RequestModels;
using MediatR;

namespace Chat.Api.Handlers.Users.Queries
{
    public class GetMessageHistoryQueryHandler : IRequestHandler<GetMessageHistoryQuery, List<GetMessageHistoryResponse>>
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IMapper _mapper;
        public GetMessageHistoryQueryHandler(IChatMessageRepository chatMessageRepository, IMapper mapper) {
            _chatMessageRepository = chatMessageRepository;
            _mapper = mapper;
        }
        public async Task<List<GetMessageHistoryResponse>> Handle(GetMessageHistoryQuery request, CancellationToken cancellationToken)
        {
            var mappedRequest = _mapper.Map<GetMessageHistoryRequestModel>(request);
            var result = await _chatMessageRepository.GetHistoryMessages(mappedRequest);
            var mappedResponse = _mapper.Map<List<GetMessageHistoryResponse>>(result);
            return mappedResponse;
        }
    }
}
