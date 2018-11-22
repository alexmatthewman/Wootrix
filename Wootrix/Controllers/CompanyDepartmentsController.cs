using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wootrix.Data;
using WootrixV2.Data;
using WootrixV2.Models;

namespace WootrixV2.Controllers
{
    [Authorize]
    public class CompanyDepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private DatabaseAccessLayer _dla;

        public CompanyDepartmentsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _dla = new DatabaseAccessLayer(_context);
        }

        // GET: CompanyDepartments
        public async Task<IActionResult> Index(int? id)
        {
            var companyID = 0;
            if (!id.HasValue)
            {
                companyID = _userManager.GetUserAsync(User).GetAwaiter().GetResult().companyID;
            }
            else
            {
                companyID = (int)id;
            }
            HttpContext.Session.SetInt32("CompanyID", companyID);

            //Add a company list dropdown to the page
            ViewBag.Companies = _dla.GetCompanies();

            return View(await _context.CompanyDepartments.Where(m => m.CompanyID == id).ToListAsync());
        }

        // GET: CompanyDepartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyDepartments = await _context.CompanyDepartments
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companyDepartments == null)
            {
                return NotFound();
            }

            return View(companyDepartments);
        }

        // GET: CompanyDepartments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompanyDepartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CompanyDepartmentName")] CompanyDepartments companyDepartments)
        {
            if (ModelState.IsValid)
            {

                companyDepartments.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
                _context.Add(companyDepartments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(companyDepartments);
        }

        // GET: CompanyDepartments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyDepartments = await _context.CompanyDepartments.FindAsync(id);
            if (companyDepartments == null)
            {
                return NotFound();
            }
            return View(companyDepartments);
        }

        // POST: CompanyDepartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CompanyDepartmentName")] CompanyDepartments companyDepartments)
        {
            if (id != companyDepartments.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    companyDepartments.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
                    _context.Update(companyDepartments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyDepartmentsExists(companyDepartments.ID))
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
            return View(companyDepartments);
        }

        // GET: CompanyDepartments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyDepartments = await _context.CompanyDepartments
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companyDepartments == null)
            {
                return NotFound();
            }

            return View(companyDepartments);
        }

        // POST: CompanyDepartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyDepartments = await _context.CompanyDepartments.FindAsync(id);
            _context.CompanyDepartments.Remove(companyDepartments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyDepartmentsExists(int id)
        {
            return _context.CompanyDepartments.Any(e => e.ID == id);
        }
    }
}
