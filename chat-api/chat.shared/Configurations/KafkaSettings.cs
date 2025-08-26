namespace Chat.Shared.Configurations
{
    public class KafkaSettings
    {
        public string Bootstrap_Kafka_Local { get; set; }
        public string Bootstrap_Kafka_Server { get; set; }
        public Dictionary<string, TopicName> Topics { get; set; }
        public Dictionary<string, SubscriberData> Consumers { get; set; }
    }
    public class TopicName
    {
        public string topic { get; set; }
    }

    public class SubscriberData
    {
        public string consumer { get; set; }
        public string groupid { get; set; }
        public List<string> topic { get; set; }

    }
}
