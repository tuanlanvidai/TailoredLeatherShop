﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNET_CUOIMON.Models.ModelView
{
	public class GroupedProductsView
	{
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductView> Products { get; set; }
    }
}