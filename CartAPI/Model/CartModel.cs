using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CartAPI.Model
{
    public class CartModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string ProductName{ get; set; }

        [ForeignKey("ProductId ")]
        public int ProductId { get; set; }

        public int Quantity { get; set; } = 0;
    }
}
