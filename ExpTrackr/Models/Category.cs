using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ExpTrackr.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
