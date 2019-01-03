using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Wootrix.Data;
using WootrixV2.Data;
using WootrixV2.Models;

namespace WootrixV2.Controllers
{
    [Authorize]
    public class SegmentArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;
        private readonly string _rootpath;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser _user;
        private readonly IOptions<RequestLocalizationOptions> _rlo;
        private DataAccessLayer _dla;

        public SegmentArticlesController(IOptions<RequestLocalizationOptions> rlo, UserManager<ApplicationUser> userManager, IHostingEnvironment env, ApplicationDbContext context)
        {
            _context = context;
            _env = env;
            _rootpath = _env.WebRootPath;
            _userManager = userManager;
            _rlo = rlo;
            _dla = new DataAccessLayer(_context);
        }

        public async Task<IActionResult> ChangeOrder(string id)
        {
            var orderArray = id.Split("|");
            var order = orderArray[0].ToString();
            bool success = Int32.TryParse(orderArray[1].ToString(), out int articleID);

            var _companyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            var article = await _context.SegmentArticle.FindAsync(articleID);
            int whereItCurrentlyIs = article.Order ?? 0;
            success = Int32.TryParse(order, out int whereItIsMovingTo);

            // So for each segment with an order greater than the order we need to increment the order number
            if (whereItCurrentlyIs < whereItIsMovingTo)
            {
                foreach (var art in _context.SegmentArticle.Where(m => m.CompanyID == _companyID && ((m.Order ?? 0) <= whereItIsMovingTo) && (m.Order ?? 0) > whereItCurrentlyIs))
                {
                    art.Order--;
                    _context.Update(art);
                }
            }
            else
            {
                foreach (var art in _context.SegmentArticle.Where(m => m.CompanyID == _companyID && ((m.Order ?? 0) >= whereItIsMovingTo) && (m.Order ?? 0) < whereItCurrentlyIs))
                {
                    art.Order++;
                    _context.Update(art);
                }
            }

            // Update the order of the segmentID to be as passed
            article.Order = whereItIsMovingTo;
            _context.Update(article);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // Decrement everything else
        public void InsertAtOrder1()
        {
            var _companyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            foreach (var art in _context.SegmentArticle.Where(m => m.CompanyID == _companyID))
            {
                art.Order--;
                _context.Update(art);
            }
            _context.SaveChanges();
        }


        // GET: SegmentArticles
        public async Task<IActionResult> Index()
        {
            ViewBag.UploadsLocation = "https://s3-us-west-2.amazonaws.com/wootrixv2uploadfiles/images/Uploads/";

            //Get the company name out the session and use it as a filter for the groups returned
            var id = HttpContext.Session.GetInt32("CompanyID");
            var ctx = await _context.SegmentArticle.Where(m => m.CompanyID == id).ToListAsync();
            foreach (SegmentArticle item in ctx)
            {
                if (!string.IsNullOrWhiteSpace(item.Languages)) item.Languages.Replace("|", ", ");
                if (!string.IsNullOrWhiteSpace(item.Groups)) item.Groups.Replace("|", ", ");
                if (!string.IsNullOrWhiteSpace(item.Segments)) item.Segments.Replace("|", ", ");
                if (!string.IsNullOrWhiteSpace(item.TypeOfUser)) item.TypeOfUser.Replace("|", ", ");
                if (!string.IsNullOrWhiteSpace(item.Topics)) item.Topics.Replace("|", ", ");
            }
            return View(ctx);
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



        // GET: SegmentArticles/Article/5
        public async Task<IActionResult> Article(int? id)
        {
            ViewBag.UploadsLocation = "https://s3-us-west-2.amazonaws.com/wootrixv2uploadfiles/images/Uploads/";

            if (id == null)
            {
                return NotFound();
            }

            DataAccessLayer dla = new DataAccessLayer(_context);
            ViewBag.CommentCount = dla.GetArticleApprovedCommentCount(id ?? 0);

            ViewBag.Comments = dla.GetArticleCommentsList(id ?? 0);

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
            _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            SegmentArticleViewModel s = new SegmentArticleViewModel();
            s.Order = 1;
            s.PublishFrom = DateTime.Now.Date;
            s.PublishTill = DateTime.Now.AddYears(10).Date;
            s.CompanyID = _user.companyID;
            s.Author = _user.name;
            s.AllowComments = true;

            DataAccessLayer dla = new DataAccessLayer(_context);

            var listOfAllSegements = dla.GetArticleSegments(_user.companyID);
            foreach (var seg in listOfAllSegements)
            {

                s.AvailableSegments.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
            }

            // Add group checkboxes
            var listOfAllGroups = dla.GetListGroups(_user.companyID);
            foreach (var seg in listOfAllGroups)
            {
                s.AvailableGroups.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
            }

            // Add topic checkboxes
            var listOfAllTopics = dla.GetListTopics(_user.companyID);
            foreach (var seg in listOfAllTopics)
            {
                s.AvailableTopics.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
            }

            // Add type checkboxes
            var listOfAllTypeOfUser = dla.GetListTypeOfUser(_user.companyID);
            foreach (var seg in listOfAllTypeOfUser)
            {
                s.AvailableTypeOfUser.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
            }

            // Add language checkboxes
            var listOfAllLanguages = dla.GetListLanguages(_user.companyID, _rlo);
            foreach (var seg in listOfAllLanguages)
            {
                s.AvailableLanguages.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
            }

            // Get location dropdown data
            s.Countries = dla.GetCountries();
            s.States = dla.GetNullStatesOrCities();
            s.Cities = dla.GetNullStatesOrCities();
            return View(s);
        }


        public string CreateSegmentsStringWithOrder1(IList<string> segments)
        {
            var segString = "";
            var last = segments.Last();
            foreach (string segTitle in segments)
            {
                if (segTitle == last)
                {
                    // We don't put the separator on the end of the last item
                    segString += segTitle + "/1";
                }
                else
                {
                    segString += segTitle + "/1|";
                }
                UpdateSegmentsAsThereWasAnInsertAt1(segTitle);
            }
            return segString;
        }

        public string CreateSegmentsStringWithOrder1ButWithCheckToMakeSureNotAt1Already(IList<string> newSegments, IList<string> oldSegments)
        {
            var segString = "";
            var last = newSegments.Last();
            bool isNewSegement = true;
            foreach (string segTitle in newSegments)
            {
                isNewSegement = true;
                if (segTitle == last)
                {
                    // We don't put the separator on the end of the last item
                    segString += segTitle + "/1";
                }
                else
                {
                    segString += segTitle + "/1|";
                }

                foreach (string segOldTitle in oldSegments)
                {
                    if (segOldTitle.Contains(segTitle))
                    {
                        isNewSegement = false;
                        var titleAndOrder = segOldTitle.Split("/");
                        //If the new segment is at order 1 don't update anything
                        if (titleAndOrder[1] != "1")
                        {
                            UpdateSegmentsAsThereWasAnInsertAt1(segTitle);
                        }
                    }
                }
                if (isNewSegement)
                {
                    // If this is a totally new segment we are adding the article to then all the others need to be decremented as it's going in at 1
                    UpdateSegmentsAsThereWasAnInsertAt1(segTitle);
                }
            }
            return segString;
        }



        public void UpdateSegmentsAsThereWasAnInsertAt1(string segmentTitleWhereInsertHappened)
        {
            //Get all the company segments

            var id = HttpContext.Session.GetInt32("CompanyID");
            var ctx = _context.SegmentArticle
                .Where(m => m.CompanyID == id && m.Segments.Contains(segmentTitleWhereInsertHappened))
                .ToList();

            foreach (SegmentArticle item in ctx)
            {
                var updatedSegmentsAndOrders = "";
                //Split the segments into a list, grab their order and increment it then save the change
                var segments = item.Segments;
                var segmentsList = item.Segments.Split("|");
                foreach (string segmentTitleAndOrder in segmentsList)
                {
                    var ender = "";
                    // If this isn't the last title, add a delimited
                    if (segmentsList.Last() != segmentTitleAndOrder) ender = "|";

                    if (segmentTitleAndOrder.Contains(segmentTitleWhereInsertHappened))
                    {
                        //Get the order and increment

                        var titleAndOrder = segmentTitleAndOrder.Split("/");
                        bool success = int.TryParse(titleAndOrder[1], out int ord);
                        ord++;
                        updatedSegmentsAndOrders += titleAndOrder[0] + "/" + ord + ender;
                    }
                    else
                    {
                        // just add it unchanged 
                        updatedSegmentsAndOrders += segmentTitleAndOrder + ender;
                    }

                }
                item.Segments = updatedSegmentsAndOrders;
                _context.Update(item);
            }

            _context.SaveChanges();
        }


        // POST: SegmentArticles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [DisableRequestSizeLimit]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SegmentArticleViewModel sa)
        {
            _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            var myArticle = new SegmentArticle();

            //Have to check the article title isn't already used
            var existingArticle = _context.SegmentArticle.FirstOrDefault(n => n.Title == sa.Title && n.CompanyID == sa.CompanyID);
            if (existingArticle == null)
            {

                if (ModelState.IsValid)
                {
                    //ID,Order,Title,CoverImage,CoverImageMobileFriendly,PublishDate,FinishDate,ClientName,ClientLogoImage,ThemeColor,StandardColor,Draft,Department,Tags
                    myArticle.CompanyID = _user.companyID;
                   
                    myArticle.Title = WebUtility.HtmlEncode(sa.Title);
                    myArticle.PublishFrom = sa.PublishFrom ?? DateTime.Now.AddDays(-1);
                    myArticle.PublishTill = sa.PublishTill ?? DateTime.Now.AddYears(10);
                    myArticle.AllowComments = sa.AllowComments;
                    myArticle.ArticleContent = WebUtility.HtmlEncode(sa.ArticleContent);
                    myArticle.Author = (sa.Author ?? _user.name); //if null set to be user name
                    myArticle.CreatedBy = _user.UserName;
                    myArticle.ArticleUrl = WebUtility.HtmlEncode(sa.ArticleUrl);

                    myArticle.Tags = WebUtility.HtmlEncode(sa.Tags);

                    myArticle.Languages = string.Join("|", sa.SelectedLanguages);
                    myArticle.Groups = string.Join("|", sa.SelectedGroups);
                    myArticle.Topics = string.Join("|", sa.SelectedTopics);
                    myArticle.TypeOfUser = string.Join("|", sa.SelectedTypeOfUser);
                    if (sa.Country != null && sa.Country != "") myArticle.Country = _context.LocationCountries.FirstOrDefault(m => m.country_code == sa.Country).country_name;
                    if (sa.State != null && sa.State != "") myArticle.State = _context.LocationStates.FirstOrDefault(n => n.country_code == sa.Country && n.state_code == sa.State).state_name;
                    myArticle.City = sa.City;

                    //Now we need to get what the article ID will be as it isn't normally generated till created.
                    int artID = _context.SegmentArticle.OrderByDescending(u => u.ID).FirstOrDefault().ID + 1;

                    IFormFile img = sa.Image;
                    if (img != null)
                    {      
                        await _dla.UploadFileToS3(img, _user.companyName + "_" + artID + "_" + img.FileName, "images/Uploads/Articles");
                        //The file has been saved to disk - now save the file name to the DB
                        myArticle.Image = img.FileName;
                    }

                    IFormFile vid = sa.EmbeddedVideo;
                    if (vid != null)
                    {
                        await _dla.UploadFileToS3(vid, _user.companyName + "_" + artID + "_" + vid.FileName, "images/Uploads/Articles");
                        //The file has been saved to disk - now save the file name to the DB
                        myArticle.EmbeddedVideo = vid.FileName;
                    }

                    // As they want order to be associated with segment now im having to smush it in with the current field....a bit ugly 
                    if (sa.SelectedSegments.Count > 0)
                    {
                        myArticle.Segments = CreateSegmentsStringWithOrder1(sa.SelectedSegments);
                    }

                    _context.Add(myArticle);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                
            }
            else
            {
                // Article title already exists
                ModelState.AddModelError(string.Empty, "Article Title already exists - please choose something unique");
            }
            DataAccessLayer dla = new DataAccessLayer(_context);

            var listOfAllSegements = dla.GetArticleSegments(_user.companyID);
            foreach (var seg in listOfAllSegements)
            {
                sa.AvailableSegments.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
            }

            // Add group checkboxes
            var listOfAllGroups = dla.GetListGroups(_user.companyID);
            foreach (var seg in listOfAllGroups)
            {
                sa.AvailableGroups.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
            }

            // Add topic checkboxes
            var listOfAllTopics = dla.GetListTopics(_user.companyID);
            foreach (var seg in listOfAllTopics)
            {
                sa.AvailableTopics.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
            }

            // Add type checkboxes
            var listOfAllTypeOfUser = dla.GetListTypeOfUser(_user.companyID);
            foreach (var seg in listOfAllTypeOfUser)
            {
                sa.AvailableTypeOfUser.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
            }

            // Add language checkboxes
            var listOfAllLanguages = dla.GetListLanguages(_user.companyID, _rlo);
            foreach (var seg in listOfAllLanguages)
            {
                sa.AvailableLanguages.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
            }

            // Get location dropdown data
            sa.Countries = dla.GetCountries();
            sa.States = dla.GetNullStatesOrCities();
            sa.Cities = dla.GetNullStatesOrCities();
            return View(sa);
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

            DataAccessLayer dla = new DataAccessLayer(_context);
            _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            SegmentArticleViewModel s = new SegmentArticleViewModel();
            s.Order = segmentArticle.Order;
            s.ArticleUrl = WebUtility.HtmlDecode(segmentArticle.ArticleUrl);
            s.Title = WebUtility.HtmlDecode(segmentArticle.Title);
            //s.Image = segmentArticle.Image; TODO still need to fix this crap
            //s.EmbeddedVideo = segmentArticle.EmbeddedVideo;
            s.ArticleContent = WebUtility.HtmlDecode(segmentArticle.ArticleContent);
            s.PublishFrom = segmentArticle.PublishFrom;
            s.PublishTill = segmentArticle.PublishTill;
            s.CompanyID = segmentArticle.CompanyID;
            s.Author = (segmentArticle.Author == null || segmentArticle.Author == "" ? _user.name : segmentArticle.Author);
            s.AllowComments = segmentArticle.AllowComments;
            s.Tags = WebUtility.HtmlDecode(segmentArticle.Tags);
            s.CreatedBy = _user.UserName;

            // Get location dropdown data
            s.Countries = dla.GetCountries();
            s.States = dla.GetNullStatesOrCities();
            s.Cities = dla.GetNullStatesOrCities();


            if (segmentArticle.Segments != null && segmentArticle.Segments != "")
            {
                s.AvailableSegments = RemoveDigitsAndSlashes(segmentArticle.Segments).Split('|').Select(x => new SelectListItem { Text = x, Value = x, Selected = true }).ToList();
            }
            //Add any options not already in the segmentlist

            var listOfAllSegements = dla.GetArticleSegments(_user.companyID);
            foreach (var seg in listOfAllSegements)
            {
                var match = s.AvailableSegments.FirstOrDefault(stringToCheck => stringToCheck.Value.Contains(seg.Value));

                if (match == null)
                {
                    //if no match (not in the list) then add it
                    s.AvailableSegments.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                }
            }

            if (segmentArticle.Groups != null && segmentArticle.Groups != "")
            {
                s.AvailableGroups = segmentArticle.Groups.Split('|').Select(x => new SelectListItem { Text = x, Value = x, Selected = true }).ToList();
            }
            //Add any options not already in the segmentlist
            var listOfAllGroups = dla.GetListGroups(_user.companyID);
            foreach (var seg in listOfAllGroups)
            {
                if (s.AvailableGroups.FirstOrDefault(stringToCheck => stringToCheck.Value.Contains(seg.Value)) == null)
                {
                    //if no match (not in the list) then add it
                    s.AvailableGroups.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                }
            }


            if (segmentArticle.Topics != null && segmentArticle.Topics != "")
            {
                s.AvailableTopics = segmentArticle.Topics.Split('|').Select(x => new SelectListItem { Text = x, Value = x, Selected = true }).ToList();
            }
            //Add any options not already in the segmentlist
            var listOfAllTopics = dla.GetListTopics(_user.companyID);
            foreach (var seg in listOfAllTopics)
            {
                if (s.AvailableTopics.FirstOrDefault(stringToCheck => stringToCheck.Value.Contains(seg.Value)) == null)
                {
                    //if no match (not in the list) then add it
                    s.AvailableTopics.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                }
            }

            if (segmentArticle.TypeOfUser != null && segmentArticle.TypeOfUser != "")
            {
                s.AvailableTypeOfUser = segmentArticle.TypeOfUser.Split('|').Select(x => new SelectListItem { Text = x, Value = x, Selected = true }).ToList();
            }
            //Add any options not already in the segmentlist
            var listOfAllTypeOfUser = dla.GetListTypeOfUser(_user.companyID);
            foreach (var seg in listOfAllTypeOfUser)
            {
                if (s.AvailableTypeOfUser.FirstOrDefault(stringToCheck => stringToCheck.Value.Contains(seg.Value)) == null)
                {
                    //if no match (not in the list) then add it
                    s.AvailableTypeOfUser.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                }
            }

            if (segmentArticle.Languages != null && segmentArticle.Languages != "")
            {
                s.AvailableLanguages = segmentArticle.Languages.Split('|').Select(x => new SelectListItem { Text = x, Value = x, Selected = true }).ToList();
            }
            //Add any options not already in the segmentlist
            var listOfAllLanguages = dla.GetListLanguages(_user.companyID, _rlo);
            foreach (var seg in listOfAllLanguages)
            {
                if (s.AvailableLanguages.FirstOrDefault(stringToCheck => stringToCheck.Value.Contains(seg.Value)) == null)
                {
                    //if no match (not in the list) then add it
                    s.AvailableLanguages.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                }
            }

            return View(s);
        }

        public string RemoveDigitsAndSlashes(string input)
        {
            var output = Regex.Replace(input, @"[\d-]", string.Empty).Replace("/", "");
            return output;
        }

        // POST: SegmentArticles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [DisableRequestSizeLimit]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SegmentArticleViewModel sa)
        {
            if (id != sa.ID)
            {
                return NotFound();
            }

            _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            var myArticle = _context.SegmentArticle.Find(id);

            //Have to check the article title isn't already used
            var existingArticle = _context.SegmentArticle.FirstOrDefault(n => n.Title == sa.Title && n.CompanyID == sa.CompanyID);
            if (existingArticle == null)
            {

                if (ModelState.IsValid)
                {
                    myArticle.ID = sa.ID;
                    myArticle.CompanyID = _user.companyID;
                    myArticle.Order = sa.Order ?? 1;
                    myArticle.Title = WebUtility.HtmlEncode(sa.Title);

                    myArticle.PublishFrom = sa.PublishFrom ?? DateTime.Now.AddDays(-1);
                    myArticle.PublishTill = sa.PublishTill ?? DateTime.Now.AddYears(10);
                   
                    myArticle.AllowComments = sa.AllowComments;
                    myArticle.ArticleContent = WebUtility.HtmlEncode(sa.ArticleContent);
                    myArticle.Author = sa.Author;


                    myArticle.Tags = WebUtility.HtmlEncode(sa.Tags);
                    myArticle.ArticleUrl = WebUtility.HtmlEncode(sa.ArticleUrl);

                    myArticle.Languages = string.Join("|", sa.SelectedLanguages);
                    myArticle.Groups = string.Join("|", sa.SelectedGroups);
                    myArticle.Topics = string.Join("|", sa.SelectedTopics);
                    myArticle.TypeOfUser = string.Join("|", sa.SelectedTypeOfUser);
                    myArticle.City = sa.City;
                    if (sa.Country != null && sa.Country != "") myArticle.Country = _context.LocationCountries.FirstOrDefault(m => m.country_code == sa.Country).country_name;
                    if (sa.State != null && sa.State != "") myArticle.State = _context.LocationStates.FirstOrDefault(n => n.country_code == sa.Country && n.state_code == sa.State).state_name;

                    IFormFile img = sa.Image;
                    if (img != null)
                    {
                        await _dla.UploadFileToS3(img, _user.companyName + "_" + id + "_" + img.FileName, "images/Uploads/Articles");
                        //The file has been saved to disk - now save the file name to the DB
                        myArticle.Image = img.FileName;
                    }

                    IFormFile vid = sa.EmbeddedVideo;
                    if (vid != null)
                    {
                        await _dla.UploadFileToS3(vid, _user.companyName + "_" + id + "_" + vid.FileName, "images/Uploads/Articles");                       
                        //The file has been saved to disk - now save the file name to the DB
                        myArticle.EmbeddedVideo = vid.FileName;
                    }
                    else
                    {
                        myArticle.EmbeddedVideo = "";
                    }

                    try
                    {
                        // If the article has some segments set
                        if (!string.IsNullOrEmpty(myArticle.Segments))
                        {
                            var oldSegments = myArticle.Segments.Split("|");
                            //Even though it is an edit we are resetting the order to 1...its waaay to complicated otherwise.
                            myArticle.Segments = CreateSegmentsStringWithOrder1ButWithCheckToMakeSureNotAt1Already(sa.SelectedSegments, oldSegments);
                        }
                        _context.Update(myArticle);
                        await _context.SaveChangesAsync();

                        //update the selected article segments

                        //DataAccessLayer dla = new DataAccessLayer(_context, _user.companyID);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SegmentArticleExists(myArticle.ID))
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
                else
                {
                    // Article title already exists
                    ModelState.AddModelError(string.Empty, "Article Title already exists - please choose something unique");
                }
            }
             return RedirectToAction("Edit", new { id = myArticle.ID });
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
            DataAccessLayer dla = new DataAccessLayer(_context);

            // OK no need to find the segment. They can remove from a segment but delete removes from all segments and deletes article.
            // We do however need to increment the order of all articles in that segment below the current order.

            var segments = segmentArticle.Segments;
            if (!string.IsNullOrEmpty(segments))
            {
                var segmentsList = segmentArticle.Segments.Split("|");
                foreach (string segmentArticleIsIn in segmentsList)
                {
                    var titleAndOrder = segmentArticleIsIn.Split("/");
                    var success2 = int.TryParse(titleAndOrder[1], out int ord);
                    dla.DeleteArticleAndUpdateOthersOrder(segmentArticle, titleAndOrder[0], ord);
                }
            }
               
            _context.SegmentArticle.Remove(segmentArticle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // POST: SegmentArticles/Delete/5

        public async Task<IActionResult> RemoveFromMagazine(int id)
        {
            var segmentArticle = await _context.SegmentArticle.FindAsync(id);
            DataAccessLayer dla = new DataAccessLayer(_context);

            var segID = HttpContext.Session.GetInt32("SegmentID") ?? 0;
            CompanySegment cs = await _context.CompanySegment.FindAsync(segID);

            dla.DeleteArticleAndUpdateOthersOrder(segmentArticle, cs.Title);
            return RedirectToAction("Details", "CompanySegments", new { id = segID });
        }

        private bool SegmentArticleExists(int id)
        {
            return _context.SegmentArticle.Any(e => e.ID == id);
        }

        [HttpGet]
        public JsonResult GetStates(string countryCode)
        {
            if (!string.IsNullOrWhiteSpace(countryCode) && countryCode.Length == 2)
            {
                HttpContext.Session.SetString("countryCode", countryCode);
                DataAccessLayer dla = new DataAccessLayer(_context);
                IEnumerable<SelectListItem> states = dla.GetStates(countryCode);
                return Json(states);
            }
            return null;
        }

        [HttpGet]
        public JsonResult GetCities(string stateCode)
        {
            if (!string.IsNullOrWhiteSpace(stateCode))
            {
                string countryCode = HttpContext.Session.GetString("countryCode");
                DataAccessLayer dla = new DataAccessLayer(_context);
                IEnumerable<SelectListItem> cities = dla.GetCities(countryCode, stateCode);
                return Json(cities);
            }
            return null;
        }
    }
}
