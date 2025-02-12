namespace Commerce.EntityLayer.Models
{
    public class ProductCart
    {
        public int ProductCartID { get; set; }
        public int CartID { get; set; }
        public Cart Cart { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
