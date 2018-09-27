using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class CompanyMagazine
    {
        [Key]
        public int ID { get; set; }

        public int CompanyID { get; set; }

        public string MagazineTitle { get; set; }

        public byte[] MagazineCoverImage { get; set; }

        public List<CompanyGroups> CompanyGroups { get; set; }

        public List<CompanyLanguages> CompanyLanguages { get; set; }

        public List<CompanyLocations> CompanyLocations { get; set; }

        public List<MagazineArticle> MagazineArticles { get; set; }

    }
}
