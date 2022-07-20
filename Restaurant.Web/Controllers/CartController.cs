using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Web.Models;
using Restaurant.Web.Services.Interfaces;

namespace Restaurant.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public CartController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await LoadCart());
        }

        private async Task<CartDto> LoadCart()
        {
            var userId = "x";
            var accessToken = "y";
            var response = await _cartService.GetCartAsnyc<ResponseDto>();

            CartDto cartDto = new();

            if (response != null && response.IsSuccess)
            {
                cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            }

            if (cartDto.Header != null)
            {
                foreach (var detail in cartDto.CartDetails)
                {
                    cartDto.Header.OrderTotal += (detail.Product.Price * detail.Count);
                }
            }

            return cartDto;
        }

        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCart());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            try
            {
                var response = await _cartService.Checkout<ResponseDto>(cartDto.Header);
                if (!response.IsSuccess)
                {
                    TempData["Error"] = response.DisplayMessage;
                    return RedirectToAction(nameof(Checkout));
                }
                
                return RedirectToAction(nameof(Confirmation));
            }
            catch (Exception e)
            {
                return View(cartDto);
            }
        }

        public async Task<IActionResult> Confirmation()
        {
            return View();
        }
    }
}
