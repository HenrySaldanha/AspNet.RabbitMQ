using Application.IServices;
using Domain;
using Domain.Events;
using MassTransit;
using Repository;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IPublishEndpoint _publisher;

    public UserService(IUserRepository repo, IPublishEndpoint publisher)
    {
        _repo = repo;
        _publisher = publisher;
    }

    public async Task<User> CreateAsync(User user)
    {
        // TODO: Add log 
        try
        {
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            await _repo.CreateUserAsync(user);

            await _publisher.Publish<UserCreatedEvent>(new
            {
                user.Id,
                user.Username,
                user.PhoneNumber,
                user.CreatedAt,
                user.Email

            });
            return user;
        }
        catch (Exception e)
        {
            // TODO: Add log 
            throw;
        }
    }

    public Task UpdateEmailAsync(Guid id, string email)
    {
        // TODO: Add log 
        // TODO: implement
        throw new NotImplementedException();
    }

    public User Get(Guid id)
    {
        // TODO: Add log 
        return _repo.Get(id);
    }
}
