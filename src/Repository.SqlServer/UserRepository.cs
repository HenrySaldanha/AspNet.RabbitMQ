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

    public async Task UpdateEmailAsync(Guid id, string email)
    {
        var user = _context.Users.FirstOrDefault(c => c.Id == id);
        user.Email = email;
        _context.Users.Update(user);

        await _context.SaveChangesAsync();
    }

    public User Get(Guid id)
    {
        return _context.Users.FirstOrDefault(c => c.Id == id);
    }

    public bool HasUserByEmail(string email)
    {
        return _context.Users.Any(c => c.Email == email);
    }

    public bool HasUserByUsername(string username)
    {
        return _context.Users.Any(c => c.Username == username);
    }
}