using e_commerce_basic.Dtos.Gallery;
using e_commerce_basic.Models;

namespace e_commerce_basic.Mappings
{
    public static class GalleryMapping
    {
        public static Gallery ToGalleryEntity(this CreateGalleryDto createGalleryDto, int productId)
        {
            return new Gallery
            {
                Size = createGalleryDto.Size,
                Url = createGalleryDto.Url,
                ContentType = createGalleryDto.ContentType,
                FileName = createGalleryDto.FileName,
                ProductId = productId
            };
        }

        public static List<Gallery> ToGalleryEntities(this List<CreateGalleryDto> dtos, int productId)
        {
            return dtos.Select(x => x.ToGalleryEntity(productId)).ToList();
        }
    }
}