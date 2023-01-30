namespace POC.Services.Publishers
{
    using MassTransit;
    using POC.Events.DomainA.Events;

    public class PublishSomethingCreated
	{
        private ILogger<PublishSomethingCreated> logger; 
        private IPublishEndpoint publishEndpoint;
        private int orderCount = 0;


        public PublishSomethingCreated(IPublishEndpoint publishEndpoint, ILogger<PublishSomethingCreated> logger)
        {
            this.publishEndpoint = publishEndpoint;
            this.logger = logger;
        }

        public async Task CreateRandomOrder()
        {
            int id = Interlocked.Increment(ref orderCount);
            logger.LogInformation("Publishing event.");
            await publishEndpoint.Publish<SomethingCreated>(new() { Id = id});

        }

    }
}
