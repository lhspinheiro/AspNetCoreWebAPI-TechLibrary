using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TechLibrary.Api.Domain.Entities;

namespace TechLibrary.Api.Infraestructure.Security.Tokens.Access;

public class JwtTokenGenerator
{
    public string Generate(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(60), //Quando o token vai expirar
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature)
        };
 
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(securityToken);

    }

    private static SymmetricSecurityKey SecurityKey()
    {
        var signKey = "fbAspGUD9rAE9jHsdP9uLXg14nxjX6Ke";

        var symmetricKey = Encoding.UTF8.GetBytes(signKey);
            
        return new SymmetricSecurityKey(symmetricKey);
    }
}