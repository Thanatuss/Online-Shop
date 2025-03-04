using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.Category;
using Application.Command.Utilities;
using Domain.Category;
using MediatR;
using Persistance.DBContext;

namespace Application.Command.Services.Category
{
    public class AddCategoryCommand : IRequest<OperationHandler>
    {
        public AddCategoryDTO AddCategoryDTO { get; set; }
        public AddCategoryCommand(AddCategoryDTO addCategoryDTO)
        {
            AddCategoryDTO = addCategoryDTO;
        }
    }
    public class AddCategoryHandler : IRequestHandler<AddCategoryCommand , OperationHandler>
    {
        private readonly CommandDBContext _commandDBContext;
        public AddCategoryHandler(CommandDBContext commandDBContext)
        {
            _commandDBContext = commandDBContext;

        }
        public async Task<OperationHandler> Handle(AddCategoryCommand request , CancellationToken cancellationToken)
        {
            var addCategorydto = request.AddCategoryDTO;
            if (addCategorydto.ParentId == 0)
            {
                _commandDBContext.Categorys.Add(new Domain.Category.Category
                {
                    Description = addCategorydto.Description,
                    IsDeleted = false,
                    Name = addCategorydto.Name
                });
                _commandDBContext.SaveChanges();
                return OperationHandler.Success("We added a new Category!");
            }
            _commandDBContext.Categorys.Add(new Domain.Category.Category
            {
                Description = addCategorydto.Description,
                IsDeleted = false,
                Name = addCategorydto.Name,
            });
            _commandDBContext.SaveChanges();
            return OperationHandler.Success("We added a new Category!");
        }
    }
}
