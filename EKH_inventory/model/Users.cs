using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKH_inventory.model
{
    public class Users
    {

        [Key]
        public int UID { get; set; }
        public string Username { get; set; }
        public string Upassword { get; set; }
        public string Uemail { get; set; }
        public string Urole { get; set; }
    }
}
