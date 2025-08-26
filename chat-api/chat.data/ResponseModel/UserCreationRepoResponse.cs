namespace Chat.Data.ResponseModel
{
    public class UserCreationRepoResponse
    {
        public bool IsSuccess { get; set; } = false;
        public string? Message { get; set; }
        public List<string> ErrorMessage { get; set; } = new();
        public List<string> ValidationMessage { get; set; } = new();
    }
}
