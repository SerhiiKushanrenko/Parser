using Consumer.TransactionalEmail;
using MassTransit;
using Parser.Publisher;

namespace Consumer.Consumer
{
    public class APIConsumer : IConsumer<Message>
    {
        public Task Consume(ConsumeContext<Message> context)
        {
            var result = context.Message;
            Console.WriteLine(result.Text);
            SendWithSendGrid.SendMessage(result).Wait();
            return Task.CompletedTask;
        }
    }
}
