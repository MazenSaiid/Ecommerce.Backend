using Ecom.Backend.Core.Dtos;
using Ecom.Backend.Core.Entities;
using Ecom.Backend.Core.Sharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Backend.Core.Interfaces
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        //methods for product only
        Task<ReturnProductDto> GetAllAsync(ProductParams productParams);
        Task<bool> AddAsync(CreateProductDto productDto);
        Task<bool> UpdateAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> DeleteAsyncWithPicture(int id);
    }
}
