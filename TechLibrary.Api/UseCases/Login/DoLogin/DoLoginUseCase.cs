using TechLibrary.Api.Infraestructure;
using TechLibrary.Api.Infraestructure.Security.Cryptography;
using TechLibrary.Api.Infraestructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Login.DoLogin;

public class DoLoginUseCase
{
    public ResponseRegisteredUserJson Execute(RequestLoginJson request)
    {
        var dbContext = new TechLIbraryDbContext();

        var entity = dbContext.Users.FirstOrDefault(user => user.Email.Equals(request.Email));
        if (entity is null)
            throw new InvalidLoginException();
        
        var cryptography = new BCryptAlgorithm(); 
        var passwordIsValid = cryptography.Verify(request.Password, entity);
        if (passwordIsValid == false)
            throw new InvalidLoginException();
        
        var tokenGenarator = new JwtTokenGenerator();
        return new ResponseRegisteredUserJson
        {
            Name = entity.Name,
            AcessToken = tokenGenarator.Generate(entity)
        };
    }
}