using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wootrix.Data;
using Microsoft.AspNetCore.Http.Features;
using WootrixV2.Models;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace WootrixV2.Data
{
    public class DatabaseAccessLayer
    {

        private readonly ApplicationDbContext _context;

        public DatabaseAccessLayer(ApplicationDbContext context)
        {
            _context = context;
        }


        public IEnumerable<SelectListItem> GetCountries()
        {
            List<SelectListItem> item = _context.CompanyLocCountries.AsNoTracking()
                .OrderBy(n => n.country_name)
                    .Select(n =>
                    new SelectListItem
                    {
                        Value = n.country_code,
                        Text = n.country_name
                    }).ToList();
            var itemTop = new SelectListItem()
            {
                Value = null,
                Text = "--- Select Country ---"
            };
            item.Insert(0, itemTop);
            return new SelectList(item, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetNullStatesOrCities()
        {
            List<SelectListItem> item = new List<SelectListItem>();
            var itemTop = new SelectListItem()
            {
                Value = null,
                Text = "--- Select Country ---"
            };
            item.Insert(0, itemTop);
            return new SelectList(item, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetStates(string countyCode)
        {
            if (!String.IsNullOrWhiteSpace(countyCode))
            {
                IEnumerable<SelectListItem> states = _context.CompanyLocStates.AsNoTracking()
                    .OrderBy(n => n.state_name)
                    .Where(n => n.country_code == countyCode)
                    .Select(n =>
                       new SelectListItem
                       {
                           Value = n.state_code,
                           Text = n.state_name
                       }).ToList();
                return new SelectList(states, "Value", "Text");
            }
            return null;
        }


        public IEnumerable<SelectListItem> GetCities(string countryCode, string stateCode)
        {
            if (!String.IsNullOrWhiteSpace(stateCode))
            {
                IEnumerable<SelectListItem> cities = _context.CompanyLocCities.AsNoTracking()
                    .OrderBy(n => n.city_name_ascii)
                    .Where(n => n.country_code == countryCode)
                    .Where(n => n.state_code == stateCode)
                    .Select(n =>
                       new SelectListItem
                       {
                           Value = n.city_name_ascii,
                           Text = n.city_name_ascii
                       }).ToList();
                return new SelectList(cities, "Value", "Text");
            }
            return null;
        }



        public IEnumerable<SelectListItem> GetDepartments(int companyID)
        {
            //using auto disposes the object (contect - which we still need) so remove it
            //using (var context = _context)
            //{
            List<SelectListItem> deps = _context.CompanyDepartments.AsNoTracking()
            .Where(n => n.CompanyID == companyID)
                .OrderBy(n => n.CompanyDepartmentName)
                    .Select(n =>
                    new SelectListItem
                    {
                        Value = n.ID.ToString(),
                        Text = n.CompanyDepartmentName
                    }).ToList();
            var depstip = new SelectListItem()
            {
                Value = null,
                Text = "--- Select Department ---"
            };
            deps.Insert(0, depstip);
            return new SelectList(deps, "Value", "Text");
            //}
        }

        public IEnumerable<SelectListItem> GetInterfaceLanguages(int companyID, IOptions<RequestLocalizationOptions> rlo)
        {
            var cultureItems = rlo.Value.SupportedUICultures
                .Select(c => new SelectListItem { Value = c.DisplayName, Text = c.DisplayName })
                .ToList();

            return cultureItems;
        }


        public IEnumerable<SelectListItem> GetGenders()
        {
            List<SelectListItem> gen = new List<SelectListItem>();
            var gentip = new SelectListItem()
            {
                Value = "Not Identified",
                Text = "Not Identified"
            };
            var gen1 = new SelectListItem()
            {
                Value = "Male",
                Text = "Male"
            };
            var gen2 = new SelectListItem()
            {
                Value = "Female",
                Text = "Female"
            };
            var gen3 = new SelectListItem()
            {
                Value = "Other",
                Text = "Other"
            };
            gen.Insert(0, gentip);
            gen.Insert(1, gen1);
            gen.Insert(2, gen2);
            gen.Insert(3, gen3);
            return new SelectList(gen, "Value", "Text");

        }



        public IEnumerable<SelectListItem> GetCompanies()
        {
            //using (var context = _context)
            //{
            List<SelectListItem> deps = _context.Company.AsNoTracking()
                .OrderBy(n => n.CompanyName)
                    .Select(n =>
                    new SelectListItem
                    {
                        Value = n.ID.ToString(),
                        Text = n.CompanyName
                    }).ToList();
            var depstip = new SelectListItem()
            {
                Value = null,
                Text = "--- Select Company ---"
            };
            deps.Insert(0, depstip);
            return new SelectList(deps, "Value", "Text");
            //}
        }


        public List<SelectListItem> GetGroups(int companyID)
        {
            using (var context = _context)
            {
                List<SelectListItem> deps = context.CompanyGroups.AsNoTracking()
                .Where(n => n.CompanyID == companyID)
                    .OrderBy(n => n.GroupName)
                        .Select(n =>
                        new SelectListItem
                        {
                            Value = n.ID.ToString(),
                            Text = n.GroupName
                        }).ToList();
                var depstip = new SelectListItem()
                {
                    Value = null,
                    Text = "--- Select Groups ---"
                };
                deps.Insert(0, depstip);
                return deps;
            }
        }

        public List<SelectListItem> GetListGroups(int companyID)
        {
            List<SelectListItem> list = _context.CompanyGroups.AsNoTracking()
            .Where(n => n.CompanyID == companyID)
                .OrderBy(n => n.GroupName)
                    .Select(n =>
                    new SelectListItem
                    {
                        //Value = n.ID.ToString(),
                        Value = n.GroupName,
                        Text = n.GroupName
                    }).ToList();

            return list;
        }

        public List<SelectListItem> GetListTopics(int companyID)
        {
            List<SelectListItem> list = _context.CompanyTopics.AsNoTracking()
            .Where(n => n.CompanyID == companyID)
                .OrderBy(n => n.Topic)
                    .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Topic,
                        Text = n.Topic
                    }).ToList();

            return list;
        }

        public List<SelectListItem> GetListTypeOfUser(int companyID)
        {
            List<SelectListItem> list = _context.CompanyTypeOfUser.AsNoTracking()
            .Where(n => n.CompanyID == companyID)
                .OrderBy(n => n.TypeOfUser)
                    .Select(n =>
                    new SelectListItem
                    {
                        Value = n.TypeOfUser,
                        Text = n.TypeOfUser
                    }).ToList();

            return list;
        }



        public List<SelectListItem> GetListLanguages(int companyID, IOptions<RequestLocalizationOptions> rlo)
        {

            List<SelectListItem> list = rlo.Value.SupportedUICultures
                .Select(c => new SelectListItem { Value = c.DisplayName, Text = c.DisplayName })
                .ToList();

            return list;
        }


        public List<SelectListItem> GetArticleSegments(int companyID)
        {
            List<SelectListItem> deps = _context.CompanySegment.AsNoTracking()
            .Where(n => n.CompanyID == companyID)
                .OrderBy(n => n.Title)
                    .Select(n =>
                    new SelectListItem
                    {
                        //Value = n.ID.ToString(),
                        Value = n.Title,
                        Text = n.Title
                    }).ToList();

            return deps;

        }


        public List<WootrixV2.Models.SegmentArticle> GetArticlesList(int companyID)
        {

            List<WootrixV2.Models.SegmentArticle> articles = _context.SegmentArticle.AsNoTracking()
            .Where(n => n.CompanyID == companyID)
                .OrderBy(n => n.Order)
                    .ToList();
            //TODO Add all the user filtering here
            return articles;

        }

        public List<WootrixV2.Models.CompanySegment> GetSegmentsList(int companyID, User usr, string segmentSearchString, string articleSearchString)
        {
            // Have to do this backwards - get all the articles the user can see, then get a list of all segments which have articles
            List<CompanySegment> segments = new List<CompanySegment>();
            List<SegmentArticle> articles = new List<SegmentArticle>();

            // Our filters are Country, State, City, Language, Topics, Groups, TypeOfUser....also publish date
            // IF an article has a country set, only show it if the user also has it set
            // OR IF neither article or user have it set, show it 

            //Allow for searches too
            var setOfArticles = _context.SegmentArticle.ToList();
            if (!string.IsNullOrEmpty(articleSearchString))
            {

                setOfArticles = _context.SegmentArticle
                    .Where(n => n.CompanyID == companyID)
                    .Where(m => (m.Title.Contains(articleSearchString) || m.Tags.Contains(articleSearchString))).ToList();
            }

            foreach (SegmentArticle art in _context.SegmentArticle)
            {
                if (art.CompanyID == usr.CompanyID)
                {
                    // If the article is published
                    if (art.PublishFrom < DateTime.Now)
                    {
                        // Should work for null == null too I think
                        if (art.Country == usr.Country
                            && art.State == usr.State
                            && art.City == usr.City)
                        {
                            // For groups, If both are null or equal it's fine. If not cycle through the user Groups and if the Article groups match one then if it allowed
                            if (PassesFilter(art.Groups, usr.Groups) && PassesFilter(art.TypeOfUser, usr.TypeOfUser) && PassesFilter(art.Topics, usr.Topics) && PassesFilter(art.Languages, usr.WebsiteLanguage))
                            {
                                articles.Add(art);
                            }
                        }


                    }
                }

            }

            // So now we have the articles that are valid for the user
            // We need to loop through them and for each Segement found, if it is not already in the list, add it
            foreach (SegmentArticle item in articles)
            {
                var articleSegments = item.Segments.Split(',').ToList();
                foreach (var segmentTitle in articleSegments)
                {
                    // Check if the segment title is in the existing segment list
                    if (segments.FirstOrDefault(p => p.Title == segmentTitle) == null)
                    {
                        // Not in list so add it
                        if (string.IsNullOrEmpty(segmentSearchString))
                        {
                            // No search filter
                            segments.Add(_context.CompanySegment.FirstOrDefault(p => p.Title == segmentTitle));

                        }
                        else
                        {
                            //Have to filter on search string too
                            var seg = _context.CompanySegment.FirstOrDefault(p => p.Title == segmentTitle && (p.Title.Contains(segmentSearchString) || p.Tags.Contains(segmentSearchString)));
                            if (seg != null) segments.Add(seg);
                        }
                    }
                }
            }

            //TODO Add all the user and article filtering here
            return segments;
        }

        public bool PassesFilter(string articleCSVFilter, string userCSVFilter)
        {
            bool isInFilter = false;
            // For groups, If both are null or equal it's fine. If not cycle through the user Groups and if the Article groups match one then if it allowed
            if (string.IsNullOrEmpty(articleCSVFilter) && string.IsNullOrEmpty(userCSVFilter)) return true;

            var filterList = userCSVFilter.Split(',').ToList();
            foreach (var filter in filterList)
            {
                if (articleCSVFilter != null)
                {
                    if (articleCSVFilter.Contains(filter))
                    {
                        // Crap have to do this stupid loop for each type of filter - at least it should be fast
                        return true;
                    }
                }
            }

            return isInFilter;
        }

        public List<WootrixV2.Models.SegmentArticleComment> GetArticleCommentsList(int ArticleID)
        {

            List<WootrixV2.Models.SegmentArticleComment> comments = _context.SegmentArticleComment.AsNoTracking()
            .Where(n => n.SegmentArticleID == ArticleID && n.Status == "Approved")
                .OrderBy(n => n.CreatedDate)
                    .ToList();
            //TODO Add all the user and article filtering here
            return comments;
        }

        /// <summary>
        /// Get the approved comments count for the article 
        /// </summary>
        /// <param name="ArticleID">The article to get the comment count for</param>
        /// <returns></returns>
        public int GetArticleApprovedCommentCount(int ArticleID)
        {
            var commentCount = _context.SegmentArticleComment
                 .Where(m => m.SegmentArticleID == ArticleID && m.Status == "Approved")
                 .Count();
            return commentCount;
        }

        /// <summary>
        /// Get the "Review" status comments count for the company 
        /// </summary>
        /// <param name="ArticleID">The article to get the comment count for</param>
        /// <returns></returns>
        public int GetArticleReviewCommentCount(int CompanyID)
        {
            var commentCount = _context.SegmentArticleComment
                 .Where(m => m.CompanyID == CompanyID && m.Status == "Review")
                 .Count();
            return commentCount;
        }

        public string GetCompanyDepartmentName(string DepartmentID)
        {
            CompanyDepartments cd = _context.CompanyDepartments.AsNoTracking().FirstOrDefault(n => n.ID.ToString() == DepartmentID);
            return cd.CompanyDepartmentName;
        }

        public string GetCountryName(string CountryCode)
        {
            List<SelectListItem> item = _context.CompanyLocCountries.AsNoTracking()
               .OrderBy(n => n.country_name)
               .Where(n => n.country_code == CountryCode)
                   .Select(n =>
                   new SelectListItem
                   {
                       Value = n.country_code,
                       Text = n.country_name
                   }).ToList();
            var cz = _context.CompanyLocCountries.Find(CountryCode);
            var cq = _context.CompanyLocCountries.Where(m => m.country_code == CountryCode);
            var ct = _context.CompanyLocCountries.AsNoTracking().FirstOrDefault(m => m.country_code == CountryCode);
            return ct.country_name;
        }

        public string GetStateName(string StateCode)
        {
            var cs = _context.CompanyLocStates.AsNoTracking().FirstOrDefault(p => p.state_code == StateCode);
            return cs.state_name;
        }

        public void RemoveSegmentFromArticleSegments(SegmentArticle art, string segmentTitle)
        {
            var updatedSegmentsAndOrders = "";
            //Split the segments into a list, grab their order and increment it then save the change
            var segments = art.Segments;
            var segmentsList = art.Segments.Split("|");
            foreach (string segmentTitleAndOrder in segmentsList)
            {
                if (!segmentTitleAndOrder.Contains(segmentTitle))
                {

                    updatedSegmentsAndOrders += segmentTitleAndOrder;
                }
            }
            art.Segments = updatedSegmentsAndOrders;
            _context.Update(art);
            _context.SaveChanges();
        }

        // Decrement everything below it
        public void DeleteArticleAndUpdateOthersOrder(SegmentArticle article, string segmentTitle)
        {
            foreach (var art in _context.SegmentArticle.Where(m => m.CompanyID == article.CompanyID && m.Order > article.Order))
            {
                var updatedSegmentsAndOrders = "";
                //Split the segments into a list, grab their order and increment it then save the change
                var segments = art.Segments;
                var segmentsList = art.Segments.Split("|");
                foreach (string segmentTitleAndOrder in segmentsList)
                {
                    var ender = "";
                    // If this isn't the last title, add a delimited
                    if (segmentsList.Last() != segmentTitleAndOrder) ender = "|";

                    if (segmentTitleAndOrder.Contains(segmentTitle))
                    {
                        //Get the order and increment
                        var titleAndOrder = segmentTitleAndOrder.Split("/");
                        int ord;
                        bool success2 = int.TryParse(titleAndOrder[1], out ord);
                        ord--;
                        updatedSegmentsAndOrders += titleAndOrder[0] + "/" + ord + ender;
                    }
                    else
                    {
                        // just add it unchanged 
                        updatedSegmentsAndOrders += segmentTitleAndOrder + ender;
                    }
                }
                art.Segments = updatedSegmentsAndOrders;
                art.Order--;
                _context.Update(art);
            }

            // Remove the current segement and order from the passed article
            RemoveSegmentFromArticleSegments(article, segmentTitle);
            _context.SaveChanges();
        }

    }
}
