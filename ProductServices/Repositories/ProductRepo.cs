using Microsoft.EntityFrameworkCore;
using ProductServices.Data;
using ProductServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.Repositories
{
    public class ProductRepo: GenericRepo<Product>, IProductRepo
    {
        public ProductServicesContext context { get; set; }
        public ProductRepo(ProductServicesContext _context): base(_context)
        {
            this.context = _context; 
        }
        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Price)
                .Include(p => p.Subcategory).ThenInclude(s => s.Category)
                .Include(p=> p.ProductHasProperties).ThenInclude(p=>p.Property).ThenInclude(p=> p.PropertyValues)
                .Include(p=>p.ProductHasSizes).ThenInclude(p=> p.Size)
                .OrderBy(p => p.CreationDate)
                .ToListAsync(); 
        }

        public override async Task<Product> GetAsyncByGuid(Guid Id)
        {
            return await _context.Set<Product>().Include(p => p.Price)
                .Include(p => p.Subcategory).ThenInclude(s => s.Category)
                .Include(p => p.ProductHasProperties).ThenInclude(p => p.Property).ThenInclude(p => p.PropertyValues)
                .Include(p => p.ProductHasSizes).ThenInclude(p => p.Size).FirstOrDefaultAsync(pr => pr.Id == Id); 
        }
    }
}
