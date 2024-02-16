using System;


namespace SharedLibrary
{
    public class AddToCartMessage
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int UserId { get; set; }
    }

    public class OrderMessage
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int UserId { get; set; }
        public int Price { get; set; } = 0;
    }
}