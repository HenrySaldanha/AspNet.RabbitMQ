using MassTransit;
using Worker;


var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.ReceiveEndpoint("user-created", e =>
    {
        e.Consumer<UserCreatedConsumer>();
        e.PrefetchCount = 10;
    });

    cfg.ReceiveEndpoint("user-email-updated", e =>
    {
        e.Consumer<UserEmailUpdatedConsumer>();
        e.PrefetchCount = 10;
    });
});
busControl.Start();

Console.WriteLine("Running...");

while (true);