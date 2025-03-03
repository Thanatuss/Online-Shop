using Application.Command.DTO.ProductDTO;
using Application.Command.Utilities;

namespace Application.Command.Services.Product
{
    public interface IProductService
    {
        Task<OperationHandler> Add<T>(T dto) where T : AddDTO;
        Task<OperationHandler> Update<T>(T dto) where T : UpdateDTO;
        Task<OperationHandler> Delete<T>(T dto) where T : DeleteDTO;
    }
}
