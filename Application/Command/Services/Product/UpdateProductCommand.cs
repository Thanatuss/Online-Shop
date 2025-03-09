using Application.Command.DTO.ProductDTO;
using Application.Command.Services.Product;
using Application.Command.Utilities;
using FluentValidation;
using MediatR;
using Persistance.DBContext;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class UpdateProductCommand : IRequest<OperationHandler>
{
    public ProductUpdateDTO ProductUpdateDTO { get; set; }

    public UpdateProductCommand(ProductUpdateDTO updateDto)
    {
        ProductUpdateDTO = updateDto;
    }
}

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, OperationHandler>
{
    private readonly CommandDBContext _commandDbContext;
    private readonly IValidator<ProductUpdateDTO> _validator;

    public UpdateProductHandler(IValidator<ProductUpdateDTO> validator, CommandDBContext commandDbContext)
    {
        _validator = validator;
        _commandDbContext = commandDbContext;
    }

    public async Task<OperationHandler> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var updateDTO = request.ProductUpdateDTO;

        var validationResult = await _validator.ValidateAsync(updateDTO, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return OperationHandler.Error(errorMessages);
        }

        var productCommand = _commandDbContext.Products.SingleOrDefault(x => x.ProductId == updateDTO.ProductId);

        if (productCommand == null)
        {
            return OperationHandler.Error("Product not found!");
        }

        try
        {
            productCommand.Description = updateDTO.Description;
            productCommand.Price = updateDTO.Price;
            productCommand.IsActive = updateDTO.IsActive;
            productCommand.ProductName = updateDTO.ProductName;

            await _commandDbContext.SaveChangesAsync(cancellationToken);

            return OperationHandler.Success("Product information updated successfully!");
        }
        catch (Exception e)
        {
            return OperationHandler.Error($"An error occurred while updating the product: {e.Message}");
        }
    }
}

// اعتبارسنجی برای ProductUpdateDTO
public class UpdateProductValidator : AbstractValidator<ProductUpdateDTO>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description cannot be empty!")
            .MinimumLength(3).WithMessage("Description must be at least 3 characters!");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("ProductName cannot be empty!")
            .MinimumLength(3).WithMessage("ProductName must be at least 3 characters!");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0!")
            .NotEmpty().WithMessage("Price cannot be empty!");
    }
}
