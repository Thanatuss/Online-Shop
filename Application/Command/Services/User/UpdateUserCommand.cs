using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.ProductDTO;
using Application.Command.DTO.User;
using Application.Command.Utilities;
using MediatR;
using Persistance.DBContext;
using Application.Command.DTO.User;
using UpdateDTO = Application.Command.DTO.User.UpdateDTO;
using Microsoft.EntityFrameworkCore;

namespace Application.Command.Services.User
{
    public class UpdateUserCommand : IRequest<OperationHandler>
    {
        public UpdateDTO UpdateDTO { get; set; }

        public UpdateUserCommand(UpdateDTO updateDto)
        {
            UpdateDTO = updateDto;
        }
    }

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, OperationHandler>
    {
        private readonly UserValidationService _userValidationService;
        private readonly CommandDBContext _commandContext;
        public UpdateUserHandler(QueryDBContext queryDBContext, CommandDBContext commandDBContext, UserValidationService userValidationService)
        {
            _userValidationService = userValidationService;
            _commandContext = commandDBContext;

        }
        public async Task<OperationHandler> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var updateUser = request.UpdateDTO;
            var isUsernameValid =  _userValidationService.IsValidUsername(updateUser.Username);
            if (!isUsernameValid)
            {
                return OperationHandler.Error("Username must be between 3 and 20 characters and contain only letters and numbers!");
            }

            //  اعتبارسنجی رمز عبور
            var isPasswordValid = _userValidationService.IsValidPassword(updateUser.Password);
            if (!isPasswordValid)
            {
                return OperationHandler.Error("Password must be at least 8 characters long and contain at least one uppercase letter, one number, and one special character!");
            }


            // بررسی اینکه فیلدهای ورودی پر شده باشند
            var areFieldsValid = await _userValidationService.AreFieldsNotEmpty(new SignUpDTO
            {
                Username = updateUser.Username,
                Email = updateUser.Email,
                Fullname = updateUser.Fullname,
                Password = updateUser.Password
            });

            if (!areFieldsValid)
            {
                return new OperationHandler()
                {
                    Message = "All fields must be filled!",
                    Status = Status.Error
                };
            }

            // جستجوی کاربر بر اساس ایمیل
            var user = await _userValidationService.FindUserViaEmail(updateUser);
            if (user == null)
            {
                return new OperationHandler()
                {
                    Message = "We could not find the user to update!",
                    Status = Status.Error
                };
            }

            // جستجو برای یافتن کاربری که قرار است به‌روزرسانی شود
            var userCommand = await _commandContext.Users
                .SingleOrDefaultAsync(x => x.Email == updateUser.Email && x.IsDeleted == false, cancellationToken);

            if (userCommand == null)
            {
                return new OperationHandler()
                {
                    Message = "User not found or account is deleted!",
                    Status = Status.Error
                };
            }

            // به‌روزرسانی اطلاعات کاربر
            userCommand.Fullname = updateUser.Fullname;
            userCommand.Username = updateUser.Username;
            userCommand.Password = updateUser.Password;

            // ذخیره تغییرات
            await _commandContext.SaveChangesAsync(cancellationToken);

            return new OperationHandler()
            {
                Message = "Your account has been updated successfully!",
                Status = Status.Success
            };
        }

    }
}
