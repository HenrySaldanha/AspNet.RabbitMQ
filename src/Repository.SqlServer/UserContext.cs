using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository.SqlServer;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
}