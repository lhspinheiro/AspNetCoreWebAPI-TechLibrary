using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Checkouts;

public class RegisterBookCheckoutUseCase
{

    private const int MAX_LOAND_DAYS = 7;
    private readonly LoggedUserService _loggedUser;

    public RegisterBookCheckoutUseCase(LoggedUserService loggedUser)
    {
        _loggedUser = loggedUser;
    }

    public async Task<bool> Execute(Guid bookId)
    {
        var dbContext = new TechLIbraryDbContext();

        await Validate(dbContext, bookId);

        var user = _loggedUser.User();

        var entity = new Checkout
        {
            userID = user.Id,
            bookId = bookId,
            ExpectedReturnDate = DateTime.Now.AddDays(MAX_LOAND_DAYS),

        };
        
        if (entity == null)
            return false;

        dbContext.Checkouts.Add(entity);

        dbContext.SaveChanges();
        
        return true;
    }

    private async  Task <bool> Validate(TechLIbraryDbContext dbContext, Guid bookId)
    {
        var book = dbContext.Books.FirstOrDefault(book => book.Id == bookId);
        if (book is null)
            throw new NotFoundException("Livro não encontrado");

        var amountBookNotReturned = dbContext.Checkouts.Count(checkout => checkout.bookId == bookId && checkout.ReturnedDate == null);

        if (amountBookNotReturned == book.Amount)
            throw new ConflictException("Livro não está disponíve para empréstimo");
        
        return true;
    }
}
