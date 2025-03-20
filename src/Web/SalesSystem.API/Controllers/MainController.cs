using Microsoft.AspNetCore.Mvc;
using SalesSystem.SharedKernel.Responses;

namespace SalesSystem.API.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        protected IResult CustomResponse<T>(Response<T> response) => response.Code switch
        {
            200 => TypedResults.Ok(response),
            400 => TypedResults.BadRequest(response),
            201 => TypedResults.Created(string.Empty, response),
            204 => TypedResults.NoContent(),
            _ => TypedResults.NotFound()
        };
    }
}