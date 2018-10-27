using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wootrix.Data;
using WootrixV2.Models;

namespace WootrixV2.Data
{
    public class DatabaseAccessLayer
    {

        private readonly ApplicationDbContext _context;
        private int _companyID;

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

        public List<SelectListItem> GetListLanguages(int companyID)
        {
            List<SelectListItem> list = _context.CompanyLanguages.AsNoTracking()
            .Where(n => n.CompanyID == companyID)
                .OrderBy(n => n.LanguageName)
                    .Select(n =>
                    new SelectListItem
                    {
                        Value = n.LanguageName,
                        Text = n.LanguageName
                    }).ToList();

            return list;
        }


        public List<SelectListItem> GetArticleSegments(int companyID)
        {
            using (var context = _context)
            {
                List<SelectListItem> deps = context.CompanySegment.AsNoTracking()
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

        public List<WootrixV2.Models.CompanySegment> GetSegmentsList(int companyID)
        {

            List<WootrixV2.Models.CompanySegment> segments = _context.CompanySegment.AsNoTracking()
            .Where(n => n.CompanyID == companyID)
                .OrderBy(n => n.Order)
                    .ToList();
            //TODO Add all the user and article filtering here
            return segments;
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
    }
}
