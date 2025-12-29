using e_commerce_basic.Database;
using e_commerce_basic.Helpers;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_basic.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            await _context.AddAsync(product);
            return product;
        }

        public async Task<PagedResult<Product>> GetPagedProductsAsync(QueryObject query, CancellationToken cancellationToken)
        {
            var products = _context.Products.AsNoTracking()
                            .Where(p => !p.IsDeleted);

            if (query.SubCategoryId != 0)
            {
                products = products.Where(p => p.SubCategoryId == query.SubCategoryId);
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim();
                // Case-insensitive search chuẩn production
                products = products.Where(p =>
                    EF.Functions.Like(p.Code, $"%{search}%") ||
                    EF.Functions.Like(p.Name, $"{search}%"));
            }

            // Total count
            var totalCount = await products.CountAsync(cancellationToken);

            // Sorting
            IOrderedQueryable<Product> orderedProducts = query.SortBy?.ToLowerInvariant() switch
            {
                "code" => query.IsDescending ? products.OrderByDescending(p => p.Code) : products.OrderBy(p => p.Code),
                "name" => query.IsDescending ? products.OrderByDescending(p => p.Name) : products.OrderBy(p => p.Name),
                "price" => query.IsDescending ? products.OrderByDescending(p => p.Price) : products.OrderBy(p => p.Price),
                "createdat" => query.IsDescending ? products.OrderByDescending(p => p.CreatedAt) : products.OrderBy(p => p.CreatedAt),
                _ => products.OrderByDescending(p => p.CreatedAt)
            };

            // Pagination
            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var result = await orderedProducts
                .Skip(skipNumber)
                .Take(query.PageSize)
                .Include(p => p.Stock)
                .Include(p => p.Galleries)
                .ToListAsync(cancellationToken);

            return new PagedResult<Product>
            {
                Meta = new Meta
                {
                    PageNumber = query.PageNumber,
                    PageSize = query.PageSize,
                    TotalCount = totalCount
                },
                Result = result
            };
        }

        public async Task<bool> IsCodeExistAsync(string code)
        {
            var codeExist = await _context.Products.AnyAsync(p => p.Code == code);
            return codeExist;
        }
    }
}