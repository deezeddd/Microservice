using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Model
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public int Price { get; set; }

        public string Category { get; set; }

    }
}
