namespace Restaurant.Web.Models
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }
        public string CoupanCode { get; set; }
        public double OrderTotal { get; set; }
    }
}
