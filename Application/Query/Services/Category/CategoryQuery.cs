using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.Utilities;
using Application.Query.DTO.CategoryQuery;
using Domain.Category;
using MediatR;
using Persistance.DBContext;

namespace Application.Query.Services.Category
{
    public class ReadCategoryQueryCommand : IRequest<List<Domain.Category.Category>>
    {
        ReadCategoryDTOQuery CategoryDTOQuery { get; set; }
        public ReadCategoryQueryCommand(ReadCategoryDTOQuery categoryQuery)
        {
            CategoryDTOQuery = categoryQuery;
        }
    }
    public class ReadCategoryQueryHandle : IRequestHandler<ReadCategoryQueryCommand , List<Domain.Category.Category>>
    {
        private readonly CommandDBContext _commandDBContext;
        public ReadCategoryQueryHandle(CommandDBContext commandDBContext)
        {
            _commandDBContext = commandDBContext;
        }
        public async Task<List<Domain.Category.Category>> Handle(ReadCategoryQueryCommand readCategoryQueryCommand , CancellationToken cancellationToken)
        {
            var data = _commandDBContext.Categorys.Where(x => x.IsDeleted == false).ToList();
            return data; 
        }
    }
}
