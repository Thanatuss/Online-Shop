using Application.Command.DTO.CommentDTO;
using Application.Command.DTO.ProductDTO;
using Application.Command.Services.Comment;
using Application.Query.Services.Comment;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using UpdateDTO = Application.Command.DTO.CommentDTO.UpdateDTO;

namespace EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(string text, string userId, string productId)
        {
            var command = new AddCommentCommand(new AddCommentDTO()
            {
                ProductID = Convert.ToInt32(productId),
                UserID = Convert.ToInt32(userId),
                Text = text
            });
            var result = await _mediator.Send(command);  // استفاده از await برای فراخوانی
            return Ok(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] string userId)
        {
            var command = new ReadCommentCommand(new AddDTO() { });

            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPost("Update")]
        public async Task<IActionResult> Update(string CommentId, string Text, string isDelete)
        {
            var isDeleteFlag = Convert.ToInt32(isDelete) == 0 ? false : true;

            var command = new UpdateCommentCommand(new UpdateDTO()
            {
                Text = Text,
                CommentId = Convert.ToInt32(CommentId),
                IsDelete = isDeleteFlag
            });

            var result = await _mediator.Send(command);  // استفاده از await برای فراخوانی
            return Ok(result);
        }

    }
}
