namespace Mango.Web.Utility
{
	public class SD
	{
		public static string? ShoppingCartApiBase { get; set; } = string.Empty;
		public static string? ProductApiBase { get; set; } = string.Empty;
		public static string? CouponApiBase { get; set; } = string.Empty;
		public static string? AuthApiBase { get; set; } = string.Empty;
		public const  string RoleAdmin  = "ADMIN";
		public const string RoleCustomer = "CUSTOMER";
		public const string TokenCookie = "JWTToken";
		public enum ApiType
		{
			GET,
			POST,
			PUT,
			DELETE
		}
	}
}
