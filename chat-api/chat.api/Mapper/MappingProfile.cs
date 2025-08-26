using AutoMapper;
using Chat.Api.Handlers.Users.Commands;
using Chat.Api.Handlers.Users.Queries;
using Chat.Api.Handlers.Users.Responses;
using Chat.Data.Entity;
using Chat.Data.Models;
using Chat.Data.RequestModels;
using Chat.Data.ResponseModel;

namespace Chat.Api.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserCommand, UserCreationEntity>().
                ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));
            CreateMap<UserLoginCommand, UserLoginModel>();
            CreateMap<GetUsersRepoResponse, GetUsersResponse>();
            CreateMap<Users, UsersDto>();
            CreateMap<GetUsersRepoResponse, GetUsersResponse>();
            CreateMap<GetMessageHistoryQuery, GetMessageHistoryRequestModel>();
            CreateMap<GetMessageHistoryRepoResponse, GetMessageHistoryResponse>();
            CreateMap<GetRecentChatUsersQuery, GetRecentChatUsersRepoRequest>();
        }
    }
}
