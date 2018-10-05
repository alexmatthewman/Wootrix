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
    public class CompanySegmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanySegmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CompanySegments
        public async Task<IActionResult> Index()
        {
            //Get the company name out the session and use it as a filter for the groups returned
            var id = HttpContext.Session.GetInt32("CompanyID");
            var ctx = _context.CompanySegment.Where(m => m.CompanyID == id);
            return View(await ctx.ToListAsync());
        }

        // GET: CompanySegments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companySegment = await _context.CompanySegment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companySegment == null)
            {
                return NotFound();
            }

            return View(companySegment);
        }

        // GET: CompanySegments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompanySegments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Order,Title,CoverImage,CoverImageMobileFriendly,PublishDate,FinishDate,ClientName,ClientLogoImage,ThemeColor,StandardColor,Draft")] CompanySegment companySegment)
        {
            if (ModelState.IsValid)
            {
                companySegment.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
                _context.Add(companySegment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(companySegment);
        }

        // GET: CompanySegments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companySegment = await _context.CompanySegment.FindAsync(id);
            if (companySegment == null)
            {
                return NotFound();
            }
            return View(companySegment);
        }

        // POST: CompanySegments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Order,Title,CoverImage,CoverImageMobileFriendly,PublishDate,FinishDate,ClientName,ClientLogoImage,ThemeColor,StandardColor,Draft")] CompanySegment companySegment)
        {
            if (id != companySegment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    companySegment.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
                    _context.Update(companySegment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanySegmentExists(companySegment.ID))
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
            return View(companySegment);
        }

        // GET: CompanySegments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companySegment = await _context.CompanySegment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companySegment == null)
            {
                return NotFound();
            }

            return View(companySegment);
        }

        // POST: CompanySegments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companySegment = await _context.CompanySegment.FindAsync(id);
            _context.CompanySegment.Remove(companySegment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanySegmentExists(int id)
        {
            return _context.CompanySegment.Any(e => e.ID == id);
        }
    }
}
