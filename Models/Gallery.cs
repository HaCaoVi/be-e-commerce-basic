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
        public required string ImageUrl { get; set; }
        [Required]
        public int ProductId { get; set; }
        public required Product Product { get; set; }
    }
}