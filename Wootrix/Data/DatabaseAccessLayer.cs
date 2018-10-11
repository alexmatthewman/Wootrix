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

        public DatabaseAccessLayer(ApplicationDbContext context, int companyID)
        {
            _context = context;
            _companyID = companyID;
        }

        public IEnumerable<SelectListItem> GetDepartments()
        {
            using (var context = _context)
            {
                List<SelectListItem> deps = context.CompanyDepartments.AsNoTracking()
                .Where(n => n.CompanyID == _companyID)
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
            }
        }

        public List<SelectListItem> GetGroups()
        {
            using (var context = _context)
            {
                List<SelectListItem> deps = context.CompanyGroups.AsNoTracking()
                .Where(n => n.CompanyID == _companyID)
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

        public List<SelectListItem> GetArticleSegments()
        {
            using (var context = _context)
            {
                List<SelectListItem> deps = context.CompanyGroups.AsNoTracking()
                .Where(n => n.CompanyID == _companyID)
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



    }
}
