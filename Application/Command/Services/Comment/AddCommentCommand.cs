using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.CommentDTO;
using Application.Command.Utilities;
using Domain.Comment;
using FluentValidation;
using MediatR;
using Persistance.DBContext;

namespace Application.Command.Services.Comment
{
    public class AddCommentCommand : IRequest<OperationHandler>
    {
        public AddCommentDTO AddCommentDTO { get; set; }

        public AddCommentCommand(AddCommentDTO addComment)
        {
            AddCommentDTO = addComment;
        }
    }

    public class AddCommentHandler : IRequestHandler<AddCommentCommand, OperationHandler>
    {
        private readonly CommandDBContext _commandDb;
        private readonly IValidator<AddCommentDTO> _validator;
        public AddCommentHandler(IValidator<AddCommentDTO> validator ,CommandDBContext command, QueryDBContext queryDb)
        {
            _commandDb = command;
            _validator = validator;
        }
        public async Task<OperationHandler> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var addCommentDTO = request.AddCommentDTO;

            var validationResult = await _validator.ValidateAsync(addCommentDTO);

            var user = _commandDb.Users.SingleOrDefault(x => x.Id == addCommentDTO.UserID);
            var product = _commandDb.Products.SingleOrDefault(x => x.Id == addCommentDTO.ProductID);
            var data = _commandDb.ProductComments.Add(new ProductComment()
            {
                Product= product,
                User = user,
                IsDeleted = false ,
                Text = addCommentDTO.Text
                
            });
            await _commandDb.SaveChangesAsync();
            
            return OperationHandler.Success("We posted your comment successfully!");

        }
    }


}
    