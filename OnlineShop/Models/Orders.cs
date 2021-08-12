using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Orders
    {
        [Key]
        public int OrdId { get; set; }
        public string UsrId { get; set; }
        public double Total { get; set; }
        public DateTime ordDate { get; set; }
    }
}
