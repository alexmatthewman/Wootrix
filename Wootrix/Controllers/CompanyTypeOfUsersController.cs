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
    public class CompanyTypeOfUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanyTypeOfUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CompanyTypeOfUsers
        public async Task<IActionResult> Index()
        {
            //Get the company name out the session and use it as a filter for the groups returned
            var id = HttpContext.Session.GetInt32("CompanyID");
            var ctx = _context.CompanyTypeOfUser.Where(m => m.CompanyID == id);
            return View(await ctx.ToListAsync());
        }

        // GET: CompanyTypeOfUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyTypeOfUser = await _context.CompanyTypeOfUser
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companyTypeOfUser == null)
            {
                return NotFound();
            }

            return View(companyTypeOfUser);
        }

        // GET: CompanyTypeOfUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompanyTypeOfUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TypeOfUser")] CompanyTypeOfUser companyTypeOfUser)
        {
            if (ModelState.IsValid)
            {
                companyTypeOfUser.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
                _context.Add(companyTypeOfUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(companyTypeOfUser);
        }

        // GET: CompanyTypeOfUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyTypeOfUser = await _context.CompanyTypeOfUser.FindAsync(id);
            if (companyTypeOfUser == null)
            {
                return NotFound();
            }
            return View(companyTypeOfUser);
        }

        // POST: CompanyTypeOfUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TypeOfUser")] CompanyTypeOfUser companyTypeOfUser)
        {
            if (id != companyTypeOfUser.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    companyTypeOfUser.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
                    _context.Update(companyTypeOfUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyTypeOfUserExists(companyTypeOfUser.ID))
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
            return View(companyTypeOfUser);
        }

        // GET: CompanyTypeOfUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyTypeOfUser = await _context.CompanyTypeOfUser
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companyTypeOfUser == null)
            {
                return NotFound();
            }

            return View(companyTypeOfUser);
        }

        // POST: CompanyTypeOfUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyTypeOfUser = await _context.CompanyTypeOfUser.FindAsync(id);
            _context.CompanyTypeOfUser.Remove(companyTypeOfUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyTypeOfUserExists(int id)
        {
            return _context.CompanyTypeOfUser.Any(e => e.ID == id);
        }
    }
}
