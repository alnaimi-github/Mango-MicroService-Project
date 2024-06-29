using Mango.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Net;
using AspNetCoreHero.ToastNotification.Abstractions;
using Mango.Web.Service.IService;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Mango.Web.Controllers
{
	public class HomeController : Controller
	{
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly INotyfService _notyf;

        public HomeController(INotyfService notyf, IProductService productService, ICartService cartService)
        {
            _notyf = notyf;
            _productService = productService;
            _cartService= cartService;
        }

		public async Task<IActionResult> Index()
		{
            List<ProductDto>? list = new();
            var response = await _productService.GetAllProductsAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result)!);
            }
            else
            {
                 _notyf.Error(response!.Message, 5);
                
            }
            return View(list);
        }
        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto? model = new();
            var response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result)!);
            }
            else
            {
                _notyf.Error(response!.Message, 5);

            }
            return View(model);
        }
        public IActionResult Privacy()
		{
			return View();
		}
		[HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            CartDto cartDto = new()
            {
                CartHeader = new CartHeaderDto()
                {
                    UserId = User.Claims.
                        Where(u => u.Type == JwtRegisteredClaimNames.Sub)
                        ?.FirstOrDefault()?.Value
                }
            };
            var cartDetails = new CartDetailsDto()
            {
               Count = productDto.Count,
               ProductId = productDto.ProductId
            };
            List<CartDetailsDto> cartDetailsDto = new(){ cartDetails };
            cartDto.CartDetails= cartDetailsDto;
            var response = await _cartService.UpdateAndInsertCartAsync(cartDto);
            if (response != null && response.IsSuccess)
            {
                _notyf.Success("Item has been added to the Shopping Cart");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _notyf.Error(response!.Message, 5);

            }
            return View(productDto);

        }
	}
}
