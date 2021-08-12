using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class AdvertismentView
    {
        public int AdvId { get; private set; }
        public string AdvTitle { get; private set; }
        public int PrdId { get; private set; }
        [Display(Name = "Advertisment Text")]
        public string AdvTxt { get; private set; }
        [Display(Name = "From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DFrom { get; private set; }
        [Display(Name = "To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DTo { get; private set; }
        public int CatId { get; private set; }
        [Required]
        [Display(Name = "Category")]
        public string CatName { get; private set; }
        public string PrdName { get; private set; }
        [Display(Name = "Product")]
        public string NameId { get; private set; }
        [Display(Name = "Product Picture")]
        public string Pic { get; set; }



    }
}
