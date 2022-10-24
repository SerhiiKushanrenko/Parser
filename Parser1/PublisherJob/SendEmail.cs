using MassTransit;
using Quartz;

namespace Parser.Publisher
{
    public class SendEmail : IJob
    {
        private readonly IPublishEndpoint publisher;
        public SendEmail(IPublishEndpoint publicher)
        {
            publisher = publicher;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Message message = new()
            {
                Text = "Super Puper"
            };

            await publisher.Publish(message);
        }
    }
}
