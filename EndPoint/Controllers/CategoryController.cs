using Application.Command.DTO.Category;
using Application.Command.Services.Category;
using Application.Query.DTO.CategoryQuery;
using Application.Query.Services.Category;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("add")]
        public IActionResult Add(AddCategoryDTO data)
        {
            var command = new AddCategoryCommand(data);
            var result = _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var command = new ReadCategoryQueryCommand(new ReadCategoryDTOQuery { });
            var result = _mediator.Send(command);
            return Ok(result);
        }
    }
}
