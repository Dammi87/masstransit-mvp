using MassTransit;

namespace POC.Configuration.Extensions.Helpers
{
    /// <summary>
    /// Configuration class for consumers.
    /// I was hoping this would bind the consumer to the alreayd available
    /// event fanout exchange.
    /// </summary>
    /// <typeparam name="TConsumer"></typeparam>
    public class ConsumerConfiguration<TConsumer> : ConsumerDefinition<TConsumer>
        where TConsumer : class, IConsumer
    {
        public ConsumerConfiguration()
        {
            // override the default endpoint name
            Type iConsumerType = typeof(TConsumer).GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>));
            Type messageType = iConsumerType.GetGenericArguments()[0];
            EndpointName = messageType.FullName;
        }
    }
}
