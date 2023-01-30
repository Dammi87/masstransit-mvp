using MassTransit;
using System.Reflection;

namespace POC.Configuration.Extensions.Helpers
{

    /// <summary>
    /// Helper class, helps me avoid using to much reflection shenenigans.
    /// </summary>
    /// <typeparam name="TConsumer"></typeparam>
    internal class MarelConsumer<TConsumer> where TConsumer : class, IConsumer
    {
        public static void AddMarelConsumer(IBusRegistrationConfigurator configuration)
        {
            // Add the consumer with the provided configuration
            configuration.AddConsumer<TConsumer, ConsumerConfiguration<TConsumer>>();
        }

    }

    public static class MassTransitExtensions
    {
        /// <summary>
        /// Find all consumers and apply the consumer configuraiton to them.
        /// </summary>
        /// <param name="configuration"></param>
        public static void ConfigureCustomConsumers(this IBusRegistrationConfigurator configuration)
        {
            List<Type> consumers = GetConsumers();
            foreach (Type consumerType in consumers)
            {
                var sendConfigHelper = typeof(MarelConsumer<>).MakeGenericType(consumerType);
                var instance = Activator.CreateInstance(sendConfigHelper);
                var sendConfiguration = sendConfigHelper.GetMethod("AddMarelConsumer");
                sendConfiguration.Invoke(instance, new object[] { configuration });
            }

        }

        private static List<Type> GetConsumers()
        {
            // Fetch assembly tyðes
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();

            // Find all consumers defined in this project
            return types.Where(t => t.IsClass && typeof(IConsumer).IsAssignableFrom(t)).ToList();

        }
    }
}
