using AutoMapper;
using Chat.Data.Entity;
using Chat.Data.Models;

namespace Chat.Data.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserCreationModel, UserCreationEntity>().
                ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));
        }
    }
}
