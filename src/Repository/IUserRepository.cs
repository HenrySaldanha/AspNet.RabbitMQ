using Domain;

namespace Repository;
public interface IUserRepository
{
    Task CreateUserAsync(User user);
    User Get(Guid id);
    bool HasUserByEmail(string email);
    bool HasUserByUsername(string username);
    Task UpdateEmailAsync(Guid id, string email);
}