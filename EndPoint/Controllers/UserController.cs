using Application.Command.Services.User;
using Domain.Entity;
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
        public UserController(IUserService userService , IUserServiceQuery userServiceQuery)
        {
            _userService = userService;
            _userServiceQuery = userServiceQuery;
        }
        public IActionResult Register(string name, string username, string email, string password)
        {
            var result = _userService.Register(new Application.Command.DTO.User.SignUpDTO
            {
                Fullname = name,
                Password = password,
                Username = username,
                Email = email
            }
            );

            return Ok(result);
        }
        [HttpPost("RemoveAccount")]
        public IActionResult RemoveAccount(string username, string password)
        {
            var result = _userService.DeleteAccount(new Application.Command.DTO.User.RemoveAccountDTO
            {
                Username = username,
                Password = password
            });
            return Ok(result);
        }

        [HttpPost("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_userServiceQuery.Read_GetAllUser());
        }


    }
}
