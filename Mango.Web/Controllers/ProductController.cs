using System.Net;
using AspNetCoreHero.ToastNotification.Abstractions;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly INotyfService _notyf;

        public ProductController(IProductService productService, INotyfService notyf)
        {
            _productService = productService;
            _notyf = notyf;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? list = new();
            var response = await _productService.GetAllProductsAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result)!);
            }
            else
            {
                if (response!.StatusCode == HttpStatusCode.Forbidden)
                {
                    _notyf.Error(response!.Message, 5);
                    return RedirectToAction("AccessDenied", "Home");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _notyf.Error(response!.Message, 5);
                    return RedirectToAction("Login", "Auth");
                }
            }
            return View(list);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.CreateProductAsync(model);
                if (response != null && response.IsSuccess)
                {
                    _notyf.Success("Product Created Successfully");
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    _notyf.Error(response!.Message);
                }
            }

            return View(model);
        }
        public async Task<IActionResult> ProductDelete(int productId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result)!);
                return View(model);
            }
            else
            {
                _notyf.Error(response!.Message);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDto model)
        {
            ResponseDto? response = await _productService.DeleteProductAsync(model.ProductId);
            if (response != null && response.IsSuccess)
            {
                _notyf.Success("Product Deleted Successfully");
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                _notyf.Error(response!.Message);
            }
            return View(model);
        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(productId);
            if (response != null && response.IsSuccess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result)!);
                return View(model);
            }
            else
            {
                _notyf.Error(response!.Message);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            ResponseDto? response = await _productService.UpdateProductAsync(productDto);
            if (response != null && response.IsSuccess)
            {
                _notyf.Success("Product Updated Successfully");
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                _notyf.Error(response!.Message);
            }
            return View(productDto);
        }
    }
}
