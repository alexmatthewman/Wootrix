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
    public class SegmentArticleCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private int _sai;

        public SegmentArticleCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SegmentArticleComments
        public async Task<IActionResult> Index(int articleID)
        {
            _sai = articleID;
            var ctx = _context.SegmentArticleComment.Where(m => m.SegmentArticleID == articleID);
            return View(await ctx.ToListAsync());
        }

        // GET: SegmentArticleComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var segmentArticleComment = await _context.SegmentArticleComment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (segmentArticleComment == null)
            {
                return NotFound();
            }

            return View(segmentArticleComment);
        }

        // GET: SegmentArticleComments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SegmentArticleComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Comment")] SegmentArticleComment segmentArticleComment)
        {
            if (ModelState.IsValid)
            {
                segmentArticleComment.SegmentArticleID = _sai;
                _context.Add(segmentArticleComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(segmentArticleComment);
        }

        // GET: SegmentArticleComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var segmentArticleComment = await _context.SegmentArticleComment.FindAsync(id);
            if (segmentArticleComment == null)
            {
                return NotFound();
            }
            return View(segmentArticleComment);
        }

        // POST: SegmentArticleComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Comment")] SegmentArticleComment segmentArticleComment)
        {
            if (id != segmentArticleComment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    segmentArticleComment.SegmentArticleID = _sai;
                    _context.Update(segmentArticleComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SegmentArticleCommentExists(segmentArticleComment.ID))
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
            return View(segmentArticleComment);
        }

        // GET: SegmentArticleComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var segmentArticleComment = await _context.SegmentArticleComment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (segmentArticleComment == null)
            {
                return NotFound();
            }

            return View(segmentArticleComment);
        }

        // POST: SegmentArticleComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var segmentArticleComment = await _context.SegmentArticleComment.FindAsync(id);
            _context.SegmentArticleComment.Remove(segmentArticleComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SegmentArticleCommentExists(int id)
        {
            return _context.SegmentArticleComment.Any(e => e.ID == id);
        }
    }
}
