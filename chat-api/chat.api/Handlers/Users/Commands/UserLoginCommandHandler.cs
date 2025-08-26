using AutoMapper;
using Chat.Api.Handlers.Users.Responses;
using Chat.Data.Models;
using Chat.Data.Repositories.UserRepository;
using MediatR;

namespace Chat.Api.Handlers.Users.Commands
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, UserLoginResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public UserLoginCommandHandler(IMapper mapper, IUserRepository userRepository) {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<UserLoginResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var userLoginEntity = _mapper.Map<UserLoginModel>(request);
            var result = await _userRepository.loginUser(userLoginEntity);
            return new UserLoginResponse
            {
                IsSuccess = result.IsSuccess,
                ValidationMessage = result.ValidationMessage,
                ErrorMessage = result.ErrorMessage,
                Email = result.Email,
                Token = result.Token,
                Message = result.Message,
                Username = result.Username
            };
        }
    }
}
