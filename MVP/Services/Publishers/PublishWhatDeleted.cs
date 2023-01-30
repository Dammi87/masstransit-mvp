namespace POC.Services.Publishers
{
    using MassTransit;
    using POC.Events.DomainB.Events;

    public class PublishWhatDeleted
    {
        private ILogger<PublishWhatDeleted> logger; 
        private IPublishEndpoint publishEndpoint;
        private int orderCount = 0;


        public PublishWhatDeleted(IPublishEndpoint publishEndpoint, ILogger<PublishWhatDeleted> logger)
        {
            this.publishEndpoint = publishEndpoint;
            this.logger = logger;
        }

        public async Task CreateRandomOrder()
        {
            int id = Interlocked.Increment(ref orderCount);
            logger.LogInformation("Publishing event.");
            await publishEndpoint.Publish<WhatDeleted>(new() { Id=id });
        }

    }
}
