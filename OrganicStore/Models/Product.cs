using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrganicStore.Models
{
    public class Product
    {
        public Product()
        {
            this.Categories = new HashSet<Category>();
               
        }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDetails { get; set; }
        public double ProductPrice { get; set; }
        public virtual ICollection<Category> Categories { get; set; }



    }
}