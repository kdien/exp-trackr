using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpTrackr.Models.ViewModels
{
    public class CategoryViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        
        public int CategoryID { get; set; }

        //[Required(ErrorMessage = "Category name cannot be empty")]
        [StringLength(50, ErrorMessage = "Category name cannot be longer than 50 characters.")]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        public int UserID { get; set; }
    }
}
