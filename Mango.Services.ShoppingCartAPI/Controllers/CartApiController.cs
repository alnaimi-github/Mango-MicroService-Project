using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using Mango.Services.ShoppingCartAPI.Service.IService;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private ResponseDto _response;
        private IMapper _mapper;

        public CartApiController(
            AppDbContext db,
            IMapper mapper,
            IProductService productService,
            ICouponService couponService)
        {
            _db = db;
            _response = new ResponseDto();
            _mapper = mapper;
            _productService = productService;
            _couponService = couponService;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                var cart = new CartDto()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(_db.CartHeaders!.First(u=>u.UserId==userId))

                };
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_db.CartDetails!
                    .Where(u=>u.CartHeaderId== cart.CartHeader.CartHeaderId));
                IEnumerable<ProductDto> productsDto = await _productService.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = productsDto!.FirstOrDefault(u => u.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product!.Price);
                }
                _response.Result = cart;
                //apply coupon if any
                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                    if (coupon !=null&&cart.CartHeader.CartTotal>coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.DisCount=coupon.DiscountAmount;
                    }
                }
            }
            catch (Exception e)
            {
                _response.Message = e.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartFromDb =await _db.CartHeaders!.FirstAsync(u => u.UserId == cartDto.CartHeader.UserId);
                cartFromDb.CouponCode = cartDto.CartHeader.CouponCode;
                _db.CartHeaders!.Update(cartFromDb);
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception e)
            {
                _response.Message = e.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }


        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                var cartFromDb = await _db.CartHeaders!.FirstAsync(u => u.UserId == cartDto.CartHeader.UserId);
                cartFromDb.CouponCode = "";
                _db.CartHeaders!.Update(cartFromDb);
                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception e)
            {
                _response.Message = e.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPost("CartUpdateAndInsert")]
        public async Task<ResponseDto> CartUpset(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb =
                    await _db.CartHeaders!.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb is null)
                {
                    //create header and details
                    var cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                  await  _db.CartHeaders!.AddAsync(cartHeader);
                  await _db.SaveChangesAsync();
                  cartDto.CartDetails!.First().CartHeaderId = cartHeader.CartHeaderId;
                  await _db.AddAsync(_mapper.Map<CartDetails>(cartDto.CartDetails!.First()));
                  await _db.SaveChangesAsync();
                }
                else
                {
                    //if header is not null 
                    //check if details has same product
                    var cartDetailsFromDb = 
                        await _db.CartDetails!.AsNoTracking().FirstOrDefaultAsync(u =>
                        u.ProductId == cartDto.CartDetails!.First()!.ProductId &&
                        u.CartHeaderId==cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb is null)
                    {
                        // create cart details
                        cartDto.CartDetails!.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        await _db.AddAsync(_mapper.Map<CartDetails>(cartDto.CartDetails!.First()));
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        // Update count in cart details
                        cartDto.CartDetails!.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails!.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails!.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                         _db.Update(_mapper.Map<CartDetails>(cartDto.CartDetails!.First()));
                        await _db.SaveChangesAsync();

                    }
                }

                _response.Result = cartDto;
            }
            catch (Exception e)
            {
                _response.Message=e.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }



        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody]int cartDetailsId)
        {
            try
            {
                var cartDetails = _db.CartDetails!
                    .First(u => u.CartDetailsId == cartDetailsId);
                var totalCountOfCartItem =
                    _db.CartDetails!.Where(u => u.CartDetailsId == cartDetails.CartDetailsId).Count();
                    _db.CartDetails!.Remove(cartDetails);
                if (totalCountOfCartItem == 1)
                {
                    var cartHeaderToRemove = await _db.CartHeaders!
                        .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);
                    _db.CartHeaders!.Remove(cartHeaderToRemove!);
                }

                await _db.SaveChangesAsync();
                _response.Result = true;
            }
            catch (Exception e)
            {
                _response.Message = e.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }
    }
}
