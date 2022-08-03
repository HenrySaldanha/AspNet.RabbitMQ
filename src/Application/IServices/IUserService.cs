using Domain;
namespace Application.IServices;

public interface IUserService
{
    public Task<User> CreateAsync(User request);
    public Task UpdateEmailAsync(Guid id, string email);
    public User Get(Guid id);
}
