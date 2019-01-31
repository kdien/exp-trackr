using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExpTrackr.Models
{
    public class Budget
    {
        public int BudgetID { get; set; }

        public virtual User User { get; set; }
        public int UserID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Budget name cannot be longer than 50 characters.")]
        [Display(Name = "Name")]
        public string BudgetName { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal (18, 2)")]
        [Display(Name = "Limit")]
        public decimal BudgetMax { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Total")]
        public decimal BudgetTotal { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created On")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
