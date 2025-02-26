using System.Threading.Tasks;
using Application.Command.DTO.ProductDTO;
using Application.Command.Services.Product;
using Application.Command.Utilities;
using Application.Query.Services.Product;
using Domain.ProductEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService , IProductServiceQuery productServiceQuery) : ControllerBase
    {
        private readonly IProductService _productService = productService;
        private readonly IProductServiceQuery _productServiceQuery = productServiceQuery;

        [HttpPost("Add")]
        public IActionResult Add(string Description , string Price , string ProductId , string ProductName)
        {
            var result = _productService.Add(new AddDTO()
            {
                Description = Description,
                Price = long.Parse(Price),
                ProductId = Convert.ToInt32(ProductId),
                ProductName = ProductName
            });
            return Ok(result);

        }
        [HttpPost("Remove")]
        public IActionResult Remove(string ProductId)
        {
            var result = _productService.Delete(new DeleteDTO { ProductId = Convert.ToInt32(ProductId) });
            if(result.Status == Status.NotFound)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("Update")]
        public IActionResult Update(string ProductId , string ProductName , string Description , string Price , string IsActive0Or1)
        {
            bool isActive ;
            if(Convert.ToInt32(IsActive0Or1) == 0)
            {
                isActive = false;
            }
            else
            {
                isActive = true;
            }
                var result = _productService.Update(new UpdateDTO
                {
                    Description = Description,
                    ProductName = ProductName,
                    ProductId = Convert.ToInt32(ProductId),
                    Price = long.Parse(Price),
                    IsActive = isActive


                })
                ;
            if (result.Status == Status.NotFound)
            {
                return BadRequest(result);
            }
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
