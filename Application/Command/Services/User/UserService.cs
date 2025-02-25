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
    public class UserServic  : IUserService
    {
        private readonly UserValidationService _userValidationService;
        private readonly QueryDBContext _queryContext ;
        private readonly CommandDBContext _commandContext;
        public UserServic(QueryDBContext queryDBContext, CommandDBContext commandDBContext, UserValidationService userValidationService)
        {
            _userValidationService = userValidationService; 
            _queryContext = queryDBContext;
            _commandContext = commandDBContext;

        }
        public async Task<OperationHandler> Register(SignUpDTO signUpDTO)
        {
            var areFiledsValid = _userValidationService.AreFieldsNotEmpty(signUpDTO);
            if (!areFiledsValid)
            {
                return OperationHandler.Error("Your informations can not be empty!");
            }
            var isEmialOrUsernameAreDuplicate = await _userValidationService.IsDuplicateExistVRegister(signUpDTO);
            if (!isEmialOrUsernameAreDuplicate)
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
                return OperationHandler.Success("We created your email successfully!");
            }
            return OperationHandler.Error("Your Email or Username is exist!");
            


        }



        public async Task<OperationHandler> DeleteAccount(RemoveAccountDTO removeAccountDTO)
        {
            var User = _userValidationService.FindUserViaUP(removeAccountDTO);
            if (User != null)
            {
                _commandContext.Users.Remove(User);
                _queryContext.Users.Remove(User);
                await _commandContext.SaveChangesAsync();
                await _queryContext.SaveChangesAsync();
                return OperationHandler.Success("Your account Removed successfully!");
            }
            return OperationHandler.NotFound("We could not find any account!");
        }

        public async Task<OperationHandler> Update(UpdateDTO updateDTO)
        {
            var areFiledsValid = _userValidationService.AreFieldsNotEmpty(new SignUpDTO { Username = updateDTO.Username ,Email = updateDTO.Email,Fullname = updateDTO.Fullname , Password = updateDTO.Password});
            var User = await _userValidationService.FindUserViaEmail(updateDTO);
            if (User != null)
            {
                User.Fullname = updateDTO.Fullname;
                User.Username = updateDTO.Username;
                User.Password = updateDTO.Password;
                await _commandContext.SaveChangesAsync();
                await _queryContext.SaveChangesAsync();
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
    }
}

