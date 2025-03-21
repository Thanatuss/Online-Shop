﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.ProductDTO;
using Application.Command.Utilities;
using FluentValidation;
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
        private readonly ProductValidationService _productValidationService;
        private readonly IValidator<DeleteDTO> _validator;
        
        public DeleteProductHandler(IValidator<DeleteDTO> validator , CommandDBContext commandDbContext, ProductValidationService productValidationService, QueryDBContext queryDbContext)
        {
            _commandDbContext = commandDbContext;
            _productValidationService = productValidationService;
            _validator = validator;

        }

        public async Task<OperationHandler> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var deleteDTO = request.DeleteDTO;
            var product = _productValidationService.IsExistAnyViaId(deleteDTO);
            var IsInformationValidation = _validator.Validate(deleteDTO);
            if (IsInformationValidation.IsValid)
            {
                if (product != null)
                {
                    try
                    {

                        _commandDbContext.Products.Remove(product);
                        await _commandDbContext.SaveChangesAsync();
                        return OperationHandler.Success("We removed the product successfully!");
                    }
                    catch (Exception e)
                    {
                        return OperationHandler.Error($"An error occurred while deleting the product: {e}");
                    }
                }
                else
                {
                    return OperationHandler.Error("We could not find any product by this Id !");
                }
            }
            else
            {
                return OperationHandler.NotFound("ProductId must be greater than 0!");
            }
    
        }
        public class DeleteProductValidator : AbstractValidator<DeleteDTO>
        {
            public DeleteProductValidator()
            {
                RuleFor(x => x.ProductId)
                    .GreaterThan(0).WithMessage("ProductId must be greater than 0!");
            }
        }

    }
}
