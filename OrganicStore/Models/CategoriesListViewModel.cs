using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganicStore.Models
{
    public class CategoriesListViewModel
    {

        public int CategoryId { get; set; }
        [Display(Name ="Category Name")]
        [Required]
        public string CategoryName { get; set; }
        [Display(Name ="Parent Category")]
        public string ParentCategory { get; set; }
        public int ParentCategoryId { get; set; }
        [Display(Name ="Category Details")]
        [Required]
        public string CategoryDetails { get; set; }

        
        public IEnumerable<SelectListItem> Categories { get; set; }


    }
}