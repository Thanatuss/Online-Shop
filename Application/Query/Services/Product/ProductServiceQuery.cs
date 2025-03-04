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
    public class ProductServiceQuery(QueryDBContext  queryDbContext) : IProductServiceQuery
    {
        private readonly QueryDBContext _queryDBContext = queryDbContext;
        List<Domain.ProductEntity.Product> IProductServiceQuery.GetAll()
        {
            return _queryDBContext.Products.AsNoTracking().ToList();
        }
    }
}
