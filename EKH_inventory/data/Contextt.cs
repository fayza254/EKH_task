using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EKH_inventory.model;
using Microsoft.EntityFrameworkCore;
namespace EKH_inventory.data
{
    public class Contextt:DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Product> Product { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=COM165-LAB3\\SQLEXPRESS;Initial Catalog=TKH_inventory;Integrated Security=True");
        }
    }
}
