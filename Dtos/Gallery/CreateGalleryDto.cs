
using System.ComponentModel.DataAnnotations;

namespace e_commerce_basic.Dtos.Gallery
{
    public class CreateGalleryDto
    {
        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string Url { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string ContentType { get; set; } = null!;

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Size must be greater than 0")]
        public long Size { get; set; }
    }

}