using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Command.DTO.ProductDTO;
using Application.Command.Utilities;
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
        private readonly QueryDBContext _queryDbContext;
        private readonly ProductValidationService _productValidationService;

        public AddProductHandler(CommandDBContext commandDbContext, ProductValidationService productValidationService, QueryDBContext queryDbContext)
        {
            _commandDbContext = commandDbContext;
            _productValidationService = productValidationService;
            _queryDbContext = queryDbContext;
        }

        public async Task<OperationHandler> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var addDTO = request.AddDTO;

            // بررسی مقداردهی صحیح فیلدها
            if (!_productValidationService.AreFieldsNotEmpty(addDTO))
            {
                await _commandDbContext.Products.AddAsync(new Domain.ProductEntity.Product()
                {
                    Description = addDTO.Description,
                    Price = addDTO.Price,
                    ProductId = addDTO.ProductId,
                    ProductName = addDTO.ProductName
                });
                await _queryDbContext.Products.AddAsync(new Domain.ProductEntity.Product()
                {
                    Description = addDTO.Description,
                    Price = addDTO.Price,
                    ProductId = addDTO.ProductId,
                    ProductName = addDTO.ProductName
                });
                _commandDbContext.SaveChanges();
                _queryDbContext.SaveChanges();

                return OperationHandler.Success("All fields must be filled.");
            }

            // بررسی تکراری نبودن Product ID
            if (_productValidationService.IsDuplicateExistViProductId(addDTO))
            {
                return OperationHandler.Error("Your Product ID is repeated!");
            }

            try
            {
                var newProduct = new Domain.ProductEntity.Product
                {
                    Description = addDTO.Description,
                    Price = addDTO.Price,
                    ProductId = addDTO.ProductId,
                    ProductName = addDTO.ProductName
                };

                await _commandDbContext.Products.AddAsync(newProduct, cancellationToken);
                await _queryDbContext.Products.AddAsync(newProduct, cancellationToken);

                await _commandDbContext.SaveChangesAsync(cancellationToken);
                await _queryDbContext.SaveChangesAsync(cancellationToken);

                return OperationHandler.Success("We created your product successfully!");
            }
            catch (Exception e)
            {
                return OperationHandler.Error($"An error occurred while creating the product: {e.Message}");
            }
        }
    }
}