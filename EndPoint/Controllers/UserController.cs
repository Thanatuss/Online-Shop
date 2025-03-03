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

        // ثبت‌نام کاربر
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] SignUpDTO signUpDto)
        {
            if (string.IsNullOrWhiteSpace(signUpDto.Fullname) || string.IsNullOrWhiteSpace(signUpDto.Username) ||
                string.IsNullOrWhiteSpace(signUpDto.Email) || string.IsNullOrWhiteSpace(signUpDto.Password))
            {
                return BadRequest("Informations cannot be empty!");
            }

            var command = new RegisterProductCommand(signUpDto);
            var result = await _mediator.Send(command);

            if (result.Status == Status.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        // حذف حساب کاربری
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

        // دریافت همه کاربران
        [HttpGet("GetAll")]
        public ActionResult GetAll()
        {
            var data = _userServiceQuery.Read_GetAllUser();
            return Ok(data);
        }

        // بروزرسانی اطلاعات کاربری
        [HttpPost("Update")]
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
