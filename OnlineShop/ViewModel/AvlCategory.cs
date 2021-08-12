using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class AvlCategory
    {
        public int CatId { get; private set; }
        [Required]
        [Display(Name = "Category")]
        public string CatName { get; private set; }
    }
}
