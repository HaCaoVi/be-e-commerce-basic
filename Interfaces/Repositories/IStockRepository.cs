using e_commerce_basic.Models;

namespace e_commerce_basic.Interfaces
{
    public interface IStockRepository
    {
        Task<Stock> CreateStockAsync(Stock stock);
        Task<bool> UpdateAsync(int productId, int quantity);
    }
}