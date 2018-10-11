using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class FruitModel
    {
        public IList<string> SelectedFruits { get; set; }
        public IList<SelectListItem> AvailableFruits { get; set; }

        public FruitModel()
        {
            SelectedFruits = new List<string>();
            AvailableFruits = new List<SelectListItem>();
        }


        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int ID { get; set; }

        ////This is what people see
        //public IList<string> SelectedFruits { get; set; }
        //public IList<string> AvailableFruits { get; set; }

        ////This is what gets saved to the DB
        //public string SelectedFruitsListString
        //{
        //    get { return string.Join(",", SelectedFruits); }
        //    set { SelectedFruits = value.Split(',').ToList(); }
        //}

        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int ID { get; set; }
        //public IList<String> SelectedFruits { get; set; }
        //public IList<SelectListItem> AvailableFruits { get; set; }
        //public ICollection<string> List { get; set; }
        //public string ListStringAvailableFruits
        //{
        //    get { return string.Join(",", AvailableFruits); }
        //    set { AvailableFruits = value.ToString().Split(',').ToList(); }
        //}
        //public FruitModel()
        //{
        //    SelectedFruits = new List<string>();
        //    AvailableFruits = new List<SelectListItem>();
        //}

        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }

        //private List<String> _strings { get; set; }

        //public List<string> Strings
        //{
        //    get { return _strings; }
        //    set { _strings = value; }
        //}

        //[Required]
        //public string StringsAsString
        //{
        //    get { return String.Join(',', _strings); }
        //    set { _strings = value.Split(',').ToList(); }
        //}
    }
}
