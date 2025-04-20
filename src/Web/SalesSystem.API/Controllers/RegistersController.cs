using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Registers.Application.Commands.Authentication.AddUserRole;
using SalesSystem.Registers.Application.Commands.Authentication.CreateRole;
using SalesSystem.Registers.Application.Commands.Authentication.Delete;
using SalesSystem.Registers.Application.Commands.Authentication.ForgetPassword;
using SalesSystem.Registers.Application.Commands.Authentication.ResetPassword;
using SalesSystem.Registers.Application.Commands.Authentication.SignIn;
using SalesSystem.Registers.Application.Commands.Authentication.SignUp;
using SalesSystem.Registers.Application.Commands.Customers.AddAddress;
using SalesSystem.Registers.Application.Queries.Customers.GetById;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.Enums;

namespace SalesSystem.API.Controllers
{
    [Route("api/v1/registers")]
    public class RegistersController(IHttpContextAccessor httpContextAccessor,
                                IMediatorHandler mediatorHandler)
                              : MainController(httpContextAccessor)
    {
        [HttpGet]
        public async Task<IResult> GetByIdAsync()
            => CustomResponse(await mediatorHandler.SendQuery(new GetCustomerByIdQuery(GetUserId())));

        [HttpPost]
        public async Task<IResult> SignUpAsync(SignUpUserCommand command)
            => CustomResponse(await mediatorHandler.SendCommand(command));

        [HttpPost("signin")]
        public async Task<IResult> SignInAsync(SignInUserCommand command)
            => CustomResponse(await mediatorHandler.SendCommand(command));

        [HttpDelete("{userId:guid}")]
        public async Task<IResult> DeleteAsync(Guid userId)
             => CustomResponse(await mediatorHandler.SendCommand(new DeleteUserCommand(userId)));

        [HttpPost("address")]
        public async Task<IResult> AddAddressAsync(AddAddressCommand command)
        {
            command.SetCustomerId(GetUserId());
            return CustomResponse(await mediatorHandler.SendCommand(command));
        }

        [HttpPost("forget-password")]
        public async Task<IResult> ForgetPasswordAsync(ForgetPasswordUserCommand command)
            => CustomResponse(await mediatorHandler.SendCommand(command));

        [HttpPut]
        public async Task<IResult> ResetPasswordAsync(ResetPasswordUserCommand command)
            => CustomResponse(await mediatorHandler.SendCommand(command));

        [Authorize(Roles = nameof(EUserRoles.Admin))]
        [HttpPost("roles")]
        public async Task<IResult> CreateRoleAsync(CreateRoleCommand command)
            => CustomResponse(await mediatorHandler.SendCommand(command));

        [Authorize(Roles = nameof(EUserRoles.Admin))]
        [HttpPost("roles/user-role")]
        public async Task<IResult> AddRoleToUserAsync(AddUserRoleCommand command)
            => CustomResponse(await mediatorHandler.SendCommand(command));
    }
}