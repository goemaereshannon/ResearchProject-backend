using ProductServices.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProductServices.Repositories
{
    public interface IProductRepo
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetAsyncByGuid(Guid Id);
        Task<IEnumerable<Product>> GetByExpressionAsync(Expression<Func<Product, bool>> expression);
    }
}