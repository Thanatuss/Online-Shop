using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.Category;
using Application.Command.Utilities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance.DBContext;

namespace Application.Command.Services.Category
{
    public class DeleteCategoryCommand : IRequest<OperationHandler>
    {
        public DeleteCategoryDTO DeleteCategoryDTO { get; set; }
        public DeleteCategoryCommand(DeleteCategoryDTO deleteCategoryDTO)
        {
            DeleteCategoryDTO = deleteCategoryDTO;
        }
    }
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, OperationHandler>
    {
        private readonly CommandDBContext _commandDb;

        public DeleteCategoryHandler(CommandDBContext command, QueryDBContext queryDb)
        {
            _commandDb = command;
        }
        public async Task<OperationHandler> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var deleteDTO = request.DeleteCategoryDTO;
            var category = await _commandDb.Categorys.SingleOrDefaultAsync(x => x.Id == deleteDTO.CategoryId);
            if(category != null)
            {
                _commandDb.Categorys.Remove(category);
                _commandDb.SaveChanges();
                return OperationHandler.Success("We Removed The Category");
            }
            return OperationHandler.Error("We Could Not Removed The Category");
        }
    }
}
