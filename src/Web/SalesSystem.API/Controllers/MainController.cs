using Microsoft.AspNetCore.Mvc;
using SalesSystem.SharedKernel.Responses;
using System.IdentityModel.Tokens.Jwt;

namespace SalesSystem.API.Controllers
{
    [ApiController]
    public abstract class MainController(IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        protected IResult CustomResponse<T>(Response<T> response) => response.Code switch
        {
            200 => TypedResults.Ok(response),
            400 => TypedResults.BadRequest(response),
            201 => TypedResults.Created(string.Empty, response),
            204 => TypedResults.NoContent(),
            _ => TypedResults.NotFound()
        };

        protected Guid GetUserId()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return Guid.Empty;

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (Guid.TryParse(userIdClaim, out var userId)) return userId;

            return Guid.Empty;
        }

        private HttpContext GetHttpContext() => httpContextAccessor.HttpContext!;

        private string GetToken()
        {
            var authorizationHeader = GetHttpContext().Request.Headers.Authorization.ToString();

            if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return authorizationHeader["Bearer ".Length..].Trim();

            return string.Empty;
        }

        protected string GetUserEmail()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return string.Empty;

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? string.Empty;
        }
    }
}