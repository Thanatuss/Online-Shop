using System.Threading.Tasks;
using Application.Command.DTO.Basket;
using Application.Command.Services.Basket;
using Application.Query.DTO.Basket;
using Application.Query.Services.Basket;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IBasketServiceQuery _bakBasketServiceQuery;
        private readonly IMediator _mediator;
        public BasketController(IMediator mediator, IBasketService basketService , IBasketServiceQuery bakeBasketServiceQuery)
        {
            _mediator = mediator;
            _basketService = basketService;
            _bakBasketServiceQuery = bakeBasketServiceQuery;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add(string productId, string userId)
        {
            /*var result =await _basketService.AddAsync(new BasketDTO()
            {
                ProductID = Convert.ToInt32(productId),
                UserID = Convert.ToInt32(userId)

            });*/
            var command = new AddBasketCommand(new BasketDTO()
            {
                ProductID = Convert.ToInt32(productId),
                UserID = Convert.ToInt32(userId)
            });
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string userId)
        {
            var command = new ReadBasketCommand(new GetAllDTO()
            {
                UserId = Convert.ToInt32(userId)
            });
            var result =await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(DeleteBasketDTO deleteBasket)
        {
            var command = new DeleteBasketCommand(new DeleteBasketDTO(){ProductID = deleteBasket.ProductID , UserID = deleteBasket.UserID});
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
