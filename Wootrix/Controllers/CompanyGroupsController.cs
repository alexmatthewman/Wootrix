using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wootrix.Data;
using WootrixV2.Models;

namespace WootrixV2.Controllers
{
    public class CompanyGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanyGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CompanyGroups
        public async Task<IActionResult> Index()
        {

            //Get the company name out the session and use it as a filter for the groups returned
            var id = HttpContext.Session.GetInt32("CompanyID");
            var ctx = _context.CompanyGroups.Where(m => m.CompanyID == id);
            return View(await ctx.ToListAsync());
        }

        // GET: CompanyGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyGroups = await _context.CompanyGroups
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companyGroups == null)
            {
                return NotFound();
            }

            return View(companyGroups);
        }

        // GET: CompanyGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompanyGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,GroupName")] CompanyGroups companyGroups)
        {
            if (ModelState.IsValid)
            {
                companyGroups.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
                _context.Add(companyGroups);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(companyGroups);
        }

        // GET: CompanyGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyGroups = await _context.CompanyGroups.FindAsync(id);
            if (companyGroups == null)
            {
                return NotFound();
            }
            return View(companyGroups);
        }

        // POST: CompanyGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,GroupName")] CompanyGroups companyGroups)
        {
            if (id != companyGroups.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    companyGroups.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
                    _context.Update(companyGroups);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyGroupsExists(companyGroups.ID))
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
            return View(companyGroups);
        }

        // GET: CompanyGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyGroups = await _context.CompanyGroups
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companyGroups == null)
            {
                return NotFound();
            }

            return View(companyGroups);
        }

        // POST: CompanyGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyGroups = await _context.CompanyGroups.FindAsync(id);
            _context.CompanyGroups.Remove(companyGroups);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyGroupsExists(int id)
        {
            return _context.CompanyGroups.Any(e => e.ID == id);
        }
    }
}
