using System.Net;

namespace TechLibrary.Exception;

public class InvalidLoginException : TechLibraryException
{

    public InvalidLoginException() : base("Email or password is incorrect.")
    {   
    }

    public override List<string> GetErrorMessages() => [Message];
 

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}