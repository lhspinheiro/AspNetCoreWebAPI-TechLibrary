using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Books.Register;

public class RegisterBooksUseCase
{
    public async  Task <ResponseRegisteredBookJson> Execute(RequestRegisterBooksJson request)
    {
        var dbcontext = new TechLIbraryDbContext();

        await Validate(request, dbcontext);

        var entity = new Book()
        {
            Title = request.Title,
            Author = request.Author,
            Amount = request.Amount,
        };
        
        await dbcontext.Books.AddAsync(entity);
        await dbcontext.SaveChangesAsync();


        return new ResponseRegisteredBookJson
        {
            Title = entity.Title,
            Author = entity.Author,
        };
    }


    private async Task Validate(RequestRegisterBooksJson request, TechLIbraryDbContext dbContext)
    {
        var validator = new RegisterBooksValidator();
        var results = validator.Validate(request);
         
        var existBooks = await dbContext.Books.AnyAsync(book => book.Title.Equals(request.Title) && book.Author.Equals(request.Author));

        if (existBooks)
             results.Errors.Add((new ValidationFailure("Books", "The books is already taken.")));

        if (results.IsValid == false)
        {
            var errorMessages= results.Errors.Select(error => error.ErrorMessage).ToList();
        
            throw new ErrorOnValidationException(errorMessages);
            
        }
    }
}