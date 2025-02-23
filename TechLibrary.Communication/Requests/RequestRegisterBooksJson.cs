namespace TechLibrary.Communication.Requests;

public class RequestRegisterBooksJson 
{
    public string Title { get; set; } = string.Empty;
    
    public string Author { get; set; } = string.Empty;
    
    public int Amount { get; set; }
}