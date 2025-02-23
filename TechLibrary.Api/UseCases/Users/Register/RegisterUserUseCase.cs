using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure;
using TechLibrary.Api.Infraestructure.Security.Cryptography;
using TechLibrary.Api.Infraestructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;


namespace TechLibrary.Api.UseCases.Users.Register;

public class RegisterUserUseCase
{
    public async Task <ResponseRegisteredUserJson> Execute(RequestUsersJson request)
    {

        var dbcontext = new TechLIbraryDbContext();
        
        await Validate(request, dbcontext);

        var cryptography = new BCryptAlgorithm();
        

        var entity = new User()
        {
            Email = request.Email,
            Name = request.Name,
            Password =  cryptography.HashPassword(request.Password),
        };
        
        await dbcontext.Users.AddAsync(entity);
        await dbcontext.SaveChangesAsync();

        var tokenGenarator = new JwtTokenGenerator();
        
        return new ResponseRegisteredUserJson
        {
            Name = entity.Name,
            AcessToken = tokenGenarator.Generate(entity)
        };
    }
    
    private async Task Validate(RequestUsersJson request, TechLIbraryDbContext dbContext)
    {
        var validator = new RegisterUserValidator();
        var result = validator.Validate(request);

        var existUserEmail = await dbContext.Users.AnyAsync(user => user.Email.Equals(request.Email));
        
        if (existUserEmail)
            result.Errors.Add(new ValidationFailure("Email", "Email is already in use."));
            
        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }

        
    }
}