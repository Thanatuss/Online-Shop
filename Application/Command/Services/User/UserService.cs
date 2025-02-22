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

        // Register Area
        public OperationHandler Register(SignUpDTO signUpDTO)
        {
            var SearchForDuplicate = IsDuplicateExistVRegister(signUpDTO);
            if (SearchForDuplicate)
            {
                _commandContext.Users.Add(new Domain.Entity.User()
                {
                    Email = signUpDTO.Email,
                    Fullname = signUpDTO.Fullname,
                    IsDeleted = false,
                    Password = signUpDTO.Password,
                    Role = UserRole.User,
                    Username = signUpDTO.Username
                });
                _queryContext.Users.Add(new Domain.Entity.User()
                {
                    Email = signUpDTO.Email,
                    Fullname = signUpDTO.Fullname,
                    IsDeleted = false,
                    Password = signUpDTO.Password,
                    Role = UserRole.User,
                    Username = signUpDTO.Username
                });
                _commandContext.SaveChanges();
                _queryContext.SaveChanges();
                return new OperationHandler() { Message = "We created your account successfully!" };
            }
            return new OperationHandler
            {
                Message = "We can not create your new account!"
            };

        }
        private bool IsDuplicateExistVRegister(SignUpDTO signUpDTO)
        {
            var isEmailRepeated = _commandContext.Users.Any(x => x.Email == signUpDTO.Email);
            var isUsernameRepeated = _commandContext.Users.Any(x => x.Username == signUpDTO.Username);
            if (!isEmailRepeated && !isUsernameRepeated)
            {
                return true;
            }
            return false;

        }
        // End Register Area

        // Delete Area
        public OperationHandler DeleteAccount(RemoveAccountDTO removeAccountDTO)
        {
            var ResultFindUser = FindUserVDeleteUser(removeAccountDTO);
            if (ResultFindUser != null)
            {
                var search = _commandContext.Users.Remove(ResultFindUser);
                _commandContext.SaveChanges();
                return new OperationHandler
                {
                    Status = Status.Success,
                    Message = "Your account Removed successfully!"
                };
            }
            return new OperationHandler
            {
                Status = Status.NotFound,
                Message = "We could not Removed your account!"
            };
        }
        private Domain.Entity.User FindUserVDeleteUser(RemoveAccountDTO removeAccountDTO)
        {
            var userfinder = _commandContext.Users.SingleOrDefault(x => x.Username == removeAccountDTO.Username);
            if(userfinder != null)
            {
                var Passwordfinder = userfinder.Password == removeAccountDTO.Password;
                var selectUser =
                    _commandContext.Users.SingleOrDefault(x => x.Username == removeAccountDTO.Username && x.Password == removeAccountDTO.Password && x.IsDeleted == false);
                return userfinder;
            }
            return userfinder;


        }
        // End Delete Area
        //Alidsdfa -- Ebrahimiasdfa
        // Alfsdafsdafa

        // Update Area
        public OperationHandler Update(UpdateDTO updateDTO)
        {
            var ResultFindUserMethod = FindUser(updateDTO);
            if (ResultFindUserMethod != null)
            {
                ResultFindUserMethod.Fullname = updateDTO.Fullname;
                ResultFindUserMethod.Email = updateDTO.Email;
                _commandContext.SaveChanges();
                return new OperationHandler()
                {
                    Message = "We updated your account successfully!",
                    Status = Status.Success
                };
            }
            return new OperationHandler()
            {
                Message = "We could not update your account!",
                Status = Status.Error
            };
        }
        private Domain.Entity.User FindUser(UpdateDTO updateDTO)
        {
            var selectUser =
                _commandContext.Users.SingleOrDefault(x => x.Username == updateDTO.Username && x.Password == updateDTO.Password && x.IsDeleted == false);
            return selectUser;
        }
        // End Update Area
    }
}
