using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class SegmentArticle
    {
        [Key]
        public int ID { get; set; }

        public int CompanySegmentID { get; set; }

        public string title { get; set; }

        public string image { get; set; }

        public int embedVideoLink { get; set; }

        public string description { get; set; }

        public string link { get; set; }

        public DateTime publishDate { get; set; }

        public string tags { get; set; }

        public int allowComments { get; set; }

        public int allowShare { get; set; }

        public DateTime publishFrom { get; set; }

        public DateTime publishTill { get; set; }

        public string author { get; set; }

        public List<CompanyGroups> CompanyGroups { get; set; }

        public List<CompanyLanguages> CompanyLanguages { get; set; }

        public List<CompanyLocations> CompanyLocations { get; set; }

        public List<CompanyTypeOfUser> CompanyTypeOfUser { get; set; }

        public List<CompanyCountries> CompanyCountries { get; set; }

        public List<SegmentArticleComment> SegmentArticleComment { get; set; }

    }
}
