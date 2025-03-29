using Microsoft.AspNetCore.Mvc;
using SalesSystem.Register.Application.Commands.Authentication.ForgetPassword;
using SalesSystem.Register.Application.Commands.Authentication.Register;
using SalesSystem.Register.Application.Commands.Authentication.ResetPassword;
using SalesSystem.Register.Application.Commands.Authentication.SignIn;
using SalesSystem.Register.Application.Commands.Customers.AddAddress;
using SalesSystem.SharedKernel.Communication.Mediator;

namespace SalesSystem.API.Controllers
{
    [Route("api/v1/register")]
    public class RegisterController(IHttpContextAccessor httpContextAccessor,
                                IMediatorHandler mediator)
                              : MainController(httpContextAccessor)
    {
        [HttpPost]
        public async Task<IResult> RegisterAsync(RegisterUserCommand command)
            => CustomResponse(await mediator.SendCommand(command));

        [HttpPost("login")]
        public async Task<IResult> SignInAsync(SignInUserCommand command)
            => CustomResponse(await mediator.SendCommand(command));

        [HttpPost("address")]
        public async Task<IResult> AddAddressAsync(AddAddressCommand command)
        {
            command.SetCustomerId(GetUserId());
            return CustomResponse(await mediator.SendCommand(command));
        }

        [HttpPost("forget-password")]
        public async Task<IResult> ForgetPasswordAsync(ForgetPasswordUserCommand command)
            => CustomResponse(await mediator.SendCommand(command));

        [HttpPut]
        public async Task<IResult> ResetPasswordAsync(ResetPasswordUserCommand command)
            => CustomResponse(await mediator.SendCommand(command));
    }
}