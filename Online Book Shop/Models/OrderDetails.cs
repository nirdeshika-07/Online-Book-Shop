using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Book_Shop.Models
{
    [Table("OrderDetails")]
    public class OrderDetails
    {
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int QuantityId { get; set; }
        public double UnitPrices { get; set; }
        public Order Order { get; set; }
        public Book Book{ get; set; }
    }
}
