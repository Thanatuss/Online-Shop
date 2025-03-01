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

        public async Task<IActionResult> Add(string text , string userId , string productId)
        {
            var command = new AddCommentCommand(new AddCommentDTO()
            {
                ProductID = Convert.ToInt32(productId),
                UserID = Convert.ToInt32(userId),
                Text = text
            });
            var result = _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var command = new ReadCommentCommand(new AddDTO()
            {

            });
            var result =await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(string CommentId , string Text , string isDelete)
        {
            if (Convert.ToInt32(isDelete) == 0)
            {
                var command = new UpdateCommentCommand(new UpdateDTO()
                {
                    Text = Text,
                    CommentId = Convert.ToInt32(CommentId),

                    IsDelete = false
                });
                var result = _mediator.Send(command);
                return Ok(command);

            }
            else
            {
                var command = new UpdateCommentCommand(new UpdateDTO()
                {
                    Text = Text,
                    CommentId = Convert.ToInt32(CommentId),

                    IsDelete = true
                });
                var result = _mediator.Send(command);
                return Ok(command);
            }

        }
    }
}
