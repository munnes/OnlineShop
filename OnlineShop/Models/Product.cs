using System;
using System.ComponentModel.DataAnnotations;

using System.Linq;

namespace OnlineShop.Models
{
    public partial class Product
    {
        [Key]
        public int PrdId { get; set; }
        [Required]
        [Display(Name = "Item Code")]
        public string PrdCode { get; set; }
        [Required]
        [Display(Name = "Item Name")]
        public string PrdName { get; set; }
        [Required]
        public int CatId { get; set; }

        public string Description { get; set; }
    }
}