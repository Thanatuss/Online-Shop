using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.Basket;
using Application.Command.Utilities;
using Infrastructore.Interfaces;

namespace Application.Command.Services.Basket
{
    public interface IBasketService 
    {
        Task<OperationHandler> AddAsync(BasketDTO basketDto);
        //OperationHandler Add<T>(BasketDTO basketDto);
    }
}
