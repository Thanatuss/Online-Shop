using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Command.DTO.User;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Persistance.DBContext;

namespace Application.Command.Utilities
{
    public class UserValidationService(CommandDBContext queryDBContext)
    {
        private readonly CommandDBContext _queryContext = queryDBContext;

        public async Task<bool> AreFieldsNotEmpty(SignUpDTO signUpDTO)
        {
             return !(string.IsNullOrWhiteSpace(signUpDTO.Fullname) ||
             string.IsNullOrWhiteSpace(signUpDTO.Username) ||
             string.IsNullOrWhiteSpace(signUpDTO.Email) ||
             string.IsNullOrWhiteSpace(signUpDTO.Password));
        }
        public async Task<bool> IsDuplicateExistVRegister(SignUpDTO signUpDTO)
        {
            var AnyEmail = await _queryContext.Users.AnyAsync(x => x.Email == signUpDTO.Email);
            var AnyUsername = await _queryContext.Users.AnyAsync(x => x.Username == signUpDTO.Username);
            return (AnyEmail || AnyUsername);
        }
        public async Task<User> FindUserViaUP(RemoveAccountDTO removeAccountDTO)
        {
            var user = await _queryContext.Users
                .SingleOrDefaultAsync(x => x.Username == removeAccountDTO.Username && x.Password == removeAccountDTO.Password && x.IsDeleted == false);
            return user; 

        }
        public async Task<User> FindUserViaEmail(UpdateDTO updateDTO)
        {
            var user = await _queryContext.Users
                .SingleOrDefaultAsync(x => x.Email == updateDTO.Email && x.IsDeleted == false);
            return user;

        }
        public bool IsValidEmail(string email)
        {
            var emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailRegex);
        }
        public bool IsValidUsername(string username)
        {
            return username.Length >= 3 && username.Length <= 20 && username.All(c => char.IsLetterOrDigit(c));
        }
        public bool IsValidPassword(string password)
        {
            var passwordRegex = @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.,^#])[A-Za-z\d@$!%*?&.,^#]{8,}$";
            return Regex.IsMatch(password, passwordRegex);
        }


    }
}
