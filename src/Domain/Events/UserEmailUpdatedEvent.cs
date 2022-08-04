namespace Domain.Events;
public interface UserEmailUpdatedEvent
{
    public Guid Id { get; set; }
    public string Email { get; set; }
}