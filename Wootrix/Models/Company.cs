using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class Company
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogoUrl { get; set; }
        public string CompanyMessage { get; set; }
        public string CompanyPrimaryHighlightColor { get; set; }
        public string CompanySecondaryHighlightColor { get; set; }
        public string CompanyBackgroundColor { get; set; }
    }
}
