using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.Basket;
using Application.Command.Utilities;

namespace Application.Command.Services.Basket
{
    public interface IBasketService
    {
        OperationHandler AddToBasket(BasketDTO basketDto);
    }
}
