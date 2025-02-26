using Application.Command.DTO.ProductDTO;
using Application.Command.Services.Product;
using Application.Command.Utilities;
using MediatR;
using Persistance.DBContext;

public class UpdateProductCommand : IRequest<OperationHandler>
{
    public UpdateDTO UpdateDTO { get; set; }

    public UpdateProductCommand(UpdateDTO updateDto)
    {
        UpdateDTO = updateDto;
    }
}

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, OperationHandler>
{
    private readonly CommandDBContext _commandDbContext;
    private readonly QueryDBContext _queryDbContext;
    private readonly ProductValidationService _productValidationService;

    public UpdateProductHandler(CommandDBContext commandDbContext, ProductValidationService productValidationService, QueryDBContext queryDbContext)
    {
        _commandDbContext = commandDbContext;
        _productValidationService = productValidationService;
        _queryDbContext = queryDbContext;
    }

    public async Task<OperationHandler> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var updateDTO = request.UpdateDTO;  // اصلاح نام متغیر
        var productQuery = _queryDbContext.Products.SingleOrDefault(x => x.ProductId == updateDTO.ProductId);
        var productCommand = _commandDbContext.Products.SingleOrDefault(x => x.ProductId == updateDTO.ProductId);

        // بررسی اگر محصول موجود نباشد
        if (productQuery == null || productCommand == null)
        {
            return OperationHandler.Error("Product not found!");
        }

        try
        {
            // به روزرسانی اطلاعات
            productQuery.Description = updateDTO.Description;
            productQuery.Price = updateDTO.Price;
            productQuery.IsActive = updateDTO.IsActive;
            productQuery.ProductName = updateDTO.ProductName;

            productCommand.Description = updateDTO.Description;
            productCommand.Price = updateDTO.Price;
            productCommand.IsActive = updateDTO.IsActive;
            productCommand.ProductName = updateDTO.ProductName;

            // ذخیره تغییرات در هر دو دیتابیس
            await _queryDbContext.SaveChangesAsync(cancellationToken);
            await _commandDbContext.SaveChangesAsync(cancellationToken);

            return OperationHandler.Success("We updated your information!");
        }
        catch (Exception e)
        {
            return OperationHandler.Error($"An error occurred while updating the product: {e.Message}");
        }
    }
}