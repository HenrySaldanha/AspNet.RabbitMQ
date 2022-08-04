using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.SqlServer;
using Application.Services;
using Application.IServices;
using Api.Mappings;
using Serilog;
using FluentValidation.AspNetCore;
using System.Reflection;

namespace Api;
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllers()
            .AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<Startup>());

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        services.AddSingleton(config.CreateMapper());

        services.AddDbContext<UserContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("UserContext")));

        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUserService, UserService>();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "V1",
                Title = "RabbitMQ, MassTransit API",
                Contact = new OpenApiContact
                {
                    Name = "Henry Saldanha"
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        services.AddMassTransit(bus =>
        {
            bus.UsingRabbitMq((ctx, busConfigurator) =>
            {
                busConfigurator.Host(Configuration.GetConnectionString("RabbitMq"));
            });
        }).AddMassTransitHostedService();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseHttpsRedirection();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API");
        });

        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.ApplyDatabaseMigrations();
        app.UseSerilogRequestLogging();
    }
}