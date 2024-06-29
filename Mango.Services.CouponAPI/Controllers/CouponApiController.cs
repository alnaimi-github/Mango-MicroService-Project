using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
	[Route("api/coupon")]
	[ApiController]
	[Authorize]
	public class CouponApiController: ControllerBase
	{
		private readonly AppDbContext _db;
		private  ResponseDto _response;
		private IMapper _mapper;
		public CouponApiController(AppDbContext db,IMapper mapper)
		{
			_db = db;
			_response = new ResponseDto();
			_mapper = mapper;
		}
		[HttpGet("GetAll")]
		public ResponseDto Get()
		{
			try
			{
				IEnumerable<Coupon> objList = _db.Coupons!.ToList();
				_response.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);

			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}

			return _response;
		}

		[HttpGet("GetById/{id:int}")]

	public ResponseDto GetById(int id)
		{
			try
			{
				var obj = _db.Coupons!.First(x=>x.CouponId==id);
				_response.Result = _mapper.Map<CouponDto>(obj);

			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}

			return _response;
		}
		[HttpGet("GetByCode/{code}")]

		public ResponseDto GetByCode(string code)
		{
			try
			{
				var obj = _db.Coupons!.First(x => x.CouponCode.ToLower() == code.ToLower());
				_response.Result = _mapper.Map<CouponDto>(obj);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}

			return _response;
		}
		[HttpPost("CreateNew")]

		public ResponseDto Post([FromBody] CouponDto couponDto)
		{
			try
			{
				var obj = _mapper.Map<Coupon>(couponDto);
				_db.Coupons!.Add(obj);
				_db.SaveChanges();
				_response.Result = _mapper.Map<CouponDto>(obj);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}

			return _response;
		}
		[HttpPut("Update")]

		public ResponseDto Put([FromBody] CouponDto couponDto)
		{
			try
			{
				var obj = _mapper.Map<Coupon>(couponDto);
				_db.Coupons!.Update(obj);
				_db.SaveChanges();
				_response.Result = _mapper.Map<CouponDto>(obj);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}

			return _response;
		}
		[HttpDelete("Delete/{id:int}")]

		public ResponseDto Delete(int id)
		{
			try
			{
				var obj = _db.Coupons!.First(x => x.CouponId == id);
				_db.Coupons!.Remove(obj);
				_db.SaveChanges();
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}

			return _response;
		}

	}
}
