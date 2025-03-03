using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.Basket;
using Application.Command.DTO.ProductDTO;
using Application.Command.Services.Product;
using Application.Command.Utilities;
using MediatR;
using Persistance.DBContext;
using StackExchange.Redis;

namespace Application.Command.Services.Basket
{
    public class AddBasketCommand : IRequest<OperationHandler>
    {
        public BasketDTO BasketDTO { get; }

        public AddBasketCommand(BasketDTO basketDto)
        {
            BasketDTO = basketDto;
        }
    }

    public class AddBasketHandler : IRequestHandler<AddBasketCommand, OperationHandler>
    {
        private readonly CommandDBContext _commandDbContext;
        private static readonly ConnectionMultiplexer _redisConnection = ConnectionMultiplexer.Connect("127.0.0.1:6379");

        public AddBasketHandler(CommandDBContext commandDb)
        {
            _commandDbContext = commandDb;
        }
        private IDatabase GetRedisDatabase()
        {
            return _redisConnection.GetDatabase();
        }
        public async Task<OperationHandler> Handle(AddBasketCommand request, CancellationToken cancellationToken)
        {
            var basketDto = request.BasketDTO;
            try
            {
                if (basketDto.UserID <= 0 || basketDto.ProductID <= 0)
                {
                    return OperationHandler.Error("UserID یا ProductID نامعتبر است");
                }

                var db = GetRedisDatabase();
                var redisKey = $"User-{basketDto.UserID}";
                var productField = $"Product-{basketDto.ProductID}";
                var existingQuantity = await db.HashGetAsync(redisKey, productField);

                if (existingQuantity.HasValue)
                {
                    if (int.TryParse(existingQuantity.ToString(), out int currentQty))
                    {
                        var newQuantity = currentQty + 1;
                        await db.HashSetAsync(redisKey, productField, newQuantity);
                        await db.KeyExpireAsync(redisKey, TimeSpan.FromMinutes(20)); // Fix: Added semicolon
                        return OperationHandler.Success("تعداد محصول به روز شد");
                    }
                    else
                    {
                        await db.HashSetAsync(redisKey, productField, 1);
                        await db.KeyExpireAsync(redisKey, TimeSpan.FromMinutes(20)); // Fix: Added semicolon
                        return OperationHandler.Success("مقدار نامعتبر بود، تعداد جدید تنظیم شد");
                    }
                }
                else
                {
                    await db.HashSetAsync(redisKey, productField, 1);
                    await db.KeyExpireAsync(redisKey, TimeSpan.FromMinutes(20)); // Added TTL for the first time
                    return OperationHandler.Success("محصول به سبد اضافه شد");
                }
            }
            catch (RedisConnectionException ex)
            {
                Console.WriteLine($"Redis Error: {ex.Message}");
                return OperationHandler.Error("خطای ارتباط با Redis");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"خطا: {ex.Message}");
                return OperationHandler.Error("خطای داخلی سرور");
            }
        }
    }
}
