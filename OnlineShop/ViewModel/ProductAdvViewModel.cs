using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class ProductAdvViewModel
    {
        public int AdvId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The Product field is required")]
        [Display(Name = "Product")]
        public int PrdId { get; set; }
        [Display(Name = "Item Name")]
        public List<AvlProduct> Products;
        [Required]
        [Display(Name = "Category")]
        public int CatId { get; set; }
        public List<AvlCategory> Categories;
        [Display(Name = "Advertisment Title")]
        public string AdvTitle { get; set; }
        [Display(Name = "Advertisment Text")]
        public string AdvTxt { get; set; }
        [Display(Name = "From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DFrom { get; set; }
        [Display(Name = "To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DTo { get; set; }
  
    }
}
