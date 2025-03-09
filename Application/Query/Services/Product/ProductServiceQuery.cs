using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Persistance.DBContext;

namespace Application.Query.Services.Product
{
    public class ProductServiceQuery(CommandDBContext queryDbContext) : IProductServiceQuery
    {
        private readonly CommandDBContext _commandDBContext = queryDbContext;
        List<Domain.ProductEntity.Product> IProductServiceQuery.GetAll()
        {
            return _commandDBContext.Products.AsNoTracking().ToList();
        }
    }
}
