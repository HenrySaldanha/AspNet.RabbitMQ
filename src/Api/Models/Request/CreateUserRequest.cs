namespace Api.Models.Request;

public class CreateUserRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
