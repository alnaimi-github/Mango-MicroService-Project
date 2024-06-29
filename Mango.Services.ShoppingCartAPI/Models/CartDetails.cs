using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI.Models
{
    public class CartDetails
    {
        [Key]
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        [ForeignKey(nameof(CartHeaderId))]
        public CartHeader CartHeader { get; set; } = default!;
        public int ProductId { get; set; }
        [NotMapped] 
        public ProductDto Product { get; set; } = default!;
        public int Count { get; set; }

    }
}
