using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Backend.Core.Dtos
{
    public class BasketItemsDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductPicture { get; set; }
        [Required]
        [Range(1, double.MaxValue,ErrorMessage ="Price must be greater than 0")]
        public decimal Price { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Quantity must be greater than 0")]
        public int Quantity { get; set; }
        [Required]
        public string Category { get; set; }
    }
}
