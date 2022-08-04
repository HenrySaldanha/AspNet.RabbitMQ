using Domain.Events;
using MassTransit;

namespace Worker;
public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    public Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var id = context.Message.Id;
        var name = context.Message.Username;
        var createdAt = context.Message.CreatedAt;

        Console.WriteLine($"New registered user: {id}, {name}, {createdAt}");
        return Task.CompletedTask;
    }
}