using Restaurant.API.ShoppingCartAPI.Models.Dto;

namespace Restaurant.API.ShoppingCartAPI.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task<CartDto> GetCart();
        Task<CartDto> CreateUpdateCart(CartDto cart);
        Task<bool> RemoveFromCart(int cartDetailsId);
        Task<bool> ClearCart();
    }
}
