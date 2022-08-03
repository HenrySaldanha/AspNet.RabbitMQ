using Domain;

namespace Repository.SqlServer;

public class UserRepository : IUserRepository
{
    private readonly UserContext _context;

    public UserRepository(UserContext context)
    {
        _context = context;
    }

    public async Task CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public User Get(Guid id)
    {
        return _context.Users.FirstOrDefault(c=>c.Id == id);
    }
}
