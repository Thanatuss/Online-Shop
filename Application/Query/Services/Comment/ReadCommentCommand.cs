using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.ProductDTO;
using Domain.Comment;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.DBContext;

namespace Application.Query.Services.Comment
{
    public class ReadCommentCommand :IRequest<List<ProductComment>>
    {
        public AddDTO AddDTO { get; set; }

        public ReadCommentCommand(AddDTO addDTO)
        {
            AddDTO = addDTO;
        }
    }

    public class ReadCommentHandler : IRequestHandler<ReadCommentCommand, List<ProductComment>>
    {
        private readonly CommandDBContext _commandDb;

        public ReadCommentHandler(CommandDBContext commandDbContext)
        {
            _commandDb = commandDbContext;

        }


        public async Task<List<ProductComment>> Handle(ReadCommentCommand request, CancellationToken cancellationToken)
        {
            var list = _commandDb.ProductComments.AsNoTracking().Where(x => x.IsDeleted == false).ToList();
            return list;
        }
    }
}
