using FluentValidation.Results;
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
    public ResponseRegisteredUserJson Execute(RequestUsersJson request)
    {

        var dbcontext = new TechLIbraryDbContext();
        
        Validate(request, dbcontext);

        var cryptography = new BCryptAlgorithm();
        

        var entity = new User()
        {
            Email = request.Email,
            Name = request.Name,
            Password =  cryptography.HashPassword(request.Password),
        };
        
        dbcontext.Users.Add(entity);
        dbcontext.SaveChanges();

        var tokenGenarator = new JwtTokenGenerator();
        
        return new ResponseRegisteredUserJson
        {
            Name = entity.Name,
            AcessToken = tokenGenarator.Generate(entity)
        };
    }
    
    private void Validate(RequestUsersJson request, TechLIbraryDbContext dbContext)
    {
        var validator = new RegisterUserValidator();
        var result = validator.Validate(request);

        var existUserEmail = dbContext.Users.Any(user => user.Email.Equals(request.Email));
        
        if (existUserEmail)
            result.Errors.Add(new ValidationFailure("Email", "Email is already in use."));
            
        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }

        
    }
}