using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure;

namespace TechLibrary.Api.Services.LoggedUser;

public class LoggedUserService
{
    private readonly HttpContext _httpContext;

    public LoggedUserService(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }

    public User User()
    {
       var authentication = _httpContext.Request.Headers.Authorization.ToString();
        var token = authentication["Bearer ".Length..].Trim(); // pega o token

        var tokenHandler = new JwtSecurityTokenHandler(); //cria o token
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token); //le o token

        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value; //vai na claim e pega a prmeira claim do tipo Sub e pega o valor que no caso é o user id

        var userId = Guid.Parse(identifier);

        var dbContext = new TechLIbraryDbContext();

        return dbContext.Users.First(user => user.Id == userId);
    }


}
