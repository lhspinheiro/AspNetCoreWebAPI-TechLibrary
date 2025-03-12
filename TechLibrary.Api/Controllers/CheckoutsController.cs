using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Api.UseCases.Checkouts;

namespace TechLibrary.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CheckoutsController : ControllerBase
{

    [HttpPost]
    [Route("{bookid}")]
    
    public async Task <IActionResult> BookCheckout(Guid bookid)
    {

        var loggedUser = new LoggedUserService(HttpContext); 

        var useCase = new RegisterBookCheckoutUseCase(loggedUser);

         await useCase.Execute(bookid);

        return NoContent();
    }
}
