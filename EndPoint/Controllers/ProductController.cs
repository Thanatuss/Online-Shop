using System.Threading.Tasks;
using Application.Command.DTO.ProductDTO;
using Application.Command.Services.Product;
using Application.Query.Services.Product;
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

        public ProductController(IProductService productService, IMediator mediator, IProductServiceQuery productServiceQuery)
        {
            _productService = productService;
            _productServiceQuery = productServiceQuery;
            _mediator = mediator;
        }

        // اکشن برای اضافه کردن محصول
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AddDTO addDto)
        {
            var command = new AddProductCommand(addDto);
            var result = await _mediator.Send(command); // استفاده از await
            return Ok(result);
        }

        // اکشن برای حذف محصول
        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove([FromBody] DeleteDTO deleteDto)
        {
            var command = new DeleteProductCommand(deleteDto);
            var result = await _mediator.Send(command); // استفاده از await
            return Ok(result);
        }

        // اکشن برای بروزرسانی محصول
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ProductUpdateDTO updateDto)
        {
            var result = await _mediator.Send(new UpdateProductCommand(updateDto)); // استفاده از await
            return Ok(result);
        }

        // اکشن برای دریافت تمام محصولات
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var data = _productServiceQuery.GetAll();
            return Ok(data);
        }
    }
}
