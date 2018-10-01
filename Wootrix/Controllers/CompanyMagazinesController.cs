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
    public class CompanyMagazinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanyMagazinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CompanyMagazines
        public async Task<IActionResult> Index()
        {

            //Get the company name out the session and use it as a filter for the groups returned
            var id = HttpContext.Session.GetInt32("CompanyID");
            var ctx = _context.CompanyMagazine.Where(m => m.CompanyID == id);
            return View(await ctx.ToListAsync());
        }

        // GET: CompanyMagazines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyMagazine = await _context.CompanyMagazine
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companyMagazine == null)
            {
                return NotFound();
            }

            return View(companyMagazine);
        }

        // GET: CompanyMagazines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompanyMagazines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MagazineTitle,MagazineCoverImage")] CompanyMagazine companyMagazine)
        {
            companyMagazine.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? -1;
            if (ModelState.IsValid)
            {
                _context.Add(companyMagazine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(companyMagazine);
        }

        // GET: CompanyMagazines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyMagazine = await _context.CompanyMagazine.FindAsync(id);
            if (companyMagazine == null)
            {
                return NotFound();
            }
            return View(companyMagazine);
        }

        // POST: CompanyMagazines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,MagazineTitle,MagazineCoverImage")] CompanyMagazine companyMagazine)
        {

            if (id != companyMagazine.ID)
            {
                return NotFound();
            }
            companyMagazine.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? -1;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyMagazine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyMagazineExists(companyMagazine.ID))
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
            return View(companyMagazine);
        }

        // GET: CompanyMagazines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyMagazine = await _context.CompanyMagazine
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companyMagazine == null)
            {
                return NotFound();
            }

            return View(companyMagazine);
        }

        // POST: CompanyMagazines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyMagazine = await _context.CompanyMagazine.FindAsync(id);
            _context.CompanyMagazine.Remove(companyMagazine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyMagazineExists(int id)
        {
            return _context.CompanyMagazine.Any(e => e.ID == id);
        }
    }
}
