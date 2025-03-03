using Application.Command.DTO.Basket;
using Application.Command.Services.Basket;
using Application.Command.Utilities;
using Infrastructore.Interfaces;
using Persistance.DBContext;
using StackExchange.Redis;

public class BasketService : IBasketService
{
    private readonly CommandDBContext _commandDbContext;
    private static readonly ConnectionMultiplexer _redisConnection = ConnectionMultiplexer.Connect("127.0.0.1:6379");

    public BasketService(CommandDBContext commandDb)
    {
        _commandDbContext = commandDb;
    }

    private IDatabase GetRedisDatabase()
    {
        return _redisConnection.GetDatabase();
    }

    public async Task<OperationHandler> AddAsync(BasketDTO basketDto)
    {
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
