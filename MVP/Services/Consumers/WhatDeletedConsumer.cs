namespace POC.Services.Consumers
{
    using MassTransit;
    using POC.Events.DomainB.Events;

    public class WhatDeletedConsumer : IConsumer<WhatDeleted>
    {
        private ILogger<WhatDeletedConsumer> logger;

        public WhatDeletedConsumer(ILogger<WhatDeletedConsumer> logger)
        {
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<WhatDeleted> context)
        {
            this.logger.LogInformation($"What was deleted - ID: {context.Message.Id}.");
        }
    }
}