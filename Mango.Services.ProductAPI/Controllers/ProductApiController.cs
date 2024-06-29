using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
	[Route("api/product")]
	[ApiController]
	public class ProductApiController: ControllerBase
	{
		private readonly AppDbContext _db;
		private  ResponseDto _response;
		private IMapper _mapper;
		public ProductApiController(AppDbContext db,IMapper mapper)
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
				IEnumerable<Product> objList = _db.Products!.ToList();
				_response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);

			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}

			return _response;
		}
        [Authorize(Roles = "ADMIN")]
        [HttpGet("GetById/{id:int}")]
	public ResponseDto GetById(int id)
		{
			try
			{
				var obj = _db.Products!.First(x=>x.ProductId==id);
				_response.Result = _mapper.Map<ProductDto>(obj);

			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}

			return _response;
		}

		[HttpPost("CreateNew")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] ProductDto productDto)
		{
			try
			{
				var obj = _mapper.Map<Product>(productDto);
				_db.Products!.Add(obj);
				_db.SaveChanges();
				_response.Result = _mapper.Map<ProductDto>(obj);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}

			return _response;
		}
		[HttpPut("Update")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] ProductDto couponDto)
		{
			try
			{
				var obj = _mapper.Map<Product>(couponDto);
				_db.Products!.Update(obj);
				_db.SaveChanges();
				_response.Result = _mapper.Map<ProductDto>(obj);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}

			return _response;
		}
		[HttpDelete("Delete/{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
		{
			try
			{
				var obj = _db.Products!.First(x => x.ProductId == id);
				_db.Products!.Remove(obj);
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
