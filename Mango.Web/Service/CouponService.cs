using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
	public class CouponService: ICouponService
	{
		private readonly IBaseService _baseService;
		public CouponService(IBaseService baseService)
		{
			_baseService= baseService;
		}
		public async Task<ResponseDto?> GetCouponCodeAsync(string couponCode)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.GET,
				Url = SD.CouponApiBase + $"/api/coupon/GetByCode/{couponCode}"

			});
		}

		public async Task<ResponseDto?> GetAllCouponsAsync()
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.GET,
				Url = SD.CouponApiBase+ "/api/coupon/GetAll"

			});
		}

		public async Task<ResponseDto?> GetCouponByIdAsync(int id)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.GET,
				Url = SD.CouponApiBase + $"/api/coupon/GetById/{id}"

			});
		}

		public async Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.POST,
				Data = couponDto,
				Url = SD.CouponApiBase + $"/api/coupon/CreateNew"

			});
		}

		public async Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.PUT,
				Data = couponDto,
				Url = SD.CouponApiBase + $"/api/coupon/Update"

			});
		}

		public async Task<ResponseDto?> DeleteCouponAsync(int id)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.DELETE,
				Url = SD.CouponApiBase + $"/api/coupon/Delete/{id}"

			});
		}
	}
}
