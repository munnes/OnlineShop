using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class ReviewsView
    {
    
        public int RId { get; private set; }
        public int PrdId { get; private set; }
        [Display(Name = "Overall Rating")]
        public int Stars { get; private set; }
        [Required]
        [Display(Name = " Add a headline")]
        public string RHead { get; private set; }
        [Required]
        [Display(Name = "Add a written review")]
        public string RText { get; private set; }
        public string UsrId { get; private set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RevDate { get;private set; }
        public string FullName { get;private set; }
    }
}
