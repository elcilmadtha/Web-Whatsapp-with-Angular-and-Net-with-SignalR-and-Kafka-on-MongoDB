namespace Chat.Api.Handlers.Users.Responses
{
    public class UserLoginResponse
    {
        public bool IsSuccess { get; set; } = false;
        public string? Message { get; set; }
        public List<string> ErrorMessage { get; set; }
        public List<string> ValidationMessage { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
