using System.Net;

namespace TechLibrary.Exception;

public class InvalidLoginException : TechLibraryException
{
    public override List<string> GetErrorMessages() => ["Email or password is incorrect."];
 

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}