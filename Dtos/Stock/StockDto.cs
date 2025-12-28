namespace e_commerce_basic.Dtos.Stock
{
    public class StockDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; }
        public int ProductId { get; set; }
    }
}