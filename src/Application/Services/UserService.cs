using Application.IServices;
using Domain;
using Domain.Events;
using MassTransit;
using Repository;
using Serilog;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly ILogger _logger;
    private readonly IUserRepository _repo;
    private readonly IPublishEndpoint _publisher;

    public UserService(IUserRepository repo, IPublishEndpoint publisher, ILogger logger)
    {
        _repo = repo;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<User> CreateAsync(User user)
    {
        _logger.Information("Service: {service} Method: {method} Request: {@request}",
            nameof(UserService), nameof(CreateAsync), @user);

        if (_repo.HasUserByEmail(user.Email) || _repo.HasUserByUsername(user.Username))
            return null;

        user.Id = Guid.NewGuid();
        user.CreatedAt = DateTime.UtcNow;

        try
        {
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
            _logger.Error(e, "An exception occurred");
            throw;
        }
    }

    public async Task<bool> UpdateEmailAsync(Guid id, string email)
    {
        _logger.Information("Service: {service} Method: {method} Request: {@request}",
            nameof(UserService), nameof(UpdateEmailAsync), new { id, email });

        if (_repo.Get(id) is null || _repo.HasUserByEmail(email))
            return false;

        try
        {
            await _repo.UpdateEmailAsync(id, email);

            await _publisher.Publish<UserEmailUpdatedEvent>(new
            {
                id,
                email
            });
        }
        catch (Exception e)
        {
            _logger.Error(e, "An exception occurred");
            throw;
        }

        return true;
    }

    public User Get(Guid id)
    {
        _logger.Information("Service: {service} Method: {method} Request: {@request}",
            nameof(UserService), nameof(Get), new { id });

        return _repo.Get(id);
    }
}