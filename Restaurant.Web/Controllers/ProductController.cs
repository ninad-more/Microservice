using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Web.Models;
using Restaurant.Web.Services.Interfaces;

namespace Restaurant.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> list = new();
            var response = await _productService.GetAllProductsAsync<ResponseDto>();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }

            return View(list);
        }       

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateProductAsync<ResponseDto>(productDto);

                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(productDto);
        }

        public async Task<IActionResult> Edit(int productId)
        {
            if (productId > 0)
            {
                var response = await _productService.GetProductAsync<ResponseDto>(productId);

                if (response != null && response.IsSuccess)
                {
                    var dto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                    return View(dto);
                }

                return NotFound();
            }


            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateProductAsync<ResponseDto>(productDto);

                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(productDto);
        }

        public async Task<IActionResult> Delete(int productId)
        {
            if (productId > 0)
            {
                var response = await _productService.GetProductAsync<ResponseDto>(productId);

                if (response != null && response.IsSuccess)
                {
                    var dto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                    return View(dto);
                }

                return NotFound();
            }


            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductDto productDto)
        {
            var response = await _productService.DeleteProductAsync<ResponseDto>(productDto.ProductId);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(productDto);
        }
    }
}
