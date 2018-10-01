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
    public class CompanyPushNotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanyPushNotificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CompanyPushNotifications
        public async Task<IActionResult> Index()
        {
            return View(await _context.CompanyPushNotification.ToListAsync());
        }

        // GET: CompanyPushNotifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyPushNotification = await _context.CompanyPushNotification
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companyPushNotification == null)
            {
                return NotFound();
            }

            return View(companyPushNotification);
        }

        // GET: CompanyPushNotifications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompanyPushNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CompanyID,UserID,Message,Options,SentAt")] CompanyPushNotification companyPushNotification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(companyPushNotification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(companyPushNotification);
        }

        // GET: CompanyPushNotifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyPushNotification = await _context.CompanyPushNotification.FindAsync(id);
            if (companyPushNotification == null)
            {
                return NotFound();
            }
            return View(companyPushNotification);
        }

        // POST: CompanyPushNotifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CompanyID,UserID,Message,Options,SentAt")] CompanyPushNotification companyPushNotification)
        {
            if (id != companyPushNotification.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyPushNotification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyPushNotificationExists(companyPushNotification.ID))
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
            return View(companyPushNotification);
        }

        // GET: CompanyPushNotifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyPushNotification = await _context.CompanyPushNotification
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companyPushNotification == null)
            {
                return NotFound();
            }

            return View(companyPushNotification);
        }

        // POST: CompanyPushNotifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyPushNotification = await _context.CompanyPushNotification.FindAsync(id);
            _context.CompanyPushNotification.Remove(companyPushNotification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyPushNotificationExists(int id)
        {
            return _context.CompanyPushNotification.Any(e => e.ID == id);
        }
    }
}
