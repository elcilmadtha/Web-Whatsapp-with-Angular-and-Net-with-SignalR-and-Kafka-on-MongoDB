using Chat.Data.Entity;
using Chat.Data.Models;
using Chat.Data.RequestModels;
using Chat.Data.ResponseModel;

namespace Chat.Data.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<UserCreationRepoResponse> createUser(UserCreationEntity userCreationEntity);
        Task<UserLoginRepoResponse> loginUser(UserLoginModel userModel);
        Task<GetUsersRepoResponse> GetUsers(string username);
    }
}
