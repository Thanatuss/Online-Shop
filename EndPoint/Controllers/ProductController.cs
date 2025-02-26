using System.Threading.Tasks;
using Application.Command.DTO.ProductDTO;
using Application.Command.Services.Product;
using Application.Command.Utilities;
using Application.Query.Services.Product;
using Domain.ProductEntity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductServiceQuery _productServiceQuery;
        private readonly IMediator _mediator;

        public ProductController(IProductService productService, IMediator mmediator, IProductServiceQuery productServiceQuery)
        {
            _productService = productService;
            _productServiceQuery = productServiceQuery;
            _mediator = mmediator; 
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add(string name, string description, string price, string Id)
        {
            var command = new AddProductCommand(new AddDTO()
            {
                Description = description ,
                Price = long.Parse(price),
                ProductId = Convert.ToInt32(Id),
                ProductName = name
            });
            var result = await _mediator.Send(command);
            /* var result = _productService.Add(new AddDTO()
            {
                Description = Description,
                Price = long.Parse(Price),
                ProductId = Convert.ToInt32(ProductId),
                ProductName = ProductName
            }); */
            return Ok(result);

        }
        [HttpPost("Remove")]
        public IActionResult Remove(string ProductId)
        {
            var result = _productService.Delete(new DeleteDTO { ProductId = Convert.ToInt32(ProductId) });
            return Ok(result);
        }
        [HttpPost("Update")]
        public IActionResult Update(string ProductId, string ProductName, string Description, string Price, string IsActive0Or1)
        {
            bool isActive;
            if (Convert.ToInt32(IsActive0Or1) == 0)
            {
                isActive = false;
            }
            else
            {
                isActive = true;
            }
            
            var data = new UpdateProductCommand(new UpdateDTO(){
                    Description = Description , 
                    IsActive = isActive , 
                    Price = long.Parse(Price),
                    ProductId = Convert.ToInt32(ProductId),
                    ProductName = ProductName

            });
            var result = _mediator.Send(data);

            return Ok(result);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var data = _productServiceQuery.GetAll();
            return Ok(data);
        }
    }
}
