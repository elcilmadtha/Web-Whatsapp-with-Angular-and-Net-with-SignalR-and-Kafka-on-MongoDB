namespace Chat.Api.Handlers.Users.Responses
{
    public class CreateUserResponse
    {
        public bool IsSuccess { get; set; } = false;
        public string? Message { get; set; }
        public List<string> ErrorMessage { get; set; }
        public List<string> ValidationMessage { get; set; }
    }
}
