namespace POC.Services.Consumers
{
    using MassTransit;
    using POC.Events.DomainB.Commands;

    public class DeleteWhatConsumer : IConsumer<DeleteWhat>
    {
        private ILogger<DeleteWhatConsumer> logger;

        public DeleteWhatConsumer(ILogger<DeleteWhatConsumer> logger)
        {
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<DeleteWhat> context)
        {
            this.logger.LogInformation($"Order was created  - ID: {context.Message.Id}.");
        }
    }
}