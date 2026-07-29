namespace BPM.Web.API.RabbitMQ.Publisher
{
    public interface IRabbitMQPublisher
    {
        //void Publish(string message);
        Task PublishMessageAsync<T>(T message,string entityName);
    }
}
