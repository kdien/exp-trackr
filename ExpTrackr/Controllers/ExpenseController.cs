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
    public class ExpenseController : Controller
    {
        private readonly ExpTrackrContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ExpenseController(ExpTrackrContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Expenses
        public async Task<IActionResult> Index(int? budgetId)
        {
            if (budgetId == null)
                return NotFound();
            
            var budget = GetBudget(budgetId);

            if (budget == null)
                return NotFound();

            var expenses = await _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.BudgetID == budget.BudgetID)
                .ToListAsync();

            return View(expenses);
        }

        // GET: Expenses/Create
        public IActionResult Create(int? budgetId)
        {
            if (budgetId == null)
                return NotFound();

            var budget = GetBudget(budgetId);

            if (budget == null)
                return NotFound();

            GetCategoryList();

            return View();
        }

        // POST: Expenses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BudgetID,CategoryID,Description,Amount")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { budgetId = expense.BudgetID });
            }

            return View(expense);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id, int? budgetId)
        {
            if (id == null || budgetId == null)
                return NotFound();

            var budget = GetBudget(budgetId);

            if (budget == null)
                return NotFound();

            GetCategoryList();

            var expense = await _context.Expenses
                .Where(e => e.ExpenseID == id && e.BudgetID == budget.BudgetID)
                .SingleOrDefaultAsync();

            if (expense == null)
                return NotFound();

            return View(expense);
        }

        // POST: Expenses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpenseID,BudgetID,CategoryID,Description,Amount")] Expense expense)
        {
            if (id != expense.ExpenseID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.ExpenseID))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction("Index", new { budgetId = expense.BudgetID });
            }

            return View(expense);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id, int? budgetId)
        {
            if (id == null || budgetId == null)
                return NotFound();

            var budget = GetBudget(budgetId);

            if (budget == null)
                return NotFound();

            var expense = await _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.ExpenseID == id && e.BudgetID == budget.BudgetID)
                .SingleOrDefaultAsync();

            if (expense == null)
                return NotFound();

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expense = _context.Expenses.SingleOrDefault(e => e.ExpenseID == id);

            if (expense == null)
                return NotFound();

            var budget = GetBudget(expense.BudgetID);

            if (budget == null)
                return NotFound();

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { budgetId = expense.BudgetID });
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.ExpenseID == id);
        }

        private User GetUser()
        {
            var aspUserEmail = _userManager.GetUserName(User);

            if (aspUserEmail == null)
                return null;

            return _context.Users.FirstOrDefault(u => u.Email == aspUserEmail);
        }

        private Budget GetBudget(int? id)
        {
            if (id == null)
                return null;

            var user = GetUser();

            if (user == null)
                return null;

            var budget = _context.Budgets
                .Where(b => b.BudgetID == id && b.UserID == user.UserID)
                .SingleOrDefault();

            if (budget == null)
                return null;

            if (budget.BudgetTotal > budget.BudgetMax)
                ViewData["LimitBroken"] = true;
            else
                ViewData["LimitBroken"] = false;

            ViewData["BudgetID"] = budget.BudgetID;
            ViewData["BudgetName"] = budget.BudgetName;
            ViewData["BudgetMax"] = String.Format("{0:C}", budget.BudgetMax);
            ViewData["BudgetTotal"] = String.Format("{0:C}", budget.BudgetTotal);

            return budget;
        }

        private void GetCategoryList()
        {
            var user = GetUser();

            if (user == null)
                return;

            ViewBag.CategoryList = new SelectList(_context.Categories.Where(c => c.UserID == user.UserID), "CategoryID", "CategoryName");
        }
    }
}
