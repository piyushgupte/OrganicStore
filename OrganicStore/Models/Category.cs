using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrganicStore.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required]
        [Display(Name ="Category Name")]
        public string CategoryName { get; set; }
        [Display(Name ="Parent Name")]
        public int PrarentId { get; set; }
        [Display(Name ="Category Details")]
        public string CategoryDetails { get; set; }
        public virtual ICollection<Product> Products { get; set; } 

    }
}