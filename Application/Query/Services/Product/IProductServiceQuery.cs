using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.Utilities;
using Domain.ProductEntity;

namespace Application.Query.Services.Product
{
    public interface IProductServiceQuery
    {
        List<Domain.ProductEntity.Product> GetAll();
    }
}
