The focus of this project was the implementation of some **MassTransit** functions with **RabbitMQ**, in addition to this explanation below you can consult the project files to see the configuration and implementation of the other Api and Worker components.

## Docker use

To run RabbitMQ and SqlServer you can use Docker Compose with the command:

    docker-compose up

## Packages

You will find all api and worker dependencies in the "Dependencies" project. The packages for MassTransit and RabbitMQ are:

    MassTransit
    MassTransit.AspNetCore
    MassTransit.RabbitMq


## Startup Config

RabbitMQ connection string is in appsettings.json.

    public void ConfigureServices(IServiceCollection services)
    {
        ...
        services.AddMassTransit(bus =>
        {
            bus.UsingRabbitMq((ctx, busConfigurator) =>
            {
                busConfigurator.Host(Configuration.GetConnectionString("RabbitMq"));
            });
        }).AddMassTransitHostedService();
    } 
    
## Publish Event  

In my **UserService** class I implemented the publication of a **UserCreatedEvent** event.

    public async Task<User> CreateAsync(User user)
    {
        Log.Information("Service: {service} Method: {method} Request: {@request}",
            nameof(UserService), nameof(CreateAsync), @user);

        if (_repo.HasUserByEmail(user.Email) || _repo.HasUserByUsername(user.Username))
            return null;

        user.Id = Guid.NewGuid();
        user.CreatedAt = DateTime.UtcNow;

        try
        {
            await _repo.CreateUserAsync(user);

            await _publisher.Publish<UserCreatedEvent>(new
            {
                user.Id,
                user.Username,
                user.PhoneNumber,
                user.CreatedAt,
                user.Email
            });

            return user;
        }
        catch (Exception e)
        {
            Log.Error(e, "An exception occurred");
            throw;
        }
    }

    public interface UserCreatedEvent
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }

## Consumer

I created a very simple console app where I configure the actions for each event

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

## Give a Star 
If you found this Implementation helpful or used it in your Projects, do give it a star. Thanks!

## This project was built with
* [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [MassTransit](https://masstransit-project.com/)
* [FluentValidation](https://docs.fluentvalidation.net/en/latest/#)
* [SqlServer](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
* [Entity Framework](https://docs.microsoft.com/pt-br/ef/)
* [Swagger](https://swagger.io/)

## My contacts
* [LinkedIn](https://www.linkedin.com/in/henry-saldanha-3b930b98/)
