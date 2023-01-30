using MassTransit;


namespace POC.Configuration.Extensions.Helpers
{
    /// <summary>
    /// Configures SEND and PUBLISH for the class T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigureRabbitMqMessages<T> where T : class
    {
        /// <summary>
        /// Apply routing to key to the event
        /// </summary>
        /// <param name="configurator"></param>
        public void ApplySendConfiguration(IRabbitMqBusFactoryConfigurator configurator)
        {
            configurator.Send<T>(
                x =>
                {
                    x.UseRoutingKeyFormatter(context => typeof(T).FullName);
                }
            );
        }
        public static string GetDomain(Type obj)
        {
            Console.WriteLine($"[ParentNameFormatter]: {obj}");
            return obj.FullName.Replace("POC.Events.", "").Split(".").First();
        }

        /// <summary>
        /// Configure the class to publish its message to its domain.
        /// </summary>
        /// <param name="configurator"></param>
        public void ApplyPublishConfiguration(IRabbitMqBusFactoryConfigurator configurator)
        {
            Type messageType = typeof(T);
            string domainName = GetDomain(messageType);
            Console.WriteLine($"[ApplyPublishConfiguration] {messageType.FullName} -> {domainName}");
            configurator.ReceiveEndpoint(
                messageType.FullName,
                e =>
                {
                    e.Lazy = true;
                    e.Bind(domainName, x =>
                    {
                        x.RoutingKey = messageType.FullName;
                        x.ExchangeType = "topic";
                        x.Durable = true;
                    });
                }
            );

            configurator.Publish<T>(
                x =>
                {
                    x.ExchangeType = "topic";
                }
            );
        }
    }
}
