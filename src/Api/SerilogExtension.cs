using Serilog;
using Serilog.Events;

namespace Api;

public static class SerilogExtension
{
    public static WebApplicationBuilder AddSerilogApi(this WebApplicationBuilder webApp)
    {
        Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
          .Enrich.FromLogContext()
          .WriteTo.Console()
          .CreateLogger();

        return webApp;
    }
}
