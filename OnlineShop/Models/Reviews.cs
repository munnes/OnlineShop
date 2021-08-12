using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Reviews
    {
        [Key]
        public int RId { get; set; }
        public int PrdId { get; set; }
        [Display(Name="Overall Rating")]
        public int Stars { get; set; }
        [Required]
        [Display(Name = " Add a headline")]
        public string RHead { get; set; }
        [Required]
        [Display(Name = "Add a written review")]
        public string RText { get; set; }
        public string UsrId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RevDate { get; set; }
    }
}
