using AutoMapper;
using Chat.Api.Handlers.Users.Responses;
using Chat.Data.Entity;
using Chat.Data.Repositories.UserRepository;
using MediatR;

namespace Chat.Api.Handlers.Users.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository) {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userEntity = _mapper.Map<UserCreationEntity>(request);
            var result = await _userRepository.createUser(userEntity);
            CreateUserResponse createUserResponse = new CreateUserResponse
            {
                IsSuccess = result.IsSuccess,
                ValidationMessage = result.ValidationMessage,
                ErrorMessage = result.ErrorMessage,
                Message = result.Message 
            };
            return createUserResponse;
        }
    }
}
