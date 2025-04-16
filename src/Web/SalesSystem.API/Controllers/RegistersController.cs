using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MidR.Interfaces;
using SalesSystem.Register.Application;
using SalesSystem.Register.Application.Commands.Authentication.AddUserRole;
using SalesSystem.Register.Application.Commands.Authentication.CreateRole;
using SalesSystem.Register.Application.Commands.Authentication.ForgetPassword;
using SalesSystem.Register.Application.Commands.Authentication.ResetPassword;
using SalesSystem.Register.Application.Commands.Authentication.SignIn;
using SalesSystem.Register.Application.Commands.Authentication.SignUp;
using SalesSystem.Register.Application.Commands.Customers.AddAddress;
using SalesSystem.Register.Application.Queries.Customers.GetById;
using SalesSystem.SharedKernel.Abstractions.Mediator;
using SalesSystem.SharedKernel.Enums;

namespace SalesSystem.API.Controllers
{
    [Route("api/v1/registers")]
    public class RegistersController(IHttpContextAccessor httpContextAccessor,
                                IMediatorHandler mediator,
                                IMediator mediator1)
                              : MainController(httpContextAccessor)
    {
        [HttpGet]
        public async Task<IResult> GetByIdAsync()
            => CustomResponse(await mediator.SendQuery(new GetCustomerByIdQuery(GetUserId())));

        [HttpPost]
        public async Task<IResult> SignUpAsync(SignUpUserCommand command)
            => CustomResponse(await mediator1.DispatchAsync(command));

        [HttpPost("signin")]
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

        [Authorize(Roles = nameof(EUserRoles.Admin))]
        [HttpPost("roles")]
        public async Task<IResult> CreateRoleAsync(CreateRoleCommand command)
            => CustomResponse(await mediator.SendCommand(command));

        [Authorize(Roles = nameof(EUserRoles.Admin))]
        [HttpPost("roles/user-role")]
        public async Task<IResult> AddRoleToUserAsync(AddUserRoleCommand command)
            => CustomResponse(await mediator.SendCommand(command));
    }
}