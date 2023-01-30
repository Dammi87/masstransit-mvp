namespace POC.Services.Consumers
{
    using MassTransit;
    using POC.Events.DomainA.Events;

    public class SomethingCreatedConsumerB : IConsumer<SomethingCreated>
    {
        private ILogger<SomethingCreatedConsumerB> logger;

        public SomethingCreatedConsumerB(ILogger<SomethingCreatedConsumerB> logger)
        {
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<SomethingCreated> context)
        {
            this.logger.LogInformation($"Something was created (consumer b) - ID: {context.Message.Id}.");
        }
    }
}