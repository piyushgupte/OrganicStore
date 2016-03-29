using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OrganicStore.Models
{
    public class ProductCategory
    {
       
        public int ProductRefId { get; set; }
      
        public int CategoryRefId { get; set; }
        [Display(Name ="Product Name")]
        public string ProductName { get; set; }
        [Display(Name ="Category Name")]
        public string CategoryName { get; set; }


    }
}