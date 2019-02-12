using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpTrackr.Models.ViewModels
{
    public class CategoryViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public int UserID { get; set; }
        public string CategoryName { get; set; }
    }
}
