using AutoMapper;
using Chat.Api.Handlers.Users.Responses;
using Chat.Data.Repositories.UserRepository;
using MediatR;

namespace Chat.Api.Handlers.Users.Queries
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetUsersResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public GetUsersQueryHandler(IMapper mapper, IUserRepository userRepository) {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<GetUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetUsers(request.UserName);
            var mappedData = _mapper.Map<GetUsersResponse>(result);
            return mappedData;
        }
    }
}
