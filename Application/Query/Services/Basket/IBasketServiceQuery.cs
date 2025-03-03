using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.Utilities;
using Application.Query.DTO.Basket;

namespace Application.Query.Services.Basket
{
    public interface IBasketServiceQuery
    {
        Task<string> GetAll(GetAllDTO getAllDto);
    }
}
