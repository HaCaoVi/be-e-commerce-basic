using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce_basic.Models
{
    [Table("SubCategories")]
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public required Category Category { get; set; }
        public List<Product> Products { get; set; } = [];

    }
}