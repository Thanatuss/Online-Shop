using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.DTO.User
{
    public class UpdateDTO
    {
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class SignUpDTO : UpdateDTO{ }



    public class RemoveAccountDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
