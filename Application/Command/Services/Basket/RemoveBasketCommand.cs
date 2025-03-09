using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Command.DTO.Basket;
using Application.Command.Services.Basket.Repo;
using Application.Command.Utilities;
using Domain.Entity;
using MediatR;
using Persistance.DBContext;
using StackExchange.Redis;

namespace Application.Command.Services.Basket
{
    public class DeleteBasketCommand : IRequest<OperationHandler>
    {
        public DeleteBasketDTO DeleteBasketDTO { get; }

        public DeleteBasketCommand(DeleteBasketDTO basketDto)
        {
            DeleteBasketDTO = basketDto;
        }
    }

    public class DeleteBasketHandler : IRequestHandler<DeleteBasketCommand, OperationHandler>
    {
        private readonly CommandDBContext _commandDbContext;
        private static readonly ConnectionMultiplexer _redisConnection = ConnectionMultiplexer.Connect("127.0.0.1:6379");
        private readonly BasketValidations _basketValidations;
        private readonly IRedisRepo _redis;

        public DeleteBasketHandler(IRedisRepo redis , CommandDBContext commandDb, BasketValidations basketValidations)
        {
            _commandDbContext = commandDb;
            _basketValidations = basketValidations;
            _redis = redis;

        }

        private IDatabase GetRedisDatabase()
        {
            return _redisConnection.GetDatabase();
        }

        public async Task<OperationHandler> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            var basketDto = request.DeleteBasketDTO;

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

                // 3. بررسی Redis و حذف محصول از سبد خرید
                var db = _redis.Connection();
                var redisKey = $"User-{basketDto.UserID}";
                var productField = $"Product-{basketDto.ProductID}";

                // بررسی وجود محصول در سبد خرید
                var existingQuantity = await db.HashGetAsync(redisKey, productField);

                if (!existingQuantity.HasValue)
                {
                    return OperationHandler.Error("محصولی برای حذف یافت نشد.");
                }

                // حذف محصول از سبد خرید
                await db.HashDeleteAsync(redisKey, productField);

                // انقضا 20 دقیقه‌ای پس از حذف محصول
                await db.KeyExpireAsync(redisKey, TimeSpan.FromMinutes(20));

                return OperationHandler.Success("محصول از سبد خرید حذف شد.");
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
