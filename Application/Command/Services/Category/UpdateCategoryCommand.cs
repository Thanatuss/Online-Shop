using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.Category;
using Application.Command.DTO.CommentDTO;
using Application.Command.Services.Comment;
using Application.Command.Utilities;
using FluentValidation;
using MediatR;
using Persistance.DBContext;

namespace Application.Command.Services.Category
{
    class UpdateCategoryCommand : IRequest<OperationHandler>
    {
        public CategoryUpdateDTO CategoryUpdateDTO { get; set; }

        public UpdateCategoryCommand(CategoryUpdateDTO category)
        {
            CategoryUpdateDTO = category;
        }
    }
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, OperationHandler>
    {
        private readonly CommandDBContext _commandDb;
        private readonly IValidator<UpdateCategoryValidator> _validator;

        public UpdateCategoryHandler(IValidator<UpdateCategoryValidator> validator, CommandDBContext commandDbContext)
        {
            _commandDb = commandDbContext;
            _validator = validator;
        }

        public async Task<OperationHandler> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var categoryDTO = request.UpdateDTO;

            // اعتبارسنجی داده‌ها
            var validationResult = await _validator.ValidateAsync((IValidationContext)categoryDTO);
            if (!validationResult.IsValid)
            {
                // اگر اعتبارسنجی شکست، خطا را بازگشت بدهید
                return OperationHandler.Error("Validation failed: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            // جستجو برای کامنت بر اساس ID
            var category = _commandDb.Categorys.SingleOrDefault(x => x.Id == categoryDTO.);

            // اگر کامنت یافت نشد
            if (userCommand == null)
            {
                return OperationHandler.Error("Comment not found");
            }

            // بروزرسانی فیلدها
            userCommand.Text = categoryDTO.Text;
            userCommand.IsDeleted = categoryDTO.IsDelete;

            // ذخیره تغییرات در پایگاه داده
            await _commandDb.SaveChangesAsync();

            // بازگشت موفقیت
            return OperationHandler.Success("We updated your comment successfully!");
        }
    }
    public class UpdateCategoryValidator : AbstractValidator<CategoryUpdateDTO>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required!")
                .MinimumLength(3).WithMessage("at least category name must has 3 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description name is required!")
                .MinimumLength(5).WithMessage("at least category Description must has 5 characters");

            RuleFor(x => x.ParentId)
                .NotEmpty().WithMessage("Comment text cannot be empty.")
                
        }
    }

}
/*
 *using System;
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
   
 *
 */