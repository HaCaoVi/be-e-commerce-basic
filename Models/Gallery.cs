using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce_basic.Models
{
    [Table("Galleries")]
    public class Gallery
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(500)]
        public string FileName { get; set; } = null!;
        [Required]
        [MaxLength(500)]
        public string Url { get; set; } = null!;
        [Required]
        public string ContentType { get; set; } = null!;
        [Required]
        public long Size { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}