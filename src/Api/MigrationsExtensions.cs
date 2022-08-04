using Microsoft.EntityFrameworkCore;
using Repository.SqlServer;

namespace Api;
public static class MigrationsExtensions
{
    public static void ApplyDatabaseMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        scope.ServiceProvider.GetService<UserContext>().Database.Migrate();
    }
}