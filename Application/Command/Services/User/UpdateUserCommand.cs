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
        private readonly QueryDBContext _queryContext;
        private readonly CommandDBContext _commandContext;
        public UpdateUserHandler(QueryDBContext queryDBContext, CommandDBContext commandDBContext, UserValidationService userValidationService)
        {
            _userValidationService = userValidationService;
            _queryContext = queryDBContext;
            _commandContext = commandDBContext;

        }

        public async Task<OperationHandler> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var Updateuser = request.UpdateDTO;
            var areFiledsValid = _userValidationService.AreFieldsNotEmpty(new SignUpDTO { Username = Updateuser.Username, Email = Updateuser.Email, Fullname = Updateuser.Fullname, Password = Updateuser.Password });
            var User = await _userValidationService.FindUserViaEmail(Updateuser);
            if (User != null)
            {
                var UserCommand =
                    _commandContext.Users.SingleOrDefault(x => x.Email == Updateuser.Email && x.IsDeleted == false);
                User.Fullname = Updateuser.Fullname;
                User.Username = Updateuser.Username;
                User.Password = Updateuser.Password;
                UserCommand.Fullname = Updateuser.Fullname;
                UserCommand.Username = Updateuser.Username;
                UserCommand.Password = Updateuser.Password;
                
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
