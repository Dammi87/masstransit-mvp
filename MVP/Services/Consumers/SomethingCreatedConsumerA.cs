
namespace POC.Services.Consumers
{
    using MassTransit;
    using POC.Events.DomainA.Events;

    public class SomethingCreatedConsumerA : IConsumer<SomethingCreated>
    {
        private ILogger<SomethingCreatedConsumerA> logger;

        public SomethingCreatedConsumerA(ILogger<SomethingCreatedConsumerA> logger)
        {
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<SomethingCreated> context)
        {
            this.logger.LogInformation($"SomethingCreated was created - ID: {context.Message.Id}.");
        }
    }
}
