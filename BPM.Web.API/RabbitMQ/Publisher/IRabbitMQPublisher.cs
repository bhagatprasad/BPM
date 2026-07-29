namespace BPM.Web.API.RabbitMQ.Publisher
{
    public interface IRabbitMQPublisher
    {
        Task PublishMessageAsync<T>(T message, string entityName);
        Task PublishMessageWithRetryAsync<T>(T message, string entityName, int maxRetries = 3);
    }
}
