using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.Basket;
using Microsoft.EntityFrameworkCore;
using Persistance.DBContext;
using StackExchange.Redis;

namespace Application.Command.Services.Basket
{
    public class BasketValidations
    {
        private static readonly ConnectionMultiplexer _redisConnection = ConnectionMultiplexer.Connect("127.0.0.1:6379");
        private readonly CommandDBContext _commandDb;
        public BasketValidations(CommandDBContext commandDB)
        {
            _commandDb = commandDB;
        }
        private IDatabase GetRedisDatabase()
        {
            return _redisConnection.GetDatabase();
        }
        public async Task<bool> IsProductExist(int productID)
        {
            return await _commandDb.Products.AsNoTracking().AnyAsync(x => x.ProductId == productID);
        }
        public async Task<bool> IsUserExist(int UserId)
        {
            try
            {
                var data = _commandDb.Users.Where(x => x.IsDeleted == false).ToList();
                return await _commandDb.Users.AsNoTracking().AnyAsync(x => x.Id == UserId);
            }
            catch (Exception ex)
            {
                // ثبت خطا
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
