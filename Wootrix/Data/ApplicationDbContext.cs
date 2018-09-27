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

        public DbSet<WootrixV2.Models.CompanyGroups> CompanyGroups { get; set; }
        public DbSet<WootrixV2.Models.CompanyLanguages> CompanyLanguages { get; set; }
        public DbSet<WootrixV2.Models.CompanyLocations> CompanyLocations { get; set; }
        public DbSet<WootrixV2.Models.CompanyMagazine> CompanyMagazine { get; set; }
        public DbSet<WootrixV2.Models.CompanyPushNotification> CompanyPushNotification { get; set; }
        public DbSet<WootrixV2.Models.MagazineArticle> MagazineArticle { get; set; }
        public DbSet<WootrixV2.Models.MagazineArticleComment> MagazineArticleComment { get; set; }
        
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
