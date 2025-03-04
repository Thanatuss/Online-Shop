using System.Threading.Tasks;
using Application.Command.DTO.User;
using Application.Command.Services.User;
using Application.Command.Utilities;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserServiceQuery _userServiceQuery;
        private readonly IMediator _mediator;

        public UserController(IUserService userService, IUserServiceQuery userServiceQuery, IMediator mediator)
        {
            _mediator = mediator;
            _userService = userService;
            _userServiceQuery = userServiceQuery;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] SignUpDTO signUpDto)
        {
            var command = new RegisterProductCommand(signUpDto);
            var result = await _mediator.Send(command);

            if (result.Status == Status.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete("RemoveAccount")]
        public async Task<ActionResult> RemoveAccount([FromBody] RemoveAccountDTO removeAccountDto)
        {
            if (string.IsNullOrWhiteSpace(removeAccountDto.Username) || string.IsNullOrWhiteSpace(removeAccountDto.Password))
            {
                return BadRequest("Your username or password cannot be empty!");
            }

            var command = new DeleteUserCommand(removeAccountDto);
            var result = await _mediator.Send(command);

            if (result.Status == Status.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            var data = await _userServiceQuery.Read_GetAllUser();
            return Ok(data);
        }

        // بروزرسانی اطلاعات کاربری
        [HttpPatch("Update")]
        public async Task<ActionResult> Update([FromBody] UpdateDTO updateDto)
        {
            var command = new UpdateUserCommand(updateDto);
            var result = await _mediator.Send(command);

            if (result.Status == Status.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
