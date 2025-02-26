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
    public class ProductValidationService(QueryDBContext queryDbContext)
    {
        private readonly QueryDBContext _queryDbContext = queryDbContext;

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
            return _queryDbContext.Products.SingleOrDefault(x => x.ProductId == deleteDTO.ProductId);
        }
        public Domain.ProductEntity.Product IsExistAnyViaIdUpdate(UpdateDTO updateDTO)
        {
            return _queryDbContext.Products.SingleOrDefault(x => x.ProductId == updateDTO.ProductId);
        }


    }
}
