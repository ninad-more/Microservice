namespace Restaurant.API.ShoppingCartAPI.Models
{
    public class Cart
    {
        public CartHeader Header { get; set; }
        public IEnumerable<CartDetails> CartDetails { get; set; }
    }
}
