using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNET_CUOIMON.Models.ModelView
{
    public class ProductView
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public List<string> ImageGallery { get; set; }
        public bool IsAvailable { get; set; }
    }

}