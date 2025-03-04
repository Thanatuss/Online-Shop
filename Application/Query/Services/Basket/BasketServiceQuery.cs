using Application.Query.DTO.Basket;
using Application.Query.Services.Basket;
using Microsoft.EntityFrameworkCore;
using Persistance.DBContext;
using StackExchange.Redis;

public class BasketServiceQuery : IBasketServiceQuery
{
    private readonly CommandDBContext _commandDb;
    private static readonly ConnectionMultiplexer _redisConnection = ConnectionMultiplexer.Connect("127.0.0.1:6379");

    public BasketServiceQuery(CommandDBContext commandDb)
    {
        _commandDb = commandDb;
    }

    private IDatabase GetRedisDatabase()
    {
        return _redisConnection.GetDatabase();
    }

    public async Task<string> GetAll(GetAllDTO getAllDto)
    {
        var db = GetRedisDatabase();
        var user = _commandDb.Users.AsNoTracking().SingleOrDefault(x => x.Id == getAllDto.UserId);
        if (user == null) return "کاربر یافت نشد";

        var userBasket = db.HashGetAll($"user:{user.Id}").ToList();
        var itemDetails = userBasket.Select(item => $"{item.Name} ({item.Value})").ToList();
        var result = string.Join(", ", itemDetails);
        return result;
    }
}