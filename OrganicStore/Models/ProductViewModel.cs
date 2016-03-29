using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganicStore.Models
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDetails { get; set; }
        public double? ProductPrice { get; set; }
        public IQueryable<SelectListItem> Categories { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }

        
    }
}