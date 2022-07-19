using Restaurant.API.ShoppingCartAPI.Models.Dto;

namespace Restaurant.API.ShoppingCartAPI.Models.Dto
{
    public class CartDto
    {
        public CartHeaderDto Header { get; set; }
        public IEnumerable<CartDetailsDto> CartDetails { get; set; }
    }
}
