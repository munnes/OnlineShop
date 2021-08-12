using Microsoft.AspNetCore.Http;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class ProductInShopViewModel
    {
        public int Trk { get; set; }

        [Display(Name = "Item Name")]
        public List<PrdByCat> Products;
        [Required]
        [Display(Name = "Category")]
        public int CatId { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "The Product field is required")]
        [Display(Name = "Product")]
        public int PrdId { get; set; }
        public List<Category> Categories;
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        [Display(Name = "Quantity")]
        public int Qty { get; set; }
        public int saveQty { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public double Price { get; set; }
        [Required]
        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateAdd { get; set; }

        public string Pic { get; set; }
        [Display(Name = "Product Picture")]
        public IFormFile File { get; set; }

    }
}
