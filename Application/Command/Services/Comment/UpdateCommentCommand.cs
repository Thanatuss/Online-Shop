using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Command.DTO.CommentDTO;
using Application.Command.Utilities;
using FluentValidation;
using MediatR;
using Persistance.DBContext;

namespace Application.Command.Services.Comment
{
    public class UpdateCommentCommand : IRequest<OperationHandler>
    {
        public UpdateDTO UpdateDTO { get; set; }

        public UpdateCommentCommand(UpdateDTO updateDto)
        {
            UpdateDTO = updateDto;
        }
    }

    public class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, OperationHandler>
    {
        private readonly CommandDBContext _commandDb;
        private readonly IValidator<UpdateDTO> _validator;

        public UpdateCommentHandler(IValidator<UpdateDTO> validator, CommandDBContext commandDbContext)
        {
            _commandDb = commandDbContext;
            _validator = validator;
        }

        public async Task<OperationHandler> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var updateDTO = request.UpdateDTO;

            // اعتبارسنجی داده‌ها
            var validationResult = await _validator.ValidateAsync(updateDTO);
            if (!validationResult.IsValid)
            {
                // اگر اعتبارسنجی شکست، خطا را بازگشت بدهید
                return OperationHandler.Error("Validation failed: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            // جستجو برای کامنت بر اساس ID
            var userCommand = _commandDb.ProductComments.SingleOrDefault(x => x.Id == updateDTO.CommentId);

            // اگر کامنت یافت نشد
            if (userCommand == null)
            {
                return OperationHandler.Error("Comment not found");
            }

            // بروزرسانی فیلدها
            userCommand.Text = updateDTO.Text;
            userCommand.IsDeleted = updateDTO.IsDelete;

            // ذخیره تغییرات در پایگاه داده
            await _commandDb.SaveChangesAsync();

            // بازگشت موفقیت
            return OperationHandler.Success("We updated your comment successfully!");
        }
    }
}
