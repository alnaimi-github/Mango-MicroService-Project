using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;
        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartApiBase + $"/api/cart/ApplyCoupon"

            });
        }

        public async Task<ResponseDto?> GetCartByUserIdAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ShoppingCartApiBase + $"/api/cart/GetCart/{userId}"

            });
        }

        public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDetailsId,
                Url = SD.ShoppingCartApiBase + $"/api/cart/RemoveCart"

            });
        }

        public async Task<ResponseDto?> UpdateAndInsertCartAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data =cartDto,
                Url = SD.ShoppingCartApiBase + $"/api/cart/CartUpdateAndInsert"

            });
        }
    }
}
