using Chat.Api.Models.MongoDB;
using Chat.Data.Entity;
using Chat.Data.Models;
using Chat.Data.ResponseModel;
using Chat.Data.Shared;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Chat.Data.Repositories.UserRepository
{
    public class UserRepository: IUserRepository
    {
        private readonly IMongoCollection<UserCreationEntity> _collection;
        private readonly JwtSettings _jwtSettings;

        public UserRepository(IOptions<MongoDbSettings> settings, JwtSettings jwtSettings) {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<UserCreationEntity>("Users");
            _jwtSettings = jwtSettings;
        }

        public async Task<UserCreationRepoResponse> createUser(UserCreationEntity userCreationEntity)
        {
            UserCreationRepoResponse userCreationResponse = new UserCreationRepoResponse();
            try
            {
                var existingUser = await _collection.Find(u => u.Username == userCreationEntity.Username || u.Email == userCreationEntity.Email).FirstOrDefaultAsync();
                if (existingUser != null)
                {
                    userCreationResponse.ValidationMessage.Add(Constants.UsersAlreadyExists);
                    return userCreationResponse;
                }
                await _collection.InsertOneAsync(userCreationEntity);
                userCreationResponse.IsSuccess = true;
                userCreationResponse.Message = Constants.UserCreatedSuccessfully;
            }
            catch (Exception ex) {
                userCreationResponse.IsSuccess = false;
                userCreationResponse.ErrorMessage.Add($"An error occurred: {ex.Message}");
            }
            return userCreationResponse;
        }

        public async Task<UserLoginRepoResponse> loginUser(UserLoginModel userModel)
        {
            UserLoginRepoResponse userLoginResponse = new UserLoginRepoResponse();
            try
            {
                var checkUser = await _collection.Find(x => x.Username == userModel.Username).FirstOrDefaultAsync();
                if (checkUser == null)
                {
                    userLoginResponse.ValidationMessage.Add(Constants.UserDoesNotExists);
                    return userLoginResponse;
                }
                bool userVerified = BCrypt.Net.BCrypt.Verify(userModel.Password, checkUser.PasswordHash);
                if (!userVerified)
                {
                    userLoginResponse.ValidationMessage.Add(Constants.WrongPassword);
                    return userLoginResponse;
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey); // Inject JwtSettings

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, checkUser.Id),
                    new Claim(ClaimTypes.Name, checkUser.Name),
                    new Claim(ClaimTypes.Email, checkUser.Email)
                }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                userLoginResponse.IsSuccess = true;
                userLoginResponse.Message = Constants.SuccessfulLogin;
                userLoginResponse.Token = jwtToken;
                userLoginResponse.Username = checkUser.Username;
                userLoginResponse.Email = checkUser.Email;
            }
            catch(Exception ex) {
                userLoginResponse.ErrorMessage.Add($"An error occurred: {ex.Message}");
            }
            return userLoginResponse;
        }
    
        public async Task<GetUsersRepoResponse> GetUsers(string username)
        {
            GetUsersRepoResponse getUsersRepoResponse = new GetUsersRepoResponse();
            try
            {
                var collectionResult = await _collection.Find(x => x.Username.ToLower().Contains(username.ToLower())).ToListAsync();
                getUsersRepoResponse.Users = collectionResult.Select(x => new Users
                { Id = x.Id, Name = x.Name, Username = x.Username, Email = x.Email }).ToList();
                getUsersRepoResponse.IsSuccess = true;
            }
            catch(Exception ex) {
                getUsersRepoResponse.IsSuccess = false;
                getUsersRepoResponse.ErrorMessage.Add($"An error occurred: {ex.Message}");
            }
            return getUsersRepoResponse;
        }
    }
}
