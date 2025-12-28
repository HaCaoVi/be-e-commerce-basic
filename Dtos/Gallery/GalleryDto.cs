
namespace e_commerce_basic.Dtos.Gallery
{
    public class GalleryDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long Size { get; set; }
    }

}