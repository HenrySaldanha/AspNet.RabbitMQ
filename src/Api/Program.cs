using Api;

var builder = WebApplication.CreateBuilder(args)
    .AddSerilogApi()
    .UseStartup<Startup>();
