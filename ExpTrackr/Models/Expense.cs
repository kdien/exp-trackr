using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int CategoryID { get; set; }

        [Display(Name = "Description")]
        public string ExpenseDescription { get; set; }

        [Display(Name = "Amount")]
        [DataType(DataType.Currency)]
        public decimal ExpenseAmount { get; set; }
    }
}
