namespace Restaurant.Web.Models
{
    public class CartDto
    {
        public CartHeaderDto Header { get; set; }
        public IEnumerable<CartDetailsDto> CartDetails { get; set; }
    }
}
