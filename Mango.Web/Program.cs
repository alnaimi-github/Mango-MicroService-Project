using AspNetCoreHero.ToastNotification;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//notification
builder.Services.AddNotyf(config =>
    {
        config.DurationInSeconds = 10;
        config.IsDismissable = true;
        config.Position = NotyfPosition.BottomRight;
    }
);
// Register HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register HttpClient
builder.Services.AddHttpClient();

// Register named HttpClient for MangoAPI
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<ICartService, CartService>();

// Base URL
SD.ShoppingCartApiBase = builder.Configuration["ServiceUrls:ShoppingCartApi"];
SD.ProductApiBase = builder.Configuration["ServiceUrls:ProductApi"];
SD.CouponApiBase = builder.Configuration["ServiceUrls:CouponApi"];
SD.AuthApiBase = builder.Configuration["ServiceUrls:AuthApi"];

// Register BaseService and CouponService with DI
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(op =>
    {
     op.ExpireTimeSpan=TimeSpan.FromMinutes(10);
     op.LoginPath = "/Auth/Login";
     op.AccessDeniedPath = "/Home/AccessDenied";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
