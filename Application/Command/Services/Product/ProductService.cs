using Application.Command.DTO.ProductDTO;
using Application.Command.Utilities;
using Persistance.DBContext;

namespace Application.Command.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly CommandDBContext _commandDbContext;
        private readonly QueryDBContext _queryDbContext;
        private readonly ProductValidationService _productValidationService;

        public ProductService(CommandDBContext commandDbContext, ProductValidationService productValidationService, QueryDBContext queryDbContext)
        {
            _commandDbContext = commandDbContext;
            _productValidationService = productValidationService;
            _queryDbContext = queryDbContext;
        }

        // متد Add با محدودیت های مشابه
        public async Task<OperationHandler> Add<T>(T dto) where T : AddDTO
        {
            var AllInputsValidation = _productValidationService.AreFieldsNotEmpty(dto);
            if (!AllInputsValidation)
            {
                var IsDuplicate = _productValidationService.IsDuplicateExistViProductId(dto);
                if (IsDuplicate)
                {
                    return OperationHandler.Error("Your Product ID is repeated!");
                }

                try
                {
                    var product = new Domain.ProductEntity.Product()
                    {
                        Description = dto.Description,
                        Price = dto.Price,
                        ProductId = dto.ProductId,
                        ProductName = dto.ProductName
                    };

                    await _commandDbContext.Products.AddAsync(product);
                    await _queryDbContext.Products.AddAsync(product);
                    await _commandDbContext.SaveChangesAsync();
                    await _queryDbContext.SaveChangesAsync();

                    return OperationHandler.Success("We created your product successfully!");
                }
                catch (Exception e)
                {
                    return OperationHandler.Error($"An error occurred while creating the product : {e}");
                }
            }
            return OperationHandler.Error("We cannot create your product successfully!");
        }

        // متد Delete با محدودیت های مشابه
        public async Task<OperationHandler> Delete<T>(T dto) where T : DeleteDTO
        {
            var Product = _productValidationService.IsExistAnyViaId(dto);
            if (Product != null)
            {
                try
                {
                    _queryDbContext.Products.Remove(Product);
                    _commandDbContext.Products.Remove(Product);
                    await _queryDbContext.SaveChangesAsync();
                    await _commandDbContext.SaveChangesAsync();

                    return OperationHandler.Success("We removed the product successfully!");
                }
                catch (Exception e)
                {
                    return OperationHandler.Error($"An error occurred while deleting the product : {e}");
                }
            }
            return OperationHandler.NotFound("We cannot find any product!");
        }

        // متد Update با محدودیت های مشابه
        public async Task<OperationHandler> Update<T>(T dto) where T : ProductUpdateDTO
        {
            var ProductQuery = _queryDbContext.Products.SingleOrDefault(x => x.ProductId == dto.ProductId);
            var ProductCommand = _commandDbContext.Products.SingleOrDefault(x => x.ProductId == dto.ProductId);

            if (ProductQuery != null && ProductCommand != null)
            {
                try
                {
                    ProductQuery.Description = dto.Description;
                    ProductQuery.Price = dto.Price;
                    ProductQuery.IsActive = dto.IsActive;

                    ProductCommand.Description = dto.Description;
                    ProductCommand.Price = dto.Price;
                    ProductCommand.IsActive = dto.IsActive;

                    await _queryDbContext.SaveChangesAsync();
                    await _commandDbContext.SaveChangesAsync();

                    return OperationHandler.Success("We updated your information!");
                }
                catch (Exception e)
                {
                    return OperationHandler.Error($"An error occurred while updating the product : {e}");
                }
            }
            return OperationHandler.Error("We could not update your information!");
        }
    }
}
