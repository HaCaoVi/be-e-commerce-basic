using e_commerce_basic.Models;

namespace e_commerce_basic.Interfaces
{
    public interface IGalleryRepository
    {
        Task<List<Gallery>> CreateGalleryAsync(List<Gallery> listGallery);
    }
}