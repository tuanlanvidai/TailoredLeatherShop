using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNET_CUOIMON.Models.ModelView
{
    public class CategoryHomeView
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ProductCount { get; set; }
        public string ImageUrl { get; set; }
    }

}