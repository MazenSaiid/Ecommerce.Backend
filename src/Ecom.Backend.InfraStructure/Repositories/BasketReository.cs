using AutoMapper;
using Ecom.Backend.Core.Dtos;
using Ecom.Backend.Core.Entities;
using Ecom.Backend.Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Ecom.Backend.InfraStructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        private readonly IMapper _mapper;

        public BasketRepository(IConnectionMultiplexer redisConnection, IMapper mapper)
        {
            _database = redisConnection.GetDatabase();
            _mapper = mapper;
        }
        public async Task<bool> DeleteCustomerBasketAsync(string basketId)
        {
            var basket = await _database.KeyExistsAsync(basketId);
           if (basket)
                return await _database.KeyDeleteAsync(basketId);
           return false;
        }

        public async Task<CustomerBasket> GetCustomerBasket(string basketId)
        {
           var data = await _database.StringGetAsync(basketId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateCustomerBasketAsync(CustomerBasket customerBasket)
        {
            var basket = await _database.StringSetAsync(customerBasket.Id,
            JsonSerializer.Serialize(customerBasket),TimeSpan.FromDays(30));

            return basket ? await GetCustomerBasket(customerBasket.Id) : null;
        }
    }
}
