using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.User;
using Application.Command.Utilities;
using Domain.Entity;
using Persistance.DBContext;
namespace Application.Command.Services.User
{
    public class UserService(QueryDBContext queryDBContext, CommandDBContext commandDBContext) : IUserService
    {
        private readonly QueryDBContext _queryContext = queryDBContext;    
        private readonly CommandDBContext _commandContext = commandDBContext;

        public OperationHandler Create(SignUpDTOS signUpDTOS)
        {
            var anyUsername = _commandContext.Users.Any(x => x.Username == signUpDTOS.Username);
            var anyEmail = _commandContext.Users.Any(x => x.Gmail == signUpDTOS.Email);
            if (!anyEmail && !anyUsername)
            {
                _commandContext.Users.Add(new Domain.Entity.User()
                {
                    Gmail = signUpDTOS.Email,
                    Fullname = signUpDTOS.Fullname,
                    IsDeleted = false,
                    Password = signUpDTOS.Password,
                    Role = UserRole.User,
                    Username = signUpDTOS.Username
                });
                _commandContext.SaveChanges();
                return new OperationHandler() { Message = "We created your account successfully!" };
            }
            else
            {
                return new OperationHandler
                {
                    Message = "We can not create your new account!"
                };
            }
            
        }

        public OperationHandler DeleteAccount(RemoveAccountDTO removeAccountDTO)
        {
            var User = _commandContext.Users.SingleOrDefault(x =>
                x.Username == removeAccountDTO.Username && x.Password == removeAccountDTO.Password);
            if (User != null)
            {
                var search = _commandContext.Users.Remove(User);
                return new OperationHandler
                {
                    Status = Status.Success,
                    Message = "Your account Removed successfully!"
                };
            }
            return new OperationHandler
            {
                Status = Status.NotFound,
                Message = "Your could not Removed your account!"
            };



        }

        public OperationHandler Login(SignUpDTOS signUpDTOS)
        {
            var isEmailRepeated = _commandContext.Users.Any(x => x.Gmail == signUpDTOS.Email);
            var isUsernameRepeated = _commandContext.Users.Any(x => x.Username == signUpDTOS.Username);
            if (!isEmailRepeated && !isUsernameRepeated)
            {
                _commandContext.Users.Add(new Domain.Entity.User
                {
                    Fullname = signUpDTOS.Fullname,
                    Gmail = signUpDTOS.Email,
                    IsDeleted = false,
                    Password = signUpDTOS.Password,
                    Role = UserRole.User,
                    Username = signUpDTOS.Username
                });
                _commandContext.SaveChanges();
            }
            return new OperationHandler() { Message = "Success" };

        }



        public OperationHandler Update(UpdateDTO updateDto)
        {
            var selectUser =
                _commandContext.Users.SingleOrDefault(x => x.Username == updateDto.Username && x.Password == updateDto.Password);
            if (selectUser != null)
            {
                selectUser.Fullname = updateDto.Fullname;
                selectUser.Gmail = updateDto.Email;
                _commandContext.SaveChanges();
                return new OperationHandler()
                {
                    Message = "We updated your account successfully!",
                    Status = Status.Success
                };
            }
            else
            {
                return new OperationHandler()
                {
                    Message = "We could not update your account!", Status = Status.Error
                };
            }
            

        }
    }
}
