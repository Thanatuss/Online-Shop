using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        private readonly BasketValidations _basketValidations;

        public ReadBasketHandler(CommandDBContext commandDbContext , BasketValidations basketValidations)
        {
            _commandDbContext = commandDbContext;
            _basketValidations = basketValidations;
        }
        private IDatabase GetRedisDatabase()
        {
            return _redisConnection.GetDatabase();
        }

        public async Task<OperationHandler> Handle(ReadBasketCommand request, CancellationToken cancellationToken)
        {
            var basketDto = request.GetAllDTO;
            var db = GetRedisDatabase();
            var userExists =await _basketValidations.IsUserExist(basketDto.UserId);
            if (db == null)
            {
                return new OperationHandler { Message = "Failed to get Redis database." };
            }
            if (!userExists)
            {
                return new OperationHandler
                {
                    Message = "کاربری با این شناسه یافت نشد!"
                };
            }
            var redisKey = $"User-{basketDto.UserId}";
            var allBasketItems = await db.HashGetAllAsync(redisKey);
            if (allBasketItems == null)
            {
                return new OperationHandler { Message = "No basket items found in Redis." };
            }
            var basketDetails = allBasketItems.Select(item =>
                new BasketItemDTO
                {
                    ProductID = item.Name,
                    Quantity = item.Value
                }).ToList();
            if(basketDetails == null || !basketDetails.Any())
            {
                return new OperationHandler { Message = "سبد خرید خالی است." };
            }

            // بازگشت پیام موفقیت
            return new OperationHandler
            {
                Message = $"{basketDetails[0].ProductID} - {basketDetails[0].Quantity}"
            };
        }
    }

    public class BasketItemDTO
    {
        public string ProductID { get; set; }
        public string Quantity { get; set; }
    }
}
