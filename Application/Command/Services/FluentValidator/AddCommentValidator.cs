using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.CommentDTO;
using FluentValidation;
namespace Application.Command.Services.FluentValidator
{
    public class AddCommentValidator : AbstractValidator<AddCommentDTO>
    {
        public AddCommentValidator()
        {
            RuleFor(x => x.UserID)
                .NotEmpty().WithMessage("User ID is required.")
                .GreaterThan(0).WithMessage("User ID must be a valid positive number.");

            RuleFor(x => x.ProductID)
                .NotEmpty().WithMessage("Product ID is required.")
                .GreaterThan(0).WithMessage("Product ID must be a valid positive number.");

            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Comment text cannot be empty.")
                .Length(10, 1000).WithMessage("Comment text must be between 10 and 1000 characters.");
        }
    }
}
