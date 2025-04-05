using ASPdotNET_CUOIMON.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPdotNET_CUOIMON.Controllers
{
    public class CategoryController : Controller
    {
        private readonly handbagShopDBEntities db = new handbagShopDBEntities();

        // POST: /Category/AddCategory
        [HttpPost]
        public JsonResult AddCategory(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(new { success = false, message = "Tên danh mục không được để trống." });
            }

            var existCategory = db.tbl_categories.FirstOrDefault(c => c.name == name && c.is_delete == false);
            if (existCategory != null)
            {
                return Json(new { success = false, message = "Danh mục đã tồn tại." });
            }

            var newCategory = new tbl_categories
            {
                name = name,
                is_delete = false
            };

            db.tbl_categories.Add(newCategory);
            db.SaveChanges();

            return Json(new { success = true, id = newCategory.category_id, name = newCategory.name });
        }
    }
}