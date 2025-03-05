namespace Online_Book_Shop.Models.DTOs
{
    public class OrderDetailModelDTO
    {
        public string DivId { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
