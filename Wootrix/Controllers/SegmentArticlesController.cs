using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wootrix.Data;
using WootrixV2.Models;

namespace WootrixV2.Controllers
{
    public class SegmentArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private int csi;

        public SegmentArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SegmentArticles
        public async Task<IActionResult> Index(int segmentID)
        {//Get the company name out the session and use it as a filter for the groups returned
            csi = segmentID;
            var ctx = _context.SegmentArticle.Where(m => m.CompanySegmentID == segmentID);
            return View(await ctx.ToListAsync());
        }

        // GET: SegmentArticles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var segmentArticle = await _context.SegmentArticle
                .FirstOrDefaultAsync(m => m.ID == id);
            if (segmentArticle == null)
            {
                return NotFound();
            }

            return View(segmentArticle);
        }

        // GET: SegmentArticles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SegmentArticles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Order,Title,Image,embedVideoLink,description,link,publishDate,tags,allowComments,publishFrom,publishTill,author")] SegmentArticle segmentArticle)
        {
            if (ModelState.IsValid)
            {
                segmentArticle.CompanySegmentID = csi;
                _context.Add(segmentArticle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(segmentArticle);
        }

        // GET: SegmentArticles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var segmentArticle = await _context.SegmentArticle.FindAsync(id);
            if (segmentArticle == null)
            {
                return NotFound();
            }
            return View(segmentArticle);
        }

        // POST: SegmentArticles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Order,Title,Image,embedVideoLink,description,link,publishDate,tags,allowComments,publishFrom,publishTill,author")] SegmentArticle segmentArticle)
        {
            if (id != segmentArticle.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    segmentArticle.CompanySegmentID = csi;
                    _context.Update(segmentArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SegmentArticleExists(segmentArticle.ID))
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
            return View(segmentArticle);
        }

        // GET: SegmentArticles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var segmentArticle = await _context.SegmentArticle
                .FirstOrDefaultAsync(m => m.ID == id);
            if (segmentArticle == null)
            {
                return NotFound();
            }

            return View(segmentArticle);
        }

        // POST: SegmentArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var segmentArticle = await _context.SegmentArticle.FindAsync(id);
            _context.SegmentArticle.Remove(segmentArticle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SegmentArticleExists(int id)
        {
            return _context.SegmentArticle.Any(e => e.ID == id);
        }
    }
}
