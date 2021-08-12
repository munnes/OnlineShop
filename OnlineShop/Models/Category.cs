using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public partial class Category
    {
        [Key]
        public int CatId { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string CatName { get; set; }
        [Display(Name = "Description")]
        public string mainDesc { get; set; }

    }
}
