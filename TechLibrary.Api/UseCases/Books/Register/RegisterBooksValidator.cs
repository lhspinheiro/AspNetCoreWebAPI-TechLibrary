using FluentValidation;
using TechLibrary.Communication.Requests;

namespace TechLibrary.Api.UseCases.Books.Register;

public class RegisterBooksValidator : AbstractValidator<RequestRegisterBooksJson>
{
    public RegisterBooksValidator()
    {
        RuleFor( request => request.Title).NotEmpty().WithMessage("Title is required");
        RuleFor( request => request.Author).NotEmpty().WithMessage("Title is required");
        RuleFor( request => request.Amount).NotEmpty().GreaterThan(0).WithMessage("Amount is required and must be greater than 0");
        
    }
}