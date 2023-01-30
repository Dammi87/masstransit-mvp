namespace POC.Services.Publishers
{
    using MassTransit;
    using POC.Events.DomainB.Commands;

    public class PublishDeleteWhat
    {
        private ILogger<PublishDeleteWhat> logger; 
        private IPublishEndpoint publishEndpoint;
        private int orderCount = 0;


        public PublishDeleteWhat(IPublishEndpoint publishEndpoint, ILogger<PublishDeleteWhat> logger)
        {
            this.publishEndpoint = publishEndpoint;
            this.logger = logger;
        }

        public async Task CreateRandomOrder()
        {
            int id = Interlocked.Increment(ref orderCount);
            logger.LogInformation("Publishing event.");
            await publishEndpoint.Publish<DeleteWhat>(new() { Id=id });
        }

    }
}
