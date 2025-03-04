using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.ProductDTO;
using Application.Command.DTO.User;
using Application.Command.Utilities;
using Azure.Core;
using MediatR;
using Persistance.DBContext;

namespace Application.Command.Services.User
{
    public class DeleteUserCommand : IRequest<OperationHandler>
    {
        public RemoveAccountDTO RemoveAccountDTO { get; set; }
        public DeleteUserCommand(RemoveAccountDTO removeAccountDTO)
        {
            RemoveAccountDTO = removeAccountDTO;
        }
    }

    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, OperationHandler>
    {
        private readonly UserValidationService _userValidationService;
        private readonly CommandDBContext _commandContext;
        public DeleteUserHandler(QueryDBContext queryDBContext, CommandDBContext commandDBContext, UserValidationService userValidationService)
        {
            _userValidationService = userValidationService;
            _commandContext = commandDBContext;

        }

        public async Task<OperationHandler> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var deleteDTO = request.RemoveAccountDTO;

            var user = await _userValidationService.FindUserViaUP(deleteDTO);

            if (user == null)
            {
                return OperationHandler.NotFound("We could not find any account!");
            }

            // اگر کاربر پیدا شد، حالت حذف نرم را اعمال می‌کنیم
            user.IsDeleted = true;

            // ذخیره تغییرات
            await _commandContext.SaveChangesAsync(cancellationToken);

            return OperationHandler.Success("Your account has been removed successfully!");
        }

    }

}
