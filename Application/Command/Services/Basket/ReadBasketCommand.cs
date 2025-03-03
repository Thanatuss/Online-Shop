using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command.DTO.Basket;
using Application.Command.Utilities;
using Application.Query.DTO.Basket;
using MediatR;
using Persistance.DBContext;
using StackExchange.Redis;

namespace Application.Command.Services.Basket
{
    public class ReadBasketCommand : IRequest<OperationHandler>
    {
        public GetAllDTO GetAllDTO { get; }

        public ReadBasketCommand(GetAllDTO getAllDto)
        {
            GetAllDTO = getAllDto;
        }
    }

    public class ReadBasketHandler : IRequestHandler<ReadBasketCommand, OperationHandler>
    {
        private readonly CommandDBContext _commandDbContext;
        private static readonly ConnectionMultiplexer _redisConnection = ConnectionMultiplexer.Connect("127.0.0.1:6379");

        public ReadBasketHandler(CommandDBContext commandDb)
        {
            _commandDbContext = commandDb;
        }

        private IDatabase GetRedisDatabase()
        {
            return _redisConnection.GetDatabase();
        }

        public async Task<OperationHandler> Handle(ReadBasketCommand request, CancellationToken cancellationToken)
        {
            var basketDto = request.GetAllDTO;
            var db = GetRedisDatabase();
            var user = _commandDbContext.Users.SingleOrDefault(x => x.Id == basketDto.UserId);
            if (user == null)
            {
                return new OperationHandler
                {
                    Message = "کاربر یافت نشد"
                };
            }

            var userBasket = db.HashGetAll($"user:{user.Id}").ToList();
            var itemDetails = userBasket.Select(item => $"{item.Name} ({item.Value})").ToList();
            var result = string.Join(", ", itemDetails);

            return new OperationHandler
            {
                Message = result
            };
        }
    }
}
