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
    public class MagazineArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MagazineArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MagazineArticles
        public async Task<IActionResult> Index()
        {
            return View(await _context.MagazineArticle.ToListAsync());
        }

        // GET: MagazineArticles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var magazineArticle = await _context.MagazineArticle
                .FirstOrDefaultAsync(m => m.ID == id);
            if (magazineArticle == null)
            {
                return NotFound();
            }

            return View(magazineArticle);
        }

        // GET: MagazineArticles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MagazineArticles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CompanyMagazineID,title,image,embedVideoLink,description,link,publishDate,tags,allowComments,allowShare,publishFrom,publishTill,author")] MagazineArticle magazineArticle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(magazineArticle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(magazineArticle);
        }

        // GET: MagazineArticles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var magazineArticle = await _context.MagazineArticle.FindAsync(id);
            if (magazineArticle == null)
            {
                return NotFound();
            }
            return View(magazineArticle);
        }

        // POST: MagazineArticles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CompanyMagazineID,title,image,embedVideoLink,description,link,publishDate,tags,allowComments,allowShare,publishFrom,publishTill,author")] MagazineArticle magazineArticle)
        {
            if (id != magazineArticle.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(magazineArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MagazineArticleExists(magazineArticle.ID))
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
            return View(magazineArticle);
        }

        // GET: MagazineArticles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var magazineArticle = await _context.MagazineArticle
                .FirstOrDefaultAsync(m => m.ID == id);
            if (magazineArticle == null)
            {
                return NotFound();
            }

            return View(magazineArticle);
        }

        // POST: MagazineArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var magazineArticle = await _context.MagazineArticle.FindAsync(id);
            _context.MagazineArticle.Remove(magazineArticle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MagazineArticleExists(int id)
        {
            return _context.MagazineArticle.Any(e => e.ID == id);
        }
    }
}
