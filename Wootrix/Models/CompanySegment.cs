using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class CompanySegment
    {
        [Key]
        public int ID { get; set; }

        public int CompanyID { get; set; }

        public string Title { get; set; }

        public string CoverImage { get; set; }

        public string CoverImageMobileFriendly { get; set; }

        public DateTime PublishDate { get; set; }

        public DateTime FinishDate { get; set; }

        //In case they want to override the company settings for a magazine
        public string ClientName { get; set; }

        public string ClientLogoImage { get; set; }

        public string ThemeColor { get; set; }

        public string StandardColor { get; set; }

        public bool Draft { get; set; }




    }
}
