namespace Chat.Data.ResponseModel
{
    public class GetUsersRepoResponse
    {
        public List<Users> Users { get; set; }
        public bool IsSuccess { get; set; } = false;
        public List<string> ErrorMessage { get; set; }
        public List<string> ValidationMessage { get; set; }
    }
    public class Users
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
