using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ASPdotNET_CUOIMON
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "CategoryProducts",
                url: "Product/ByCategory/{categoryId}",
                defaults: new { controller = "Product", action = "ByCategory", categoryId = UrlParameter.Optional }
            );

            routes.MapRoute(
                   name: "AddCategory",
                   url: "Category/AddCategory",
                   defaults: new { controller = "Category", action = "AddCategory" }
               );
            routes.MapRoute(
                    name: "AdminAccount",
                    url: "AdminAccount/{action}/{id}",
                    defaults: new { controller = "AdminAccount", action = "Index", id = UrlParameter.Optional }
                );
            routes.MapRoute(
                    name: "Order",
                    url: "Order/{action}/{id}",
                    defaults: new { controller = "Order", action = "Order", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                    name: "MyOrders",
                    url: "Order/MyOrders",
                    defaults: new { controller = "Order", action = "MyOrders" }
                );

            routes.MapRoute(
                    name: "CancelOrder",
                    url: "Order/CancelOrder/{orderId}",
                    defaults: new { controller = "Order", action = "CancelOrder", orderId = UrlParameter.Optional }
                );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
