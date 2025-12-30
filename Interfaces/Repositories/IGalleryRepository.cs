using e_commerce_basic.Models;

namespace e_commerce_basic.Interfaces
{
    public interface IGalleryRepository
    {
        Task<List<Gallery>> CreateAsync(List<Gallery> listGallery);
        Task<List<Gallery>> GetListByProductIdAsync(int productId);
        Task<bool> UpdateAsync(int productId, List<Gallery> gallery);
    }
}