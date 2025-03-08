using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Command.DTO.Category;
using Application.Command.DTO.CommentDTO;
using Application.Command.Utilities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.DBContext;

namespace Application.Command.Services.Category
{
    public class UpdateCategoryCommand : IRequest<OperationHandler>
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
        private readonly IValidator<CategoryUpdateDTO> _validator;

        public UpdateCategoryHandler(IValidator<CategoryUpdateDTO> validator, CommandDBContext command, QueryDBContext queryDb)
        {
            _commandDb = command;
            _validator = validator;
        }

        public async Task<OperationHandler> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryData = request.CategoryUpdateDTO;
            var validation = _validator.Validate(categoryData);

            if (validation.IsValid)
            {
                var data = await _commandDb.Categorys.SingleOrDefaultAsync(x => x.Id == categoryData.Id);
                if (data != null)
                {
                    data.Name = categoryData.Name;
                    data.Description = categoryData.Description;

                    // Force EF to mark this entity as modified
                    _commandDb.Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await _commandDb.SaveChangesAsync();

                    return OperationHandler.Success("The information is saved!");
                }
                return OperationHandler.Error("Category not found!");
            }
            return OperationHandler.Error("Validation failed. The information is not saved!");
        }
    }

    public class UpdateCategoryValidator : AbstractValidator<CategoryUpdateDTO>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Your Id must bigger than 0"!)
                .NotEmpty().WithMessage("Your id can not be empty!");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Your name can not be empty!")
                .MinimumLength(3).WithMessage("Your name must be at least 3 charactors!");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Your Description can not be empty!")
                .MinimumLength(10).WithMessage("Your Description must be at least 10 charactors!");
        }
    }
}
