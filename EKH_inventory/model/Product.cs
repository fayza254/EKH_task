using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKH_inventory.model
{
    public class Product
    {
        [Key]
        public int PID { get; set; }
        public string Pname { get; set; }
        public string Pdescription { get; set; }
        public int Pquantity { get; set; }
        public int Price { get; set; }

        public int PSID { get; set; }

        [ForeignKey("PSID")]
        public virtual Supplier Supplier { get; set; }


        [NotMapped]
        public string Sname => Supplier?.Sname;

        [NotMapped]
        public int SID => Supplier?.SID ?? 0;
    }
}
