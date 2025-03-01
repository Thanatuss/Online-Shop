using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.CommentDTO;
using Application.Command.Utilities;
using MediatR;
using Persistance.DBContext;

namespace Application.Command.Services.Comment
{
    public class UpdateCommentCommand : IRequest<OperationHandler>
    {
        public UpdateDTO UpdateDTO { get; set; }

        public UpdateCommentCommand(UpdateDTO updateDto)
        {
            UpdateDTO = updateDto;
        }
    }

    public class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, OperationHandler>
    {
        private readonly CommandDBContext _commandDb;
        private readonly QueryDBContext _queryDbContext;

        public UpdateCommentHandler(CommandDBContext commandDbContext , QueryDBContext queryDbContext)
        {
            _commandDb = commandDbContext;
            _queryDbContext = queryDbContext;
        }

        public async Task<OperationHandler> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var updateDTO = request.UpdateDTO;

            var userCommand = _commandDb.ProductComments.SingleOrDefault(x => x.Id == updateDTO.CommentId);
            var userQuery = _queryDbContext.ProductComments.SingleOrDefault(x => x.Id == updateDTO.CommentId);

            //var userCommand = _commandDb.ProductComments.SingleOrDefault(x => x.Id == updateDTO.CommentId);
            //var userQuery = _queryDbContext.ProductComments.SingleOrDefault(x => x.Id == updateDTO.CommentId);
            userCommand.Text = updateDTO.Text;
            userCommand.IsDeleted = updateDTO.IsDelete;
            userQuery.Text = updateDTO.Text;
            userQuery.IsDeleted = updateDTO.IsDelete;
            return OperationHandler.Success("We Updated your comment successfully!");
        }
    }
}
