using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WootrixV2.Data;
using WootrixV2.Models;

namespace Wootrix.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<WootrixV2.Models.Company> Company { get; set; }
        public DbSet<WootrixV2.Models.CompanyDepartments> CompanyDepartments { get; set; }
        public DbSet<WootrixV2.Models.CompanyGroups> CompanyGroups { get; set; }
        public DbSet<WootrixV2.Models.CompanyLocCountries> CompanyLocCountries { get; set; }
        public DbSet<WootrixV2.Models.CompanyLocStates> CompanyLocStates { get; set; }
        public DbSet<WootrixV2.Models.CompanyLocCities> CompanyLocCities { get; set; }
        public DbSet<WootrixV2.Models.CompanyPushNotification> CompanyPushNotification { get; set; }
        public DbSet<WootrixV2.Models.CompanySegment> CompanySegment { get; set; }
        public DbSet<WootrixV2.Models.CompanyTopics> CompanyTopics { get; set; }
        public DbSet<WootrixV2.Models.CompanyTypeOfUser> CompanyTypeOfUser { get; set; }
        public DbSet<WootrixV2.Models.SegmentArticle> SegmentArticle { get; set; }
        public DbSet<WootrixV2.Models.SegmentArticleComment> SegmentArticleComment { get; set; }

        public DbSet<WootrixV2.Models.SegmentArticleSegments> SegmentArticleSegments { get; set; }
        
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
