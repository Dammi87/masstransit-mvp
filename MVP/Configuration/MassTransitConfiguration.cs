using MassTransit;
using POC.Configuration.Extensions;
using POC.Configuration.Extensions.Helpers;

namespace POC.Configuration
{
    public static class MassTransitConfiguration
    {
        /// <summary>
        /// Add the massTransit related services.
        /// </summary>
        /// <param name="services">Service collection being built.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <returns>The service collection has all the services needed to use MassTransit.</returns>
        public static IServiceCollection AddMassTransitIntegration(this IServiceCollection services, IConfiguration configuration)
        {

            Console.WriteLine("Configuring MassTransit...");
            Console.WriteLine($"RabbitMQ Host: {configuration["RabbitMQ:Host"]}");
            Console.WriteLine($"RabbitMQ VirtualHost: {configuration["RabbitMQ:VirtualHost"]}");
            Console.WriteLine($"RabbitMQ Username: {configuration["RabbitMQ:Username"]}");
            Console.WriteLine($"RabbitMQ Password: {configuration["RabbitMQ:Password"]}");
            Console.WriteLine("Waiting for RabbitMQ to initialize...");
            //Thread.Sleep(7000);
            services.AddMassTransit(x =>
            {
                // Try to bind all consumers to the event type exchange
                x.ConfigureCustomConsumers();
                x.UsingRabbitMq((context, cfg) =>
                {
                    // Configure location of RabbitMQ
                    cfg.Host(configuration["RabbitMQ:Host"], configuration["RabbitMQ:VirtualHost"], h => {
                        h.Username(configuration["RabbitMQ:Username"]);
                        h.Password(configuration["RabbitMQ:Password"]);
                    });

                    // Apply the custom publish and send logic
                    cfg.ApplyMessageConfiguration();
                    cfg.BindConsumers(context);
                });

            });

            // Set options to wait for bus
            services.AddOptions<MassTransitHostOptions>()
                .Configure(options =>
                {
                    // if specified, waits until the bus is started before
                    // returning from IHostedService.StartAsync
                    // default is false
                    options.WaitUntilStarted = true;

                    // if specified, limits the wait time when starting the bus
                    options.StartTimeout = TimeSpan.FromSeconds(10);

                    // if specified, limits the wait time when stopping the bus
                    options.StopTimeout = TimeSpan.FromSeconds(30);
                });

            Console.WriteLine("Done configuring MassTransit!");
            return services;
        }
    }
}
