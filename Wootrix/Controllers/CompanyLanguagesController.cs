using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wootrix.Data;
using WootrixV2.Models;

namespace WootrixV2.Controllers
{
    [Authorize]
    public class CompanyLanguagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanyLanguagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CompanyLanguages
        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetInt32("CompanyID");
            var ctx = _context.CompanyLanguages.Where(m => m.CompanyID == id);
            return View(await ctx.ToListAsync());
        }

        // GET: CompanyLanguages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyLanguages = await _context.CompanyLanguages
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companyLanguages == null)
            {
                return NotFound();
            }

            return View(companyLanguages);
        }

        // GET: CompanyLanguages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompanyLanguages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LanguageName")] CompanyLanguages companyLanguages)
        {
            if (ModelState.IsValid)
            {
                companyLanguages.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
                _context.Add(companyLanguages);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(companyLanguages);
        }

        // GET: CompanyLanguages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyLanguages = await _context.CompanyLanguages.FindAsync(id);
            if (companyLanguages == null)
            {
                return NotFound();
            }
            return View(companyLanguages);
        }

        // POST: CompanyLanguages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LanguageName")] CompanyLanguages companyLanguages)
        {
            if (id != companyLanguages.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    companyLanguages.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
                    _context.Update(companyLanguages);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyLanguagesExists(companyLanguages.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(companyLanguages);
        }

        // GET: CompanyLanguages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyLanguages = await _context.CompanyLanguages
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companyLanguages == null)
            {
                return NotFound();
            }

            return View(companyLanguages);
        }

        // POST: CompanyLanguages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyLanguages = await _context.CompanyLanguages.FindAsync(id);
            _context.CompanyLanguages.Remove(companyLanguages);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyLanguagesExists(int id)
        {
            return _context.CompanyLanguages.Any(e => e.ID == id);
        }
    }
}
