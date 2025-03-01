using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.User;
using Application.Command.Utilities;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Persistance.DBContext;

namespace Application.Command.Services.User
{
    public class RegisterProductCommand : IRequest<OperationHandler>
    {
        public SignUpDTO SignUpDTO { get; set; }

        public RegisterProductCommand(SignUpDTO signUpDTO)
        {
            SignUpDTO = signUpDTO;
        }
    }

    public class RegisterProductHandler : IRequestHandler<RegisterProductCommand, OperationHandler>
    {
        private readonly UserValidationService _userValidationService;
        private readonly CommandDBContext _commandContext;

        public RegisterProductHandler(UserValidationService userValidationService , QueryDBContext queryDbContext , CommandDBContext commandDbContext)
        {
            _userValidationService = userValidationService;
            _commandContext = commandDbContext;
        }
        public async Task<OperationHandler> Handle(RegisterProductCommand request, CancellationToken cancellationToken)
        {
            var SignUpDTO = request.SignUpDTO;
            var areFiledsValid = _userValidationService.AreFieldsNotEmpty(SignUpDTO);
            if (!areFiledsValid)
            {
                return OperationHandler.Error("Your informations can not be empty!");
            }
            var isEmialOrUsernameAreDuplicate = await _userValidationService.IsDuplicateExistVRegister(SignUpDTO);
            if (!isEmialOrUsernameAreDuplicate)
            {
                _commandContext.Users.Add(new Domain.Entity.User()
                {
                    Email = SignUpDTO.Email,
                    Fullname = SignUpDTO.Fullname,
                    IsDeleted = false,
                    Password = SignUpDTO.Password,
                    Role = UserRole.User,
                    Username = SignUpDTO.Username
                });
                
                _commandContext.SaveChanges();
                return OperationHandler.Success("We created your email successfully!");
            }
            return OperationHandler.Error("Your Email or Username is exist!");



        }
    }
    
}