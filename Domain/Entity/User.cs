using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class User : Base
    {
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Gmail { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
    public enum UserRole
    {
        Admin , 
        Author , 
        User
    }
}
