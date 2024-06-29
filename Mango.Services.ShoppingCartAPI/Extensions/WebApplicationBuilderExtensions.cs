using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Mango.Services.ShoppingCartAPI.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
        {

            var settingsSection = builder.Configuration.GetSection("ApiSettings");
            var secret = settingsSection.GetValue<string>("Secret");
            var issuer = settingsSection.GetValue<string>("Issuer");
            var audience = settingsSection.GetValue<string>("Audience");
            var key = Encoding.UTF8.GetBytes(secret!);
            builder.Services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, // You can set to true if you have issuer validation
                        ValidIssuer = issuer,
                        ValidateAudience = true, // You can set to true if you have audience validation
                        ValidAudience = audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });
            builder.Services.AddAuthorization();
            return builder;
        }
    }
}
