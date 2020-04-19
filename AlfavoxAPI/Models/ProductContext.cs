using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AlfavoxAPI.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext() : base("name=ProductContext")
        {
        }

        public System.Data.Entity.DbSet<AlfavoxAPI.Models.Product> Products { get; set; }
    }
}
