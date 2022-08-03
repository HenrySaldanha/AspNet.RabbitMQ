using Domain;

namespace Repository;
public interface IUserRepository
{
    Task CreateUserAsync(User user);
    User Get(Guid id);
}