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
    }
}
