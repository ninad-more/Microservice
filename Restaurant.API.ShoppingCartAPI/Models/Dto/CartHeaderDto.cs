using System.ComponentModel.DataAnnotations;

namespace Restaurant.API.ShoppingCartAPI.Models.Dto
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }
        public string CoupanCode { get; set; }
    }
}
