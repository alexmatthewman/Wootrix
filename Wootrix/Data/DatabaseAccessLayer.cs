using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        
    }
}
