namespace Chat.Api.Models.MongoDB
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string MessagesCollection { get; set; }
        public string UsersCollection { get; set; }
    }
}
