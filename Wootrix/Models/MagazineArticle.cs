using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class MagazineArticle
    {
        [Key]
        public int ID { get; set; }

        public int CompanyMagazineID { get; set; }
        public string title { get; set; }
        public byte[] image { get; set; }
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

    }
}
