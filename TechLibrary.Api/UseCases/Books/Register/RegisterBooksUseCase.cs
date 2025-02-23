using FluentValidation.Results;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Books.Register;

public class RegisterBooksUseCase
{
    public ResponseRegisteredBookJson Execute(RequestRegisterBooksJson request)
    {
        var dbcontext = new TechLIbraryDbContext();

        Validate(request, dbcontext);

        var entity = new Book()
        {
            Title = request.Title,
            Author = request.Author,
            Amount = request.Amount,
        };
        
        dbcontext.Books.Add(entity);
        dbcontext.SaveChanges();


        return new ResponseRegisteredBookJson
        {
            Title = entity.Title,
            Author = entity.Author,
        };
    }


    private void Validate(RequestRegisterBooksJson request, TechLIbraryDbContext dbContext)
    {
        var validator = new RegisterBooksValidator();
        var results = validator.Validate(request);
         
        var existBooks = dbContext.Books.Any(book => book.Title.Equals(request.Title) && book.Author.Equals(request.Author));

        if (existBooks)
            results.Errors.Add((new ValidationFailure("Books", "The books is already taken.")));

        if (results.IsValid == false)
        {
            var errorMessages=results.Errors.Select(error => error.ErrorMessage).ToList();
        
            throw new ErrorOnValidationException(errorMessages);
            
        }
    }
}