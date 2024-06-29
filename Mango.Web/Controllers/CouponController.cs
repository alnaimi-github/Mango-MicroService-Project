using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Net;

namespace Mango.Web.Controllers
{
    
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        private readonly INotyfService _notyf;

        public CouponController(ICouponService couponService, INotyfService notyfService)
        {
            _couponService = couponService;
            _notyf = notyfService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list = new();
            var response = await _couponService.GetAllCouponsAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result)!);
            }
            else
            {
                if (response!.StatusCode == HttpStatusCode.Forbidden)
                {
                    _notyf.Error(response!.Message, 5);
                    return RedirectToAction("AccessDenied", "Home");
                }
                else if(response.StatusCode== HttpStatusCode.Unauthorized)
                {
                    _notyf.Error(response!.Message, 5);
                    return RedirectToAction("Login", "Auth");
                }
            }
            return View(list);
        }

        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.CreateCouponAsync(model);
                if (response != null && response.IsSuccess)
                {
                    _notyf.Success("Coupon Created Successfully");
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    _notyf.Error(response!.Message);
                }
            }

            return View(model);
        }
        public async Task<IActionResult> CouponDelete(int couponId)
        {
            ResponseDto? response = await _couponService.GetCouponByIdAsync(couponId);
                if(response != null && response.IsSuccess)
                {
                CouponDto? model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result)!);
                return View(model);
                }
                else
                {
                    _notyf.Error(response!.Message);
                }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto model)
        {
            ResponseDto? response = await _couponService.DeleteCouponAsync(model.CouponId);
            if (response != null && response.IsSuccess)
            {
                _notyf.Success("Coupon Deleted Successfully");
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                _notyf.Error(response!.Message);
            }
            return View(model);
        }

    }
}
