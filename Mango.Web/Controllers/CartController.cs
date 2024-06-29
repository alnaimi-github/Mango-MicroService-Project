using AspNetCoreHero.ToastNotification.Abstractions;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly INotyfService _notyf;

        public CartController(ICartService cartService, INotyfService notyfService)
        {
            _cartService = cartService;
            _notyf = notyfService;
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        private async Task<CartDto?> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims.
                Where(u => u.Type == JwtRegisteredClaimNames.Sub)
                ?.FirstOrDefault()?.Value;
            var response = await _cartService.GetCartByUserIdAsync(userId!);
            if (response!=null && response.IsSuccess)
            {
                var cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result)!);
                return cartDto;
            }

            return new CartDto();
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.
                Where(u => u.Type == JwtRegisteredClaimNames.Sub)
                ?.FirstOrDefault()?.Value;
            var response = await _cartService.RemoveFromCartAsync(cartDetailsId);
            if(response != null && response.IsSuccess)
            {
                 _notyf.Success("Cart update successfully");
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var response = await _cartService.ApplyCouponAsync(cartDto);
            if (response != null && response.IsSuccess)
            {
                _notyf.Success("Cart update successfully");
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = "";
            var response = await _cartService.ApplyCouponAsync(cartDto);
            if (response != null && response.IsSuccess)
            {
                _notyf.Success("Cart update successfully");
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }
    }
}
