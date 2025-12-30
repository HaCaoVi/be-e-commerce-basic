using e_commerce_basic.Database;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_basic.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateStockAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            return stock;
        }

        public async Task<bool> UpdateAsync(int productId, int quantity)
        {
            await _context.Stocks.Where(s => s.ProductId == productId).ExecuteUpdateAsync(s =>
            s.SetProperty(x => x.Quantity, quantity)
            );
            return true;
        }
    }
}