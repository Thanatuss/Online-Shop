using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Shared;

namespace Domain.Entity
{
    public class User : Base
    {
        public string Username { get; set; }
        public string Fullname { get; set; }
        [EmailAddress]
        public string Email { get; set; }
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
