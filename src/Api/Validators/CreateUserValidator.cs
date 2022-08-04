using Api.Models.Request;
using FluentValidation;

namespace Api.Validators;
public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(request => request.Email).NotNull().Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        RuleFor(request => request.PhoneNumber).NotNull().Matches(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$");
    }
}