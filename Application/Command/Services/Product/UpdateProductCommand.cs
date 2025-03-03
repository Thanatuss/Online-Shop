using Application.Command.DTO.ProductDTO;
using Application.Command.Services.Product;
using Application.Command.Utilities;
using MediatR;
using Persistance.DBContext;

public class UpdateProductCommand : IRequest<OperationHandler>
{
    public ProductUpdateDTO UpdateDTO { get; set; }

    public UpdateProductCommand(ProductUpdateDTO updateDto)
    {
        UpdateDTO = updateDto;
    }
}

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, OperationHandler>
{
    private readonly CommandDBContext _commandDbContext;
    private readonly ProductValidationService _productValidationService;

    public UpdateProductHandler(CommandDBContext commandDbContext, ProductValidationService productValidationService, QueryDBContext queryDbContext)
    {
        _commandDbContext = commandDbContext;
        _productValidationService = productValidationService;
    }

    public async Task<OperationHandler> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var updateDTO = request.UpdateDTO;  // اصلاح نام متغیر
        var productCommand = _commandDbContext.Products.SingleOrDefault(x => x.ProductId == updateDTO.ProductId);

        // بررسی اگر محصول موجود نباشد
        if ( productCommand == null)
        {
            return OperationHandler.Error("Product not found!");
        }

        try
        {


            productCommand.Description = updateDTO.Description;
            productCommand.Price = updateDTO.Price;
            productCommand.IsActive = updateDTO.IsActive;
            productCommand.ProductName = updateDTO.ProductName;
            await _commandDbContext.SaveChangesAsync(cancellationToken);

            return OperationHandler.Success("We updated your information!");
        }
        catch (Exception e)
        {
            return OperationHandler.Error($"An error occurred while updating the product: {e.Message}");
        }
    }
}