using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.Basket;
using Application.Command.DTO.ProductDTO;
using Application.Command.Services.Basket.Repo;
using Application.Command.Services.Product;
using Application.Command.Utilities;
using Domain.ProductEntity;
using MediatR;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
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
        private static readonly ConnectionMultiplexer _redisConnection = ConnectionMultiplexer.Connect("127.0.0.1:6379");
        private readonly BasketValidations _basketValidations;
        private readonly IRedisRepo _redis;

        public AddBasketHandler(IRedisRepo reids ,  BasketValidations basketValidations)
        {
            _redis = reids;
            _basketValidations = basketValidations;
        }
        private IDatabase GetRedisDatabase()
        {
            return _redisConnection.GetDatabase();
        }
        public async Task<OperationHandler> Handle(AddBasketCommand request, CancellationToken cancellationToken)
        {
            var basketDto = request.BasketDTO;

            // 1. اعتبارسنجی UserID و ProductID
            if (basketDto.UserID <= 0)
            {
                return OperationHandler.Error("UserID باید بزرگتر از صفر باشد.");
            }
            if (basketDto.ProductID <= 0)
            {
                return OperationHandler.Error("ProductID باید بزرگتر از صفر باشد.");
            }

            try
            {
                // 2. بررسی موجود بودن کاربر و محصول
                var userExists = await _basketValidations.IsUserExist(basketDto.UserID);
                if (!userExists)
                {
                    return OperationHandler.Error("کاربری با این شناسه یافت نشد.");
                }

                var productExists = await _basketValidations.IsProductExist(basketDto.ProductID);
                if (!productExists)
                {
                    return OperationHandler.Error("محصولی با این شناسه یافت نشد.");
                }

                // 3. بررسی Redis و اضافه کردن محصول به سبد خرید
                var db = _redis.Connection();
                var redisKey = $"User-{basketDto.UserID}";
                var productField = $"Product-{basketDto.ProductID}";
                var existingQuantity = await db.HashGetAsync(redisKey, productField);

                // بررسی موجودیت محصول در سبد خرید
                if (existingQuantity.HasValue)
                {
                    if (int.TryParse(existingQuantity.ToString(), out int currentQty))
                    {
                        var newQuantity = currentQty + 1;
                        await db.HashSetAsync(redisKey, productField, newQuantity);
                        await db.KeyExpireAsync(redisKey, TimeSpan.FromMinutes(20)); // مدت زمان انقضا

                        return OperationHandler.Success("تعداد محصول به روز شد");
                    }
                    else
                    {
                        // اگر مقدار نامعتبر بود، مقدار جدید را تنظیم می‌کنیم
                        await db.HashSetAsync(redisKey, productField, 1);
                        await db.KeyExpireAsync(redisKey, TimeSpan.FromMinutes(20));
                        return OperationHandler.Success("مقدار نامعتبر بود، تعداد جدید تنظیم شد");
                    }
                }
                else
                {
                    // اگر محصول برای اولین بار اضافه می‌شود
                    await db.HashSetAsync(redisKey, productField, 1);
                    await db.KeyExpireAsync(redisKey, TimeSpan.FromMinutes(20)); // مدت زمان انقضا
                    return OperationHandler.Success("محصول به سبد اضافه شد");
                }
            }
            catch (RedisConnectionException ex)
            {
                // خطای ارتباط با Redis
                Console.WriteLine($"Redis Error: {ex.Message}");
                return OperationHandler.Error("خطای ارتباط با Redis. لطفاً بعداً امتحان کنید.");
            }
            catch (Exception ex)
            {
                // خطای داخلی سرور
                Console.WriteLine($"خطا: {ex.Message}");
                return OperationHandler.Error("خطای داخلی سرور. لطفاً بعداً امتحان کنید.");
            }

        }

    }
}
