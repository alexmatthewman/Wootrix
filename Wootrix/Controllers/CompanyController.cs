using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wootrix.Data;
using WootrixV2.Models;

namespace WootrixV2.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Company
        public async Task<IActionResult> Index()
        {
            return View(await _context.Company.ToListAsync());
        }


        // GET: Comany Home
        public async Task<IActionResult> Home(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .FirstOrDefaultAsync(m => m.CompanyName == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyViewModel model)
        {
            //Initialise a new company
            var myCompany = new Company();

            if (ModelState.IsValid)
            {
                //Set the simple fields
                myCompany.CompanyName = model.CompanyName;
                myCompany.CompanyTextMain = model.CompanyTextMain;
                myCompany.CompanyTextSecondary = model.CompanyTextSecondary;
                myCompany.CompanyBackgroundColor = model.CompanyBackgroundColor;
                myCompany.CompanyHeaderBackgroundColor = model.CompanyHeaderBackgroundColor;
                myCompany.CompanyHighlightColor = model.CompanyHighlightColor;
                myCompany.CompanyMainFontColor = model.CompanyMainFontColor;
                myCompany.CompanyHeaderFontColor = model.CompanyHeaderFontColor;

                //Copy the IFormFiles to stream and save it to the byte arrays
                using (var memoryStream = new MemoryStream())
                {
                    await model.CompanyLogoImage.CopyToAsync(memoryStream);
                    myCompany.CompanyLogoImage = memoryStream.ToArray();
                }

                //Optional so check if there first
                if (model.CompanyFocusImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.CompanyFocusImage.CopyToAsync(memoryStream);
                        myCompany.CompanyFocusImage = memoryStream.ToArray();
                    }
                }

                //Optional so check if there first
                if (model.CompanyBackgroundImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.CompanyBackgroundImage.CopyToAsync(memoryStream);
                        myCompany.CompanyBackgroundImage = memoryStream.ToArray();
                    }
                }

                _context.Add(myCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(myCompany);
        }


        // GET: Company/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .FirstOrDefaultAsync(m => m.ID == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Company/Create
        public IActionResult Create()
        {
            return View();
        }

        //// POST: Company/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ID,CompanyName,CompanyLogoImage,CompanyBackgroundColor,CompanyBackgroundImage,CompanyFocusImage,CompanyTextMain,CompanyTextSecondary,CompanyHighlightColor,CompanyHeaderBackgroundColor,CompanyMainFontColor,CompanyHeaderFontColor")] Company company)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(company);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(company);
        //}

        // GET: Company/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompanyViewModel model)
        {
            //Initialise a new company
            var myCompany = new Company();

            if (ModelState.IsValid)
            {
                myCompany.ID = id;
                //Set the simple fields
                myCompany.CompanyName = model.CompanyName;
                myCompany.CompanyTextMain = model.CompanyTextMain;
                myCompany.CompanyTextSecondary = model.CompanyTextSecondary;
                myCompany.CompanyBackgroundColor = model.CompanyBackgroundColor;
                myCompany.CompanyHeaderBackgroundColor = model.CompanyHeaderBackgroundColor;
                myCompany.CompanyHighlightColor = model.CompanyHighlightColor;
                myCompany.CompanyMainFontColor = model.CompanyMainFontColor;
                myCompany.CompanyHeaderFontColor = model.CompanyHeaderFontColor;

                //Copy the IFormFiles to stream and save it to the byte arrays
                using (var memoryStream = new MemoryStream())
                {
                    await model.CompanyLogoImage.CopyToAsync(memoryStream);
                    myCompany.CompanyLogoImage = memoryStream.ToArray();
                }

                //Optional so check if there first
                if (model.CompanyFocusImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.CompanyFocusImage.CopyToAsync(memoryStream);
                        myCompany.CompanyFocusImage = memoryStream.ToArray();
                    }
                }

                //Optional so check if there first
                if (model.CompanyBackgroundImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.CompanyBackgroundImage.CopyToAsync(memoryStream);
                        myCompany.CompanyBackgroundImage = memoryStream.ToArray();
                    }
                }

                try
                {
                    _context.Update(myCompany);
                await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(id))
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

            return View(myCompany);
        }

            

        // GET: Company/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .FirstOrDefaultAsync(m => m.ID == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Company.FindAsync(id);
            _context.Company.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Company.Any(e => e.ID == id);
        }
    }
}
