using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Command.DTO.ProductDTO;
using Application.Command.Utilities;
using FluentValidation;
using MediatR;
using Persistance.DBContext;

namespace Application.Command.Services.Product
{
    public class AddProductCommand : IRequest<OperationHandler>
    {
        public AddDTO AddDTO { get; }

        public AddProductCommand(AddDTO addDTO)
        {
            AddDTO = addDTO;
        }
    }

    public class AddProductHandler : IRequestHandler<AddProductCommand, OperationHandler>
    {
        private readonly CommandDBContext _commandDbContext;
        private readonly ProductValidationService _productValidationService;
        private readonly IValidator<AddDTO> _validator;

        public AddProductHandler(IValidator<AddDTO> validator, CommandDBContext commandDbContext, ProductValidationService productValidationService, QueryDBContext queryDbContext)
        {
            _validator = validator;
            _commandDbContext = commandDbContext;
            _productValidationService = productValidationService;

        }

        public async Task<OperationHandler> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var addDTO = request.AddDTO;
            var IsInformationAreValid = _validator.Validate(addDTO);
            if (IsInformationAreValid.IsValid)
            {
                // بررسی مقداردهی صحیح فیلدها
                if (!_productValidationService.AreFieldsNotEmpty(addDTO))
                {
                    var category = _commandDbContext.Categorys.SingleOrDefault(x => x.Id == addDTO.CategoryId);
                    if (category != null)
                    {
                        await _commandDbContext.Products.AddAsync(new Domain.ProductEntity.Product()
                        {
                            Description = addDTO.Description,
                            Price = addDTO.Price,
                            ProductId = addDTO.ProductId,
                            ProductName = addDTO.ProductName,
                            CategoryId = addDTO.CategoryId
                        });
                        _commandDbContext.SaveChanges();


                        return OperationHandler.Success("All fields must be filled.");
                    }
                    else
                    {
                        return OperationHandler.Error("We could not find any category by this Id");
                    }

                }

                // بررسی تکراری نبودن Product ID
                if (_productValidationService.IsDuplicateExistViProductId(addDTO))
                {
                    return OperationHandler.Error("Your Product ID is repeated!");
                }

                try
                {
                    var category = _commandDbContext.Categorys.SingleOrDefault(x => x.Id == addDTO.CategoryId);
                    if (category != null)
                    {
                        var newProduct = new Domain.ProductEntity.Product
                        {
                            Description = addDTO.Description,
                            Price = addDTO.Price,
                            ProductId = addDTO.ProductId,
                            ProductName = addDTO.ProductName,
                            CategoryId = addDTO.CategoryId
                        };

                        await _commandDbContext.Products.AddAsync(newProduct, cancellationToken);

                        await _commandDbContext.SaveChangesAsync(cancellationToken);

                        return OperationHandler.Success("We created your product successfully!");
                    }
                    return OperationHandler.Error("We could not find any category by this Id ");

                }
                catch (Exception e)
                {
                    return OperationHandler.Error($"An error occurred while creating the product: {e.Message}");
                }
            }
            else
            {
                foreach (var error in IsInformationAreValid.Errors)
                {
                    if (error.PropertyName == "CategoryId" && error.ErrorMessage.Contains("must bigger than 0"))
                    {
                        return OperationHandler.Error("Your CategoryId must be greater than 0!");
                    }

                    if (error.PropertyName == "CategoryId" && error.ErrorMessage.Contains("can not be empty"))
                    {
                        return OperationHandler.Error("Your CategoryId can not be empty!");
                    }

                    if (error.PropertyName == "Description" && error.ErrorMessage.Contains("can not be empty"))
                    {
                        return OperationHandler.Error("Your Description can not be empty!");
                    }

                    if (error.PropertyName == "Description" &&
                        error.ErrorMessage.Contains("must be at least 3 characters"))
                    {
                        return OperationHandler.Error("Your Description must be at least 3 characters!");
                    }

                    if (error.PropertyName == "Price" && error.ErrorMessage.Contains("can not be empty"))
                    {
                        return OperationHandler.Error("Your Price can not be empty!");
                    }

                    if (error.PropertyName == "Price" && error.ErrorMessage.Contains("must be greater than 0"))
                    {
                        return OperationHandler.Error("Your Price must be greater than 0!");
                    }

                    if (error.PropertyName == "ProductId" && error.ErrorMessage.Contains("must bigger than 0"))
                    {
                        return OperationHandler.Error("Your ProductId must be greater than 0!");
                    }

                    if (error.PropertyName == "ProductId" && error.ErrorMessage.Contains("can not be empty"))
                    {
                        return OperationHandler.Error("Your ProductId can not be empty!");
                    }

                    if (error.PropertyName == "ProductName" && error.ErrorMessage.Contains("must bigger than 3"))
                    {
                        return OperationHandler.Error("Your ProductName must be greater than 3 characters!");
                    }

                    if (error.PropertyName == "ProductName" && error.ErrorMessage.Contains("can not be empty"))
                    {
                        return OperationHandler.Error("Your ProductName can not be empty!");
                    }
                }
                return OperationHandler.NotFound("We could not add the product!");
            }
            
        }
    }

    public class AddProductValidator : AbstractValidator<AddDTO>
    {
        public AddProductValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Your CategoryId must bigger than 0"!)
                .NotEmpty().WithMessage("Your CategoryId can not be empty!");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Your Description can not be empty!")
                .MinimumLength(3).WithMessage("Your Description must be at least 3 charactors!");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Your Price can not be empty!")
                .GreaterThan(0).WithErrorCode("Your price can not be empty!");
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Your ProductId must bigger than 0"!)
                .NotEmpty().WithMessage("Your ProductId can not be empty!");
            RuleFor(x => x.ProductName)
                .MinimumLength(3).WithMessage("Your ProductName must bigger than 3"!)
                .NotEmpty().WithMessage("Your ProductName can not be empty!");


        }
    }
}