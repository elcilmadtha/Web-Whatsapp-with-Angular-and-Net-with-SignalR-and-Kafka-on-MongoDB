namespace Chat.Shared.Publisher
{
    public interface IKafkaPublisher
    {
        Task PublishAsync<T>(string topicKey, T message);
    }
}
