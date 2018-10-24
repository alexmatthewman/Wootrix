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
           var commentCount =  _context.SegmentArticleComment
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


    }
}
