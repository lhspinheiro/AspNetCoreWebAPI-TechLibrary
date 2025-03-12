namespace TechLibrary.Api.Domain.Entities;

public class Checkout
{
    public Guid Id { get; set; }
    public DateTime ChechouDate { get; set; } = DateTime.UtcNow;
    public Guid userID { get; set; }
    public Guid bookId { get; set; }
    public DateTime ExpectedReturnDate {  get; set; } 
    public DateTime? ReturnedDate { get; set; }

}
