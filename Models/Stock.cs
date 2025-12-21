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
        public int Sold { get; set; }
        [Required]
        public int ProductId { get; set; }
        public required Product Product { get; set; }
    }
}