using AutoMapper;
using Ecom.Backend.Core.Dtos;
using Ecom.Backend.Core.Entities;
using Ecom.Backend.Core.Interfaces;
using Ecom.Backend.Core.Sharing;
using Ecom.Backend.InfraStructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Backend.InfraStructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, IFileProvider fileProvider,
            IMapper mapper) : base(context)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;
        }
        
        public async Task<ReturnProductDto> GetAllAsync(ProductParams productParams)
        {
            var result = new ReturnProductDto();

            var allProducts = await _context.Products.
                Include(x=>x.Category)
                .AsNoTracking().ToListAsync();

            //searching by name

            if (!string.IsNullOrEmpty(productParams.Search))
                allProducts = allProducts.Where(x => x.Name.ToLower().Contains(productParams.Search.ToLower())).ToList();

            //filtering by category
            if (productParams.CategoryId is not null)
                allProducts = allProducts.Where(x=>x.CategoryId == productParams.CategoryId).ToList();

            //sorting
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                allProducts = productParams.Sort switch
                {
                    "PriceAsc" => allProducts.OrderBy(x => x.Price).ToList(),
                    "PriceDesc" => allProducts.OrderByDescending(x => x.Price).ToList(),
                    _ => allProducts.OrderBy(x => x.Name).ToList(),
                };
            }

            result.TotalItems = allProducts.Count();
            //paging
            allProducts = allProducts.Skip((productParams.PageSize) * (productParams.PageNumber - 1)).Take(productParams.PageSize).ToList();

            result.ProductDtos = _mapper.Map<List<ProductDto>>(allProducts);
            return result;
        }
        public async Task<bool> AddAsync(CreateProductDto productDto)
        {
            //Image Region
            var src = "";
            if(productDto.Image is not null) 
            {
                var root = "/images/products";
                var productName = $"{Guid.NewGuid}"+productDto.Image.FileName;
                if(!Directory.Exists("wwwroot"+root))
                {
                    Directory.CreateDirectory("wwwroot"+root);
                }
                src = root + productName;
                var picInfo = _fileProvider.GetFileInfo(src);
                var rootPath = picInfo.PhysicalPath;
                using (var filestream = new FileStream(rootPath,FileMode.Create))
                {
                   await productDto.Image.CopyToAsync(filestream);
                }
                
            }
            //Create New Product
            var product = _mapper.Map<Product>(productDto);
            product.ProductPicture = src;
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return true;


        }
        public async Task <bool> UpdateAsync(int id , UpdateProductDto updateProductDto)
        {
            var currentProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);
            if(currentProduct != null)
            {
                //Image Region

                var src = "";
                if (updateProductDto.Image is not null)
                {
                    var root = "/images/products";
                    var productName = $"{Guid.NewGuid}" + updateProductDto.Image.FileName;
                    if (!Directory.Exists("wwwroot" + root))
                    {
                        Directory.CreateDirectory("wwwroot" + root);
                    }
                    src = root + productName;
                    var picInfo = _fileProvider.GetFileInfo(src);
                    var rootPath = picInfo.PhysicalPath;
                    using (var filestream = new FileStream(rootPath, FileMode.Create))
                    {
                        await updateProductDto.Image.CopyToAsync(filestream);
                    }
                }
                //remove old picture
                if (!string.IsNullOrEmpty(currentProduct.ProductPicture))
                {
                    var picInfo = _fileProvider.GetFileInfo($"{currentProduct.ProductPicture}");
                    var rootPath = picInfo.PhysicalPath;
                    System.IO.File.Delete(rootPath);
                }

                //update product

                var product = _mapper.Map<Product>(updateProductDto);
                product.ProductPicture = src;
                product.Id = id;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return true;

            }
            return false;
            
        }
        public async Task<bool> DeleteAsyncWithPicture(int id)
        {
            var currentProduct = await _context.Products.FindAsync(id);
            if (currentProduct is not null)
            {

                //remove old picture
                if (!string.IsNullOrEmpty(currentProduct.ProductPicture))
                {
                    var picInfo = _fileProvider.GetFileInfo($"{currentProduct.ProductPicture}");
                    var rootPath = picInfo.PhysicalPath;
                    System.IO.File.Delete(rootPath);
                }
                _context.Remove(currentProduct);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
