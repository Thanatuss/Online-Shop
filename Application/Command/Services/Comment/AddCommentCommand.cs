using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Command.DTO.CommentDTO;
using Application.Command.Utilities;
using Domain.Comment;
using FluentValidation;
using MediatR;
using Persistance.DBContext;

namespace Application.Command.Services.Comment
{
    public class AddCommentCommand : IRequest<OperationHandler>
    {
        public AddCommentDTO AddCommentDTO { get; set; }

        public AddCommentCommand(AddCommentDTO addComment)
        {
            AddCommentDTO = addComment;
        }
    }

    public class AddCommentHandler : IRequestHandler<AddCommentCommand, OperationHandler>
    {
        private readonly CommandDBContext _commandDb;
        private readonly IValidator<AddCommentDTO> _validator;

        public AddCommentHandler(IValidator<AddCommentDTO> validator, CommandDBContext command, QueryDBContext queryDb)
        {
            _commandDb = command;
            _validator = validator;
        }

        public async Task<OperationHandler> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var addCommentDTO = request.AddCommentDTO;

            // اعتبارسنجی ورودی‌ها با استفاده از FluentValidation
            var validationResult = await _validator.ValidateAsync(addCommentDTO);

            // اگر اعتبارسنجی ناموفق باشد، خطاها را باز می‌گردانیم
            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                return OperationHandler.Error(errorMessages);
            }

            // بررسی اینکه کاربر و محصول موجود باشند
            var user = _commandDb.Users.SingleOrDefault(x => x.Id == addCommentDTO.UserID);
            if (user == null)
            {
                return OperationHandler.Error("User not found.");
            }

            var product = _commandDb.Products.SingleOrDefault(x => x.Id == addCommentDTO.ProductID);
            if (product == null)
            {
                return OperationHandler.Error("Product not found.");
            }

            // ایجاد کامنت جدید و افزودن به پایگاه داده
            var comment = new ProductComment()
            {
                Product = product,
                User = user,
                IsDeleted = false,
                Text = addCommentDTO.Text
            };

            // ذخیره‌سازی در پایگاه داده
            _commandDb.ProductComments.Add(comment);
            await _commandDb.SaveChangesAsync(cancellationToken);

            // بازگشت پیغام موفقیت
            return OperationHandler.Success("We posted your comment successfully!");
        }
    }

    // تعریف Validator برای AddCommentDTO
    public class AddCommentDTOValidator : AbstractValidator<AddCommentDTO>
    {
        public AddCommentDTOValidator()
        {
            // اعتبارسنجی برای UserID: باید یک عدد بزرگتر از صفر باشد.
            RuleFor(x => x.UserID)
                .GreaterThan(0).WithMessage("User ID must be greater than zero.");

            // اعتبارسنجی برای ProductID: باید یک عدد بزرگتر از صفر باشد.
            RuleFor(x => x.ProductID)
                .GreaterThan(0).WithMessage("Product ID must be greater than zero.");

            // اعتبارسنجی برای Text: نباید خالی باشد و حداقل 3 کاراکتر داشته باشد.
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Comment text cannot be empty.")
                .MinimumLength(3).WithMessage("Comment text must be at least 3 characters long.");
        }
    }
}
