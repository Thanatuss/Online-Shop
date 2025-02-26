using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.ProductDTO;
using Application.Command.Utilities;

namespace Application.Command.Services.Product
{
    public interface IProductService
    {
        Task<OperationHandler> Add(AddDTO addDTO);
        OperationHandler Update(UpdateDTO updateDTO);
        OperationHandler Delete(DeleteDTO deleteDTO);

    }
}
