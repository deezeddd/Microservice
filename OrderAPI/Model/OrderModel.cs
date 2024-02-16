using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.Model
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
   
        public int UserId { get; set; }

        public int Price { get; set; } = 0;
    }

    
}
