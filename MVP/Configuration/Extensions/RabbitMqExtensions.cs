
namespace POC.Configuration.Extensions
{
    using MassTransit;
    using System.Reflection;
    using System;
    using POC.Configuration.Extensions.Helpers;

    public static class RabbitMqExtensions
    {

        /// <summary>
        /// Helper method, invokes the ApplySendConfiguration and ApplyPublishConfiguration
        /// for the given type.
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="type"></param>
        private static void SetMessageConfiguration(IRabbitMqBusFactoryConfigurator configurator, Type type)
        {
            var sendConfigHelper = typeof(ConfigureRabbitMqMessages<>).MakeGenericType(type);
            var instance = Activator.CreateInstance(sendConfigHelper);
            var sendConfiguration = sendConfigHelper.GetMethod("ApplySendConfiguration");
            var publishConfiguration = sendConfigHelper.GetMethod("ApplyPublishConfiguration");
            sendConfiguration.Invoke(instance, new object[] { configurator });
            publishConfiguration.Invoke(instance, new object[] { configurator });
        }

        // Loop through all messages and apply the configurations
        public static void ApplyMessageConfiguration(this IRabbitMqBusFactoryConfigurator configurator, string namespacePrefix = null)
        {
            namespacePrefix = namespacePrefix ?? "POC.Events";
            // This configures all messages to be sent to an exchange name that is resolved by the ParentNameFormatter
            configurator.MessageTopology.SetEntityNameFormatter(new ParentNameFormatter());

            foreach (Type type in GetAllMessageTypes(namespacePrefix))
            {
                // Apply Send and Publish configurations for this type
                SetMessageConfiguration(configurator, type);

            }
        }

        /// <summary>
        /// Bind all consumers to the hopefully correct endpoint.
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="context"></param>
        public static void BindConsumers(this IRabbitMqBusFactoryConfigurator configurator, IBusRegistrationContext context)
        {
            foreach (Type type in GetConsumers())
            {
                // Apply Send and Publish configurations for this type
                var sendConfigHelper = typeof(ConfigureRabbitMqConsumer<>).MakeGenericType(type);
                var instance = Activator.CreateInstance(sendConfigHelper);
                var sendConfiguration = sendConfigHelper.GetMethod("BindEndpoint");
                sendConfiguration.Invoke(instance, new object[] { configurator, context });
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

       private static List<Type> GetAllMessageTypes(string namespacePrefix = null)
        {
            // Fetch assembly type
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            
            // Find all classes within this namespace
            return types.Where(t => t.IsClass && t.Namespace != null && t.Namespace.StartsWith(namespacePrefix)).ToList();
        }


    }
}
