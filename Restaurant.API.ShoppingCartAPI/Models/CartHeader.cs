using System.ComponentModel.DataAnnotations;

namespace Restaurant.API.ShoppingCartAPI.Models
{
    public class CartHeader
    {
        [Key]
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }
        public string CoupanCode { get; set; }
    }
}
