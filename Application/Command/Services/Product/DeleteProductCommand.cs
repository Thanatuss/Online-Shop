using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.ProductDTO;
using Application.Command.Utilities;
using MediatR;
using Persistance.DBContext;

namespace Application.Command.Services.Product
{
    public class DeleteProductCommand : IRequest<OperationHandler>
    {
        public DeleteDTO DeleteDTO { get; set; }

        public DeleteProductCommand(DeleteDTO deleteDTO)
        {
            DeleteDTO = deleteDTO;
        }

        

    }

    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, OperationHandler>
    {
        private readonly CommandDBContext _commandDbContext;
        private readonly QueryDBContext _queryDbContext;
        private readonly ProductValidationService _productValidationService;

        public DeleteProductHandler(CommandDBContext commandDbContext, ProductValidationService productValidationService, QueryDBContext queryDbContext)
        {
            _commandDbContext = commandDbContext;
            _productValidationService = productValidationService;
            _queryDbContext = queryDbContext;
        }

        public async Task<OperationHandler> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var deleteDTO = request.DeleteDTO;
            return OperationHandler.Success("All fields must be filled.");

        }
    }
}
