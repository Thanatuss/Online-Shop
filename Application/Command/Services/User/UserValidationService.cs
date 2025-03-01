using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public bool AreFieldsNotEmpty(SignUpDTO signUpDTO)
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
        public User FindUserViaUP(RemoveAccountDTO removeAccountDTO)
        {
            var user = _queryContext.Users
                .SingleOrDefault(x => x.Username == removeAccountDTO.Username && x.Password == removeAccountDTO.Password && x.IsDeleted == false);
            return user; 

        }
        public async Task<User> FindUserViaEmail(UpdateDTO updateDTO)
        {
            var user = await _queryContext.Users
                .SingleOrDefaultAsync(x => x.Email == updateDTO.Email && x.IsDeleted == false);
            return user;

        }

    }
}
