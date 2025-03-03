using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.ProductDTO;
using Microsoft.EntityFrameworkCore;
using Persistance.DBContext;
using Domain.ProductEntity;
namespace Application.Command.Services.Product
{
    public class ProductValidationService(CommandDBContext queryDbContext)
    {
        private readonly CommandDBContext _queryDbContext = queryDbContext;

        public bool AreFieldsNotEmpty(AddDTO addDTO)
        {
            return !(string.IsNullOrWhiteSpace(addDTO.Description) || string.IsNullOrWhiteSpace(addDTO.ProductName) ||
        addDTO.Price > 0 || addDTO.ProductId > 0);
        }

        public  bool IsDuplicateExistViProductId(AddDTO addDTO)
        {
            return _queryDbContext.Products.Any(x => x.ProductId == addDTO.ProductId);
        }

        public Domain.ProductEntity.Product IsExistAnyViaId(DeleteDTO deleteDTO)
        {
            var data = _queryDbContext.Products.Any(x => x.ProductId == deleteDTO.ProductId);
            return _queryDbContext.Products.SingleOrDefault(x => x.ProductId == deleteDTO.ProductId);
        }
        public Domain.ProductEntity.Product IsExistAnyViaIdUpdate(ProductUpdateDTO updateDTO)
        {
            return _queryDbContext.Products.SingleOrDefault(x => x.ProductId == updateDTO.ProductId);
        }


    }
}
