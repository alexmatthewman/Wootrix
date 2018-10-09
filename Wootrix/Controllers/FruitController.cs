using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WootrixV2.Models;

namespace WootrixV2.Controllers
{
    public class FruitController : Controller
    {

        public ActionResult Index()
        {
            var model = new FruitModel
            {
                AvailableFruits = GetFruits()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(FruitModel model)
        {
            if (ModelState.IsValid)
            {
                var fruits = string.Join(",", model.SelectedFruits);

                // Save data to database, and redirect to Success page.

                return RedirectToAction("Success");
            }
            model.AvailableFruits = GetFruits();
            return View(model);
        }

        public ActionResult Success()
        {
            return View();
        }

        private IList<SelectListItem> GetFruits()
        {
            return new List<SelectListItem>
        {
            new SelectListItem {Text = "Apple", Value = "Apple"},
            new SelectListItem {Text = "Pear", Value = "Pear"},
            new SelectListItem {Text = "Banana", Value = "Banana"},
            new SelectListItem {Text = "Orange", Value = "Orange"},
        };
        }
    }

}