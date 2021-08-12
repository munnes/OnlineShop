using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class PrchFRecView
    {
        public int TrkOut { get; set; }
        public int PrdId { get; set; }
        public bool InCart { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int IsCart { get; set; }
        public bool IsChkOut { get; set; }
        public string UsrId { get; set; }
        public int Qty { get; set; }
        public bool ActvOrd { get; set; }
        public double Price { get; set; }
        public DateTime ODate { get; set; }
    }
}
