using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.CommentDTO;
using FluentValidation;

namespace Application.Command.Services.FluentValidator
{
    public class UpdateCommentValidator : AbstractValidator<UpdateDTO>
    {
        public UpdateCommentValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Comment text cannot be empty.");


        }
    }
}
