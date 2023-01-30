using MassTransit;

namespace POC.Configuration.Extensions.Helpers
{
    /// <summary>
    /// This is trying to bind consumers to the event counter part.
    /// So a consumer of type IConsumer<Foo.Bar.Baz> should be consuming
    /// from the Foo.Bar.Baz exchange.
    /// </summary>
    /// <typeparam name="TConsumer"></typeparam>
    public class ConfigureRabbitMqConsumer<TConsumer>
        where TConsumer : class, IConsumer
    {
        public static void BindEndpoint(IRabbitMqBusFactoryConfigurator configurator, IBusRegistrationContext context)
        {
            string bindPoint = GetBindPoint();
            configurator.ReceiveEndpoint(typeof(TConsumer).FullName, e =>
            {
                e.ConfigureConsumeTopology = false;
                e.Bind(bindPoint);
                e.ConfigureConsumer<TConsumer>(context);
            });
        }

        /// <summary>
        /// Fetch the event type from the consumer.
        /// </summary>
        /// <returns></returns>
        private static string GetBindPoint()
        {
            Type iConsumerType = typeof(TConsumer).GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>));
            Type messageType = iConsumerType.GetGenericArguments()[0];
            return messageType.FullName;
        }

    }
}
