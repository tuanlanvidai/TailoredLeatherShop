using ASPdotNET_CUOIMON.Models.ModelView;
using ASPdotNET_CUOIMON.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPdotNET_CUOIMON.Controllers
{
    public class HomeController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Categories = new CategoryRepository().GetAll();

            base.OnActionExecuting(filterContext);
        }

        public ActionResult Index()
        {
            var categoryRepo = new CategoryRepository();
            var categories = categoryRepo.GetAll();
            var productRepo = new ProductRepository();

            var grouped = categories
                .Select(c => new CategoryHomeView
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.Name,
                    ProductCount = productRepo.GetByCategory(c.CategoryId).Count, 
                    ImageUrl = productRepo.GetByCategory(c.CategoryId).FirstOrDefault()?.ImageUrl ?? "default.jpg"
                })
                .ToList();

            ViewBag.CategorySummaries = grouped;

            return View();
        }


    }
}