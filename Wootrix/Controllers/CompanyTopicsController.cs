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
    public class CompanyTopicsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanyTopicsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CompanyTopics
        public async Task<IActionResult> Index()
        {
            //Get the company name out the session and use it as a filter for the groups returned
            var id = HttpContext.Session.GetInt32("CompanyID");
            var ctx = _context.CompanyTopics.Where(m => m.CompanyID == id);
            return View(await ctx.ToListAsync());
        }

        // GET: CompanyTopics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyTopics = await _context.CompanyTopics
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companyTopics == null)
            {
                return NotFound();
            }

            return View(companyTopics);
        }

        // GET: CompanyTopics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompanyTopics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Topic")] CompanyTopics companyTopics)
        {
            if (ModelState.IsValid)
            {
                companyTopics.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
                _context.Add(companyTopics);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(companyTopics);
        }

        // GET: CompanyTopics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyTopics = await _context.CompanyTopics.FindAsync(id);
            if (companyTopics == null)
            {
                return NotFound();
            }
            return View(companyTopics);
        }

        // POST: CompanyTopics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Topic")] CompanyTopics companyTopics)
        {
            if (id != companyTopics.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    companyTopics.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
                    _context.Update(companyTopics);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyTopicsExists(companyTopics.ID))
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
            return View(companyTopics);
        }

        // GET: CompanyTopics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyTopics = await _context.CompanyTopics
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companyTopics == null)
            {
                return NotFound();
            }

            return View(companyTopics);
        }

        // POST: CompanyTopics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyTopics = await _context.CompanyTopics.FindAsync(id);
            _context.CompanyTopics.Remove(companyTopics);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyTopicsExists(int id)
        {
            return _context.CompanyTopics.Any(e => e.ID == id);
        }
    }
}
