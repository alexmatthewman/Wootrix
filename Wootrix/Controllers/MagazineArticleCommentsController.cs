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
    public class MagazineArticleCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MagazineArticleCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MagazineArticleComments
        public async Task<IActionResult> Index()
        {
            return View(await _context.MagazineArticleComment.ToListAsync());
        }

        // GET: MagazineArticleComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var magazineArticleComment = await _context.MagazineArticleComment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (magazineArticleComment == null)
            {
                return NotFound();
            }

            return View(magazineArticleComment);
        }

        // GET: MagazineArticleComments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MagazineArticleComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MagazineArticleID,Comment,UserID,CreatedDated,status")] SegmentArticleComment magazineArticleComment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(magazineArticleComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(magazineArticleComment);
        }

        // GET: MagazineArticleComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var magazineArticleComment = await _context.MagazineArticleComment.FindAsync(id);
            if (magazineArticleComment == null)
            {
                return NotFound();
            }
            return View(magazineArticleComment);
        }

        // POST: MagazineArticleComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,MagazineArticleID,Comment,UserID,CreatedDated,status")] SegmentArticleComment magazineArticleComment)
        {
            if (id != magazineArticleComment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(magazineArticleComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MagazineArticleCommentExists(magazineArticleComment.ID))
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
            return View(magazineArticleComment);
        }

        // GET: MagazineArticleComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var magazineArticleComment = await _context.MagazineArticleComment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (magazineArticleComment == null)
            {
                return NotFound();
            }

            return View(magazineArticleComment);
        }

        // POST: MagazineArticleComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var magazineArticleComment = await _context.MagazineArticleComment.FindAsync(id);
            _context.MagazineArticleComment.Remove(magazineArticleComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MagazineArticleCommentExists(int id)
        {
            return _context.MagazineArticleComment.Any(e => e.ID == id);
        }
    }
}
