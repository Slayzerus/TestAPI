using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;

using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AlfavoxAPI.Models
{
    public class ProductContext : DbContext
    {


        public System.Data.Entity.DbSet<AlfavoxAPI.Models.Product> Products { get; set; }

        public ProductContext() : base("name=ProductContext"){}


    }

    public interface IProductService     
    {
        Task<Product> FindAsync(long id);
        Task<Product> AddAsync(Product model);

        Task<List<Product>> GetAllAsync();
    }

    public class ProductService : IProductService
    {
        private readonly ProductContext _dbContext;
        private readonly ILogger<ProductService> _logger;


        public ProductService(ProductContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public ProductService(ProductContext dbContext, ILogger<ProductService> logger)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> FindAsync(long id) 
        {
            return await _dbContext.Products.FindAsync(id);
        }

        public async Task<Product> AddAsync(Product model)
        {
            _dbContext.Products.Add(model);
            long id = await _dbContext.SaveChangesAsync();
            return model;
        }
    }
    
}
