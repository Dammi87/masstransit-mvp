

namespace POC.Services
{
    using Microsoft.Extensions.Logging;
    using POC.Services.Publishers;

    public class TimedExecuter : BackgroundService
    {
        private readonly ILogger<TimedExecuter> logger;
        private PublishDeleteWhat pDeleteWhat;
        private PublishSomethingCreated pSomethingCreated;
        private PublishWhatDeleted pWhatDeleted;
        private double orderInterval = 5.0;

        public TimedExecuter(IServiceScopeFactory factory, ILogger<TimedExecuter> logger)
        {
            this.pDeleteWhat = factory.CreateScope().ServiceProvider.GetRequiredService<PublishDeleteWhat>();
            this.pSomethingCreated = factory.CreateScope().ServiceProvider.GetRequiredService<PublishSomethingCreated>();
            this.pWhatDeleted = factory.CreateScope().ServiceProvider.GetRequiredService<PublishWhatDeleted>();
            this.logger = logger;
            
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(orderInterval));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await this.pDeleteWhat.CreateRandomOrder();
                await this.pSomethingCreated.CreateRandomOrder();
                await this.pWhatDeleted.CreateRandomOrder();
            }
        }
    }
}
