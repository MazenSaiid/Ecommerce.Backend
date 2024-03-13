using AutoMapper;
using Ecom.Backend.Core.Interfaces;
using Ecom.Backend.InfraStructure.Data.Context;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Backend.InfraStructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;
        private readonly IConnectionMultiplexer _redisConnection;

        public UnitOfWork(ApplicationDbContext context,IFileProvider fileProvider,
            IMapper mapper, IConnectionMultiplexer redisConnection)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;
            _redisConnection = redisConnection;
            ProductRepository = new ProductRepository(_context,_fileProvider,_mapper);
            CategoryRepository = new CategoryRepository(_context);
            BasketRepository = new BasketRepository(_redisConnection,mapper);
        }
        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository {get; }

        public IBasketRepository BasketRepository { get; }
    }
}
