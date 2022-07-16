using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.ProductAPI.Database;
using Restaurant.API.ProductAPI.Models;
using Restaurant.API.ProductAPI.Models.Dto;
using Restaurant.API.ProductAPI.Repository.Interfaces;

namespace Restaurant.API.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private IMapper _mapper;

        public ProductRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
        {
            try
            {
                 Product product = _mapper.Map<ProductDto, Product>(productDto);

                if (product.ProductId > 0)
                {
                    _dbContext.Products.Update(product);
                }
                else
                {
                    _dbContext.Products.Add(product);
                }

                await _dbContext.SaveChangesAsync();

                return _mapper.Map<Product, ProductDto>(product);
            }
            catch(Exception ex)
            {
                
            }

            return null;
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId);

                if (product == null)
                {
                    return false;
                }

                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                
            }

            return false;
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            List<Product> productList = await _dbContext.Products.ToListAsync();

            return _mapper.Map<List<ProductDto>>(productList);
        }
    }
}
