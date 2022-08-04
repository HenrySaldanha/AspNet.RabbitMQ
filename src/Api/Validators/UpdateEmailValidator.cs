using Api.Models.Request;
using FluentValidation;

namespace Api.Validators;
public class UpdateEmailValidator : AbstractValidator<UpdateEmailRequest>
{
    public UpdateEmailValidator()
    {
        RuleFor(request => request.Email).NotNull().Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
    }
}