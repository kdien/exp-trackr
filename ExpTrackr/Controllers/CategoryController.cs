using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpTrackr.Data;
using ExpTrackr.Models;
using ExpTrackr.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ExpTrackr.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ExpTrackrContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CategoryController(ExpTrackrContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Categories
        public IActionResult Index()
        {
            var user = GetUser();

            if (user == null)
                return NotFound();

            var viewModel = new CategoryViewModel
            {
                UserID = user.UserID,
                Categories = _context.Categories.Where(c => c.UserID == user.UserID).OrderBy(c => c.CategoryName)
            };

            return View(viewModel);
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryName,UserID")] Category category)
        {
            var user = GetUser();

            if (user.UserID != category.UserID)
                return NotFound();

            var viewModel = new CategoryViewModel
            {
                UserID = user.UserID,
                Categories = _context.Categories.Where(c => c.UserID == user.UserID)
            };

            ViewData["DuplicateNameErrorMessage"] = "";

            try
            {
                category.CategoryName = category.CategoryName.Trim();
            }
            catch (NullReferenceException)
            {
                ViewData["DuplicateNameErrorMessage"] = "Category name cannot be empty";
                return View("Index", viewModel);
            }

            var existingCategory = await _context.Categories
                .SingleOrDefaultAsync(c => c.CategoryName.ToLower() == category.CategoryName.ToLower() && c.UserID == category.UserID);

            if (existingCategory != null)
            {
                ViewData["DuplicateNameErrorMessage"] = "This category already exists";
                return View("Index", viewModel);
            }

            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View("Index", viewModel);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var user = GetUser();

            if (user == null)
                return NotFound();

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryID == id && c.UserID == user.UserID);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryID,CategoryName,UserID")] Category category)
        {
            if (id != category.CategoryID)
                return NotFound();

            var user = GetUser();

            if (user.UserID != category.UserID)
                return NotFound();

            ViewData["DuplicateNameErrorMessage"] = "";

            try
            {
                category.CategoryName = category.CategoryName.Trim();
            }
            catch (NullReferenceException)
            {
                ViewData["DuplicateNameErrorMessage"] = "Category name cannot be empty";
                return View(category);
            }

            var existingCategory = await _context.Categories
                .SingleOrDefaultAsync(c => c.CategoryName.ToLower() == category.CategoryName.ToLower() && c.UserID == category.UserID && c.CategoryID != category.CategoryID);

            if (existingCategory != null)
            {
                ViewData["DuplicateNameErrorMessage"] = "This category already exists";
                return View(category);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var user = GetUser();

            if (user == null)
                return NotFound();

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryID == id && c.UserID == user.UserID);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = GetUser();

            if (user == null)
                return NotFound();

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryID == id && c.UserID == user.UserID);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryID == id);
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
