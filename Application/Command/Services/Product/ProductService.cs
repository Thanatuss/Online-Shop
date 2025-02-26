using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<OperationHandler> Add(AddDTO addDTO)
        {
            var AllInputsValidation = _productValidationService.AreFieldsNotEmpty(addDTO);
            if (AllInputsValidation == false)
            {
                var IsDuplicate = _productValidationService.IsDuplicateExistViProductId(addDTO);
                if (IsDuplicate)
                {
                    return OperationHandler.Error("Your Product ID is repeated!");

                }

                try
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
                    return OperationHandler.Success("We created your product successfully!");
                }
                catch (Exception e)
                {
                    return OperationHandler.Error($"An error occurred while creating the product : {e}");
                }

            }
            return OperationHandler.Error("We can not create your product successfully!");

        }

        public async Task<OperationHandler> Delete(DeleteDTO deleteDTO)
        {
            var Product = _productValidationService.IsExistAnyViaId(deleteDTO);
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
                    return OperationHandler.Error($"An error occurred while creating the product : {e}");
                }

            }
            return OperationHandler.NotFound("We can not find any product!");
        }

        public async Task<OperationHandler> Update(UpdateDTO updateDTO)
        {
            var ProductQuery = _queryDbContext.Products.SingleOrDefault(x => x.ProductId == updateDTO.ProductId);
            var ProductCommand = _commandDbContext.Products.SingleOrDefault(x => x.ProductId == updateDTO.ProductId);
            if (ProductQuery != null && ProductCommand != null)
            {
                try
                {
                    ProductQuery.Description = updateDTO.Description;
                    ProductQuery.Price = updateDTO.Price;
                    ProductQuery.IsActive = updateDTO.IsActive;
                    ProductCommand.Description = updateDTO.Description;
                    ProductCommand.Price = updateDTO.Price;
                    ProductCommand.IsActive = updateDTO.IsActive;
                    await _queryDbContext.SaveChangesAsync();
                    await _commandDbContext.SaveChangesAsync();
                    return OperationHandler.Success("We updated your information!");
                }
                catch(Exception e)
                {
                    return OperationHandler.Error($"An error occurred while creating the product : {e}");
                }
            }
            return OperationHandler.Success("We could not update your information!");
        }
    }
}
