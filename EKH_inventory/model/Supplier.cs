using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKH_inventory.model
{
    public class Supplier
    {
        [Key]
        public int SID { get; set; }
        public string Sname { get; set; }
        public int Sphone { get; set; }
        public string Semail { get; set; }
    }
}
