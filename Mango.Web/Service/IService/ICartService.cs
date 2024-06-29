using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto?> GetCartByUserIdAsync(string userId);
        Task<ResponseDto?> UpdateAndInsertCartAsync(CartDto cartDto);
        Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId);
        Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto);
    }
}
