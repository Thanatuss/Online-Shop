using Application.Command.Services.User;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace EndPoint.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _user;
        public UsersController(IUserService user)
        {
            _user = user;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignIn(string name , string username , string email , string password)
        {
            var result = _user.Create(new Application.Command.DTO.User.SignUpDTOS
            {
                Fullname = name , 
                Password = password , 
                Username = username , 
                Email = email
            }
            );

            return Content("hello");
        }
        
        public IActionResult Test()
        {
            return View();
        }
    }
}
