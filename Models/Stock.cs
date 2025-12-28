using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce_basic.Models
{
    [Table("Stocks")]
    public class Stock
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Sold { get; set; } = 0;
        [Required]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}