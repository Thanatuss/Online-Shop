using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Application.Command.Services.Basket.Repo
{
    public class RedisRepo : IRedisRepo
    {
        private readonly string _connectionString;

        public RedisRepo(string connectionString = "127.0.0.1:6379")
        {
            _connectionString = connectionString;
        }

        public IDatabase Connection()
        {
            ConnectionMultiplexer redisConnection = ConnectionMultiplexer.Connect(_connectionString);
            return redisConnection.GetDatabase();
        }
    }
    public interface IRedisRepo
    {
        IDatabase Connection();
    }

}
