using Domain.Events;
using MassTransit;

namespace Worker;
public class UserEmailUpdatedConsumer : IConsumer<UserEmailUpdatedEvent>
{
    public Task Consume(ConsumeContext<UserEmailUpdatedEvent> context)
    {
        var id = context.Message.Id;
        var email = context.Message.Email;

        Console.WriteLine($"User email changed: {id}, {email}");
        return Task.CompletedTask;
    }
}