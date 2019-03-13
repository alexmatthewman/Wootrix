using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class ArticleReporting
    {
        [Key]
        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        [ScaffoldColumn(false)]
        public int UserID { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [ScaffoldColumn(false)]
        public int SegmentID { get; set; }

        [Display(Name = "Magazine Name")]
        public string SegmentName { get; set; }

        [ScaffoldColumn(false)]
        public int ArticleID { get; set; }
        
        [Display(Name = "Article Name")]
        public string ArticleName { get; set; }

        [Display(Name = "Device Type")]
        public string DeviceType { get; set; }

        [Display(Name = "OS Type")]
        public string OSType { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy MMM dd}")]
        [Display(Name = "Article Viewed On")]
        public DateTime ArticleReadTime { get; set; }

        [Display(Name = "Country", Prompt = "Country", Description = "Country")]
        public string Country { get; set; }

        [Display(Name = "State", Prompt = "State", Description = "State")]
        public string State { get; set; }

        [Display(Name = "City", Prompt = "City", Description = "City")]
        public string City { get; set; }

        [ScaffoldColumn(false)]
        public float Latitude { get; set; }

        [ScaffoldColumn(false)]
        public float Longitude { get; set; }


    }
    
}