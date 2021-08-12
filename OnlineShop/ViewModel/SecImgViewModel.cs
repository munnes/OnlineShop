using Microsoft.AspNetCore.Http;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class SecImgViewModel
    {
        public int ImgId { get; set; }
        public List<SecondaryImg> SecondaryImg { get; set; }
        public string SecPic { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The Product field is required")]
        public int PrdId { get; set; }
        public int CatId { get; set; }
        public List<Product> Product { get; set; }
        public List<Category> Category { get; set; }
        [Display(Name = "Product Picture")]
        public IFormFile File { get; set; }
    }
}
