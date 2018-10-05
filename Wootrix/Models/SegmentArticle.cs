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

        [ScaffoldColumn(false)]
        public int CompanySegmentID { get; set; }

        public int Order { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public int embedVideoLink { get; set; }

        public string description { get; set; }

        public string link { get; set; }

        public DateTime publishDate { get; set; }

        public string tags { get; set; }

        public int allowComments { get; set; }

        public DateTime publishFrom { get; set; }

        public DateTime publishTill { get; set; }

        public string author { get; set; }

    }
}
