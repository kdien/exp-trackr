using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpTrackr.Data;
using ExpTrackr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ExpTrackr.Controllers
{
    [Authorize]
    public class BudgetController : Controller
    {
        private readonly ExpTrackrContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BudgetController(ExpTrackrContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Budget
        public async Task<IActionResult> Index()
        {
            var user = GetUser();

            if (user == null)
                return NotFound();

            var budgets = _context.Budgets.Where(b => b.UserID == user.UserID);

            return View(await budgets.ToListAsync());
        }

        // GET: Budget/Create
        public IActionResult Create()
        {
            var user = GetUser();

            if (user == null)
                return NotFound();

            var budget = new Budget()
            {
                UserID = user.UserID,
                BudgetTotal = 0,
                CreationDate = DateTime.Now.Date
            };

            ViewData["UserID"] = budget.UserID;
            ViewData["BudgetTotal"] = budget.BudgetTotal;
            ViewData["CreationDate"] = budget.CreationDate;

            return View();
        }

        // POST: Budget/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BudgetID,UserID,BudgetName,BudgetMax,BudgetTotal,CreationDate")] Budget budget)
        {
            var user = GetUser();

            if (user.UserID != budget.UserID)
                return NotFound();

            ViewData["DuplicateNameErrorMessage"] = "";

            try
            {
                budget.BudgetName = budget.BudgetName.Trim();
            }
            catch (NullReferenceException)
            {
                ViewData["DuplicateNameErrorMessage"] = "Budget name cannot be empty";
                ViewData["UserID"] = budget.UserID;
                ViewData["BudgetTotal"] = budget.BudgetTotal;
                ViewData["CreationDate"] = budget.CreationDate;
                return View(budget);
            }

            var existingBudget = await _context.Budgets
                .SingleOrDefaultAsync(b => b.BudgetName.ToLower() == budget.BudgetName.ToLower() && b.UserID == budget.UserID);

            if (existingBudget != null)
            {
                ViewData["DuplicateNameErrorMessage"] = "This budget already exists";
                ViewData["UserID"] = budget.UserID;
                ViewData["BudgetTotal"] = budget.BudgetTotal;
                ViewData["CreationDate"] = budget.CreationDate;
                return View(budget);
            }

            if (ModelState.IsValid)
            {
                _context.Add(budget);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(budget);
        }

        // GET: Budget/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var user = GetUser();

            if (user == null)
                return NotFound();

            var budget = await _context.Budgets
                .FirstOrDefaultAsync(b => b.BudgetID == id && b.UserID == user.UserID);

            if (budget == null)
                return NotFound();

            ViewData["UserID"] = budget.UserID;

            return View(budget);
        }

        // POST: Budget/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BudgetID,UserID,BudgetName,BudgetMax,BudgetTotal,CreationDate")] Budget budget)
        {
            if (id != budget.BudgetID)
                return NotFound();

            var user = GetUser();

            if (user.UserID != budget.UserID)
                return NotFound();

            ViewData["DuplicateNameErrorMessage"] = "";

            try
            {
                budget.BudgetName = budget.BudgetName.Trim();
            }
            catch (NullReferenceException)
            {
                ViewData["DuplicateNameErrorMessage"] = "Budget name cannot be empty";
                ViewData["UserID"] = budget.UserID;
                return View(budget);
            }

            var existingBudget = await _context.Budgets
                .SingleOrDefaultAsync(b => b.BudgetName.ToLower() == budget.BudgetName.ToLower() && b.UserID == budget.UserID && b.BudgetID != budget.BudgetID);

            if (existingBudget != null)
            {
                ViewData["DuplicateNameErrorMessage"] = "This budget already exists";
                ViewData["UserID"] = budget.UserID;
                return View(budget);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budget);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetExists(budget.BudgetID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(budget);
        }

        // GET: Budget/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var user = GetUser();

            if (user == null)
                return NotFound();

            var budget = await _context.Budgets
                .FirstOrDefaultAsync(b => b.BudgetID == id && b.UserID == user.UserID);

            if (budget == null)
                return NotFound();

            return View(budget);
        }

        // POST: Budget/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = GetUser();

            if (user == null)
                return NotFound();

            var budget = await _context.Budgets
                .FirstOrDefaultAsync(b => b.BudgetID == id && b.UserID == user.UserID);

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool BudgetExists(int id)
        {
            return _context.Budgets.Any(e => e.BudgetID == id);
        }

        private User GetUser()
        {
            var aspUserEmail = _userManager.GetUserName(User);

            if (aspUserEmail == null)
                return null;

            return _context.Users.FirstOrDefault(u => u.Email == aspUserEmail);
        }
    }
}
