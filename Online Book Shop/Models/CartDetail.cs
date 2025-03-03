using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Book_Shop.Models
{
    [Table("CartDetails")]
    public class CartDetail
    {
        public int Id { get; set; }
        [Required]
        public int ShoppingCartId{get;set;}
        [Required]
        public int BookId{get;set;}
        [Required]
        public int Quantity{get;set;}
        public Book Book{get;set;}
        public ShoppingCart ShoppingCart { get; set; }
    }
}
