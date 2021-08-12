
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class ProductCategoryViewModel
    {
        public int PrdId { get; set; }
        [Required]
        [Display(Name = "Item Code")]
        public string PrdCode { get; set; }
        [Required]
        [Display(Name = "Item Name")]

        public string PrdName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The Category field is required")]
        [Display(Name = "Category")]
        public int CatId { get; set; }
        public List<Category> Categories { get; set; }

        public string Description { get; set; }
        public string mainDesc { get; set; }
    }
}
