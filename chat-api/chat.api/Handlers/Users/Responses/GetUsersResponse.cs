namespace Chat.Api.Handlers.Users.Responses
{
    public class GetUsersResponse
    {
        public List<UsersDto> Users { get; set; }
        public bool IsSuccess { get; set; } = false;
        public List<string> ErrorMessage { get; set; }
        public List<string> ValidationMessage { get; set; }
    }

    public class UsersDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }   
        public string Username { get; set; }   
    }
}
