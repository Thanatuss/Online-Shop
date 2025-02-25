using System.Threading.Tasks;
using Application.Command.DTO.User;
using Application.Command.Services.User;
using Application.Command.Utilities;
using Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserServiceQuery _userServiceQuery;
        public UserController(IUserService userService, IUserServiceQuery userServiceQuery)
        {
            _userService = userService;
            _userServiceQuery = userServiceQuery;
        }
        public async Task<IActionResult> Register(string name, string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest("Informations can not be empty!");
            }
            var result = await _userService.Register(new Application.Command.DTO.User.SignUpDTO
            {
                Fullname = name,
                Password = password,
                Username = username,
                Email = email
            }
            );

            if (result.Status == Status.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        [HttpPost("RemoveAccount")]
        public async Task<IActionResult> RemoveAccount(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest("Your username or password can not be empty!");
            }
            var result = await _userService.DeleteAccount(new Application.Command.DTO.User.RemoveAccountDTO
            {
                Username = username,
                Password = password
            });
            if (result.Status == Status.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }

        }

        [HttpPost("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_userServiceQuery.Read_GetAllUser());
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(string fullname, string username, string password, string email)
        {
            var result = await _userService.Update(new UpdateDTO()
            {
                Email = email,
                Fullname = fullname,
                Password = password,
                Username = username
            });
            /*if (result.Status == Status.Success)
            {
            }*/
            if (result.Status == Status.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);

        }


    }
}
