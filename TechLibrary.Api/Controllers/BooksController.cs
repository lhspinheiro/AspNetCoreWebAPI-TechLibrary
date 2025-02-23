using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Books.Filter;
using TechLibrary.Api.UseCases.Books.Register;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredBookJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof (ResponseErrorMessagesJson), StatusCodes.Status400BadRequest )]

        public async Task <IActionResult> Register(RequestRegisterBooksJson request)
        {
            var useCase = new RegisterBooksUseCase();
            
            var response = await useCase.Execute(request);
            
            return Created(string.Empty, response);

        }
     
        [HttpGet("Filter")]
        [ProducesResponseType(typeof(ResponseBooksJson), StatusCodes.Status200OK)]
        public  IActionResult Filter(int pageNumber, string? title)
        {
            var useCase = new FilterBookUseCase();

            var request = new RequestFilterBooksJson
            {
                PageNumber = pageNumber,
                Title = title
            };
            
            var result = useCase.Execute( request );
            return Ok(result);
        }
    }
}
