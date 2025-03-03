using Application.Command.DTO.Basket;
using Application.Command.Services.Basket;
using Application.Query.DTO.Basket;
using Application.Query.Services.Basket;
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
        public BasketController(IBasketService basketService , IBasketServiceQuery bakeBasketServiceQuery)
        {
            _basketService = basketService;
            _bakBasketServiceQuery = bakeBasketServiceQuery;
        }
        [HttpPost("Add")]
        public IActionResult Add(string productId, string userId)
        {
            var result = _basketService.AddToBasket(new BasketDTO()
            {
                ProductID = Convert.ToInt32(productId),
                UserID = Convert.ToInt32(userId)

            });
            return Ok(result);
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll(string userId)
        {
            var result = _bakBasketServiceQuery.GetAll(new GetAllDTO() { UserId = Convert.ToInt32(userId) });
            return Ok(result);
        }
    }
}
