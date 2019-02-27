using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpTrackr.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ThankYouModel : PageModel
    {
        public IActionResult OnGetAsync()
        {
            return Page();
        }
    }
}