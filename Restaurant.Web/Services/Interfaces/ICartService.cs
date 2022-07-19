using Restaurant.Web.Models;

namespace Restaurant.Web.Services.Interfaces
{
    public interface ICartService
    {
        Task<T> GetCartAsnyc<T>();
        Task<T> AddToCartAsync<T>(CartDto cartDto);
        Task<T> UpdateCartAsync<T>(CartDto cartDto);
        Task<T> RemoveFromCartAsync<T>(int cartId);
    }
}
