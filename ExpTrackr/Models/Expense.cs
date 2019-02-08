using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace ExpTrackr.Models
{
    public class Expense
    {
        public int ExpenseID { get; set; }

        public virtual Budget Budget { get; set; }
        public int BudgetID { get; set; }

        public virtual Category Category { get; set; }
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal (18, 2)")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}
