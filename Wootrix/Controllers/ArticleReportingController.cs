using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Wootrix.Data;
using WootrixV2.Data;
using WootrixV2.Models;
using Newtonsoft.Json;

namespace WootrixV2.Controllers
{
    [Authorize]
    public class ArticleReportingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private WootrixV2.Models.User _user;

        public ArticleReportingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ArticleReportings
        public async Task<IActionResult> Index()
        {
            _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
            var companyID = _user.CompanyID;
            //We need to build the datasets for the graphs into the viewbag
            var articlesRead = _context.ArticleReporting.Where(m => m.CompanyID == companyID);
            var totalArticles = _context.SegmentArticle.Where(m => m.CompanyID == companyID);

            AddFilters(companyID);

            SetupOSReport(articlesRead);
            SetupPlatformReport(articlesRead);
            SetupViewsByLocation(articlesRead);
            SetupViewsPerArticleReport(articlesRead);


            return View(await articlesRead.ToListAsync());
        }

        [HttpPost]
        public IActionResult UpdateGraphs(GraphFilters filters)
        {
            if (!string.IsNullOrWhiteSpace(filters.ToString()))
            {           
                _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
                var companyID = _user.CompanyID;

                //We need to build the datasets for the graphs into the viewbag
                var articlesRead = _context.ArticleReporting.Where(m => m.CompanyID == companyID && m.SegmentName == filters.magazine);
                string[] articles = articlesRead.Select(d => d.ArticleName).Distinct().ToArray();
                SetupViewsPerArticleReport(articlesRead);
                return Json(articles);
            }
            return null;
        }

        public class GraphFilters
        {
            public string magazine { get; set; }
            
        }



        public void AddFilters( int companyID)
        {            
            
            ViewBag.Segments = _context.CompanySegment.Where(m => m.CompanyID == companyID).Select(m => m.Title).Distinct().ToList();
            //ViewBag.Articles = _context.SegmentArticle.Where(m => m.CompanyID == companyID).Select(m => m.Title).Distinct().ToList();
            //ViewBag.TypeOfUser = _context.CompanyTypeOfUser.Where(m => m.CompanyID == companyID).Select(m => m.TypeOfUser).Distinct().ToList();
            //ViewBag.Topics = _context.CompanyTopics.Where(m => m.CompanyID == companyID).Select(m => m.Topic).Distinct().ToList();
            //ViewBag.Languages = _context.CompanyLanguages.Where(m => m.CompanyID == companyID).Select(m => m.LanguageName).Distinct().ToList();
            //ViewBag.Groups = _context.CompanyGroups.Where(m => m.CompanyID == companyID).Select(m => m.GroupName).Distinct().ToList();
            //ViewBag.Countries = _context.CompanyLocCountries.Select(m => m.country_name).Distinct().ToList();
            //ViewBag.States = _context.CompanyLocStates.Select(m => m.state_name).Distinct().ToList();
            //ViewBag.Cities = _context.CompanyLocCities.Select(m => m.city_name_ascii).Distinct().ToList();
        }

        public void SetupOSReport(IQueryable<ArticleReporting> articlesRead)
        {
            //Need the count and name of each OS type put into arrays
            List<string> osTypes = articlesRead.Select(d => d.OSType).Distinct().ToList();
            int[] osTypeCountArray = new int[osTypes.Count];
            string[] osTypeColorArray = new string[osTypes.Count];

            for (int i = 0; i < osTypes.Count; i++)
            {
                osTypeCountArray[i] = articlesRead.Where(d => d.OSType == osTypes[i]).Count();
                osTypeColorArray[i] = GetRandomColor();
            }

            ViewBag.osTypes = osTypes.ToArray();
            ViewBag.osTypeCountArray = osTypeCountArray;
            ViewBag.osTypeColorArray = osTypeColorArray;
        }

        public void SetupPlatformReport(IQueryable<ArticleReporting> articlesRead)
        {
            //Need the count and name of each OS type put into arrays
            string[] platformTypes = new string[3] { "Desktop", "Mobile", "Other" };
            int[] platformTypesCountArray = new int[platformTypes.Length];
            string[] platformTypesColorArray = new string[platformTypes.Length];

            platformTypesCountArray[0] = articlesRead.Where(d => d.OSType.Contains("Windows") || d.OSType.Contains("Linux") || d.OSType.Contains("Mac")).Count();
            platformTypesCountArray[1] = articlesRead.Where(d => d.OSType.Contains("Android") || d.OSType.Contains("iOS")).Count();
            platformTypesCountArray[2] = (articlesRead.Count() - platformTypesCountArray[0] - platformTypesCountArray[1]);

            for (int i = 0; i < platformTypes.Length; i++)
            {

                platformTypesColorArray[i] = GetRandomColor();
            }

            ViewBag.platformTypes = platformTypes.ToArray();
            ViewBag.platformTypesCountArray = platformTypesCountArray;
            ViewBag.platformTypesColorArray = platformTypesColorArray;
        }

        public void SetupViewsByLocation(IQueryable<ArticleReporting> articlesRead)
        {
            //This lists the views by user location
            List<string> locations = articlesRead.Select(d => d.City).Distinct().ToList();
            //locations.RemoveAt(locations.Count-1);
            int[] locationsCountArray = new int[locations.Count];
            string[] locationsColorArray = new string[locations.Count];
            //List<ChartDataSets> cdsArray = new List<ChartDataSets>();





            for (int i = 0; i < locations.Count; i++)
            {

                locationsCountArray[i] = articlesRead.Where(d => d.City == locations[i]).Count();
                locationsColorArray[i] = GetRandomColor();
                if (locations[i] == null) locations[i] = "No Location Data";

                //ChartDataSets cds = new ChartDataSets();
                //cds.backgroundColor = locationsColorArray[i];
                //cds.borderColor = locationsColorArray[i];
                //cds.borderWidth = 1;
                //cds.data = new string[] { locationsCountArray[i].ToString() };
                //cds.label = locations[i];

                //cdsArray.Add(cds);               
            }
            //BarChartData bcd = new BarChartData();
            //bcd.labels = new string[] { "Users by City" };
            //bcd.datasets = cdsArray.ToArray<ChartDataSets>();
            //var x = JsonConvert.SerializeObject(bcd);
            //ViewBag.barChartDataStruct = x;
            ViewBag.locations = locations.ToArray();//.Skip(1).ToArray();
            ViewBag.locationsCountArray = locationsCountArray;//.Skip(1).ToArray();
            ViewBag.locationsColorArray = locationsColorArray;//.Skip(1).ToArray();
        }

        public void SetupViewsPerArticleReport(IQueryable<ArticleReporting> articlesRead)
        {
            //Need the count and name of each OS type put into arrays
            string[] articles = articlesRead.Select(d => d.ArticleName).Distinct().ToArray();
            int[] articlesCountArray = new int[articles.Length];
            string[] articlesColorArray = new string[articles.Length];



            for (int i = 0; i < articles.Length; i++)
            {
                articlesCountArray[i] = articlesRead.Where(d => d.ArticleName == articles[i]).Count();
                articlesColorArray[i] = GetRandomColor();
            }

            ViewBag.articles = articles;
            ViewBag.articlesCountArray = articlesCountArray;
            ViewBag.articlesColorArray = articlesColorArray;
        }

        public class ChartDataSets
        {
            public string Label { get; set; }
            public string BackgroundColor { get; set; }
            public string BorderColor { get; set; }
            public int BorderWidth { get; set; }
            public string[] Data { get; set; }
        }

        public class BarChartData
        {
            public string[] Labels { get; set; }
            public ChartDataSets[] Datasets { get; set; }
        }


        public string GetRandomColor()
        {
            var random = new Random();
            var color = String.Format("#{0:X6}", random.Next(0x1000000));
            return color;
        }


        ////// GET: ArticleReportings/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var articleReporting = await _context.ArticleReporting
        //        .FirstOrDefaultAsync(m => m.ID == id);
        //    if (articleReporting == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(articleReporting);
        //}

        //// GET: ArticleReportings/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ArticleReportings/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("UserName,SegmentName,ArticleName,DeviceType,OSType,ArticleReadTime,Country,State,City")] ArticleReporting articleReporting)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(articleReporting);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(articleReporting);
        //}

        //// GET: ArticleReportings/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var articleReporting = await _context.ArticleReporting.FindAsync(id);
        //    if (articleReporting == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(articleReporting);
        //}

        //// POST: ArticleReportings/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("UserName,SegmentName,ArticleName,DeviceType,OSType,ArticleReadTime,Country,State,City")] ArticleReporting articleReporting)
        //{
        //    if (id != articleReporting.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(articleReporting);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ArticleReportingExists(articleReporting.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(articleReporting);
        //}

        //// GET: ArticleReportings/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var articleReporting = await _context.ArticleReporting
        //        .FirstOrDefaultAsync(m => m.ID == id);
        //    if (articleReporting == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(articleReporting);
        //}

        //// POST: ArticleReportings/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var articleReporting = await _context.ArticleReporting.FindAsync(id);
        //    _context.ArticleReporting.Remove(articleReporting);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ArticleReportingExists(int id)
        //{
        //    return _context.ArticleReporting.Any(e => e.ID == id);
        //}
    }
}
