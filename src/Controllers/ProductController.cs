using ASPdotNET_CUOIMON.Models.Entities;
using ASPdotNET_CUOIMON.Models.ModelView;
using ASPdotNET_CUOIMON.Models.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPdotNET_CUOIMON.Controllers
{
    public class ProductController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Categories = new CategoryRepository().GetAll();
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Index()
        {
            var productRepo = new ProductRepository();
            var products = productRepo.GetAll();

            var categoryRepo = new CategoryRepository();
            var categories = categoryRepo.GetAll();

            var grouped = categories.Select(cat => new GroupedProductsView
            {
                CategoryId = cat.CategoryId,
                CategoryName = cat.Name,
                Products = products
                            .Where(p => p.CategoryName == cat.Name)
                            .Take(6)
                            .ToList()
            }).ToList();

            return View(grouped);
        }

        public ActionResult Details(int id)
        {
            var productRepo = new ProductRepository();
            var product = productRepo.GetById(id);

            if (product == null)
                return HttpNotFound();

            var relatedProducts = productRepo.GetRelatedProducts(product.CategoryId, id);
            ViewBag.RelatedProducts = relatedProducts;

            return View(product);
        }

        // GET: /Product/Create
        public ActionResult Create()
        {
            ViewBag.Categories = new CategoryRepository().GetAll();
            return View(new ProductView());
        }

        // POST: /Product/Create
        [HttpPost]
        public ActionResult Create(ProductView model, HttpPostedFileBase thumbnail, HttpPostedFileBase image2, HttpPostedFileBase image3, HttpPostedFileBase image4)
        {
            if (ModelState.IsValid)
            {
                var productRepo = new ProductRepository();

                if (model.CategoryId == 0)
                {
                    ViewBag.Error = "Vui lòng chọn danh mục trước khi thêm sản phẩm.";
                    ViewBag.Categories = new CategoryRepository().GetAll();
                    return View(model);
                }

                if (productRepo.IsProductNameExists(model.Name))
                {
                    ViewBag.Error = "Tên sản phẩm đã tồn tại. Vui lòng chọn tên khác.";
                    ViewBag.Categories = new CategoryRepository().GetAll();
                    return View(model);
                }

                if (thumbnail != null && thumbnail.ContentLength > 0)
                {
                    string thumbnailPath = SaveImage(thumbnail);
                    model.ImageUrl = thumbnailPath;
                }

                var productId = productRepo.CreateWithReturnId(model);
                if (productId > 0)
                {
                    SaveGalleryImages(productId, image2, image3, image4);

                    TempData["Success"] = "Thêm sản phẩm thành công!";
                    return RedirectToAction("Index", "AdminDashboard");
                }
                else
                {
                    ViewBag.Error = "Lỗi khi thêm sản phẩm.";
                }
            }

            ViewBag.Categories = new CategoryRepository().GetAll();
            return View(model);
        }

        // GET: /Product/Edit/{id}
        public ActionResult Edit(int id)
        {
            var productRepo = new ProductRepository();
            var model = productRepo.GetById(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            var categories = new CategoryRepository().GetAll();
            ViewBag.Categories = categories;
            return View(model);
        }

        // POST: /Product/Edit
        [HttpPost]
        public ActionResult Edit(ProductView model, HttpPostedFileBase thumbnail, HttpPostedFileBase image2, HttpPostedFileBase image3, HttpPostedFileBase image4)
        {
            if (ModelState.IsValid)
            {
                var productRepo = new ProductRepository();

                if (thumbnail != null && thumbnail.ContentLength > 0)
                {
                    string thumbnailPath = SaveImage(thumbnail);
                    if (!string.IsNullOrEmpty(thumbnailPath))
                    {
                        model.ImageUrl = thumbnailPath;
                    }
                }

                if (productRepo.Update(model))
                {
                    DeleteGalleryImages(model.ProductId);
                    SaveGalleryImages(model.ProductId, image2, image3, image4);

                    TempData["Success"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction("Index", "AdminDashboard");
                }
                else
                {
                    ViewBag.Error = "Lỗi khi cập nhật sản phẩm.";
                }
            }

            ViewBag.Categories = new CategoryRepository().GetAll();
            return View(model);
        }

        // GET: /Product/Delete/{id}
        public ActionResult Delete(int id)
        {
            var productRepo = new ProductRepository();
            var isDeleted = productRepo.Delete(id);

            if (isDeleted)
            {
                TempData["Success"] = "Xóa sản phẩm thành công!";
            }
            else
            {
                TempData["Error"] = "Lỗi khi xóa sản phẩm.";
            }

            return RedirectToAction("Index", "AdminDashboard");
        }

        private string SaveImage(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                string folderPath = Server.MapPath("~/Content/Client/images/");
                string path = Path.Combine(folderPath, fileName + extension);

                int count = 1;
                while (System.IO.File.Exists(path))
                {
                    string newFileName = $"{fileName}_{count}{extension}";
                    path = Path.Combine(folderPath, newFileName);
                    count++;
                }
                file.SaveAs(path);

                return "images/" + Path.GetFileName(path);
            }

            return null;
        }
        private void SaveGalleryImages(int productId, params HttpPostedFileBase[] files)
        {
            var db = new handbagShopDBEntities();

            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string path = SaveImage(file);

                    tbl_product_images image = new tbl_product_images
                    {
                        product_id = productId,
                        image_url = path,
                        is_delete = false
                    };

                    db.tbl_product_images.Add(image);
                }
            }

            db.SaveChanges();
        }
        private void DeleteGalleryImages(int productId)
        {
            var db = new handbagShopDBEntities();
            var images = db.tbl_product_images.Where(i => i.product_id == productId).ToList();
            foreach (var img in images)
            {
                img.is_delete = true;
            }
            db.SaveChanges();
        }

        public ActionResult ByCategory(int categoryId, string search = "")
        {
            var productRepo = new ProductRepository();
            var products = productRepo.GetByCategory(categoryId, search);
            var categoryRepo = new CategoryRepository();
            var category = categoryRepo.GetAll().FirstOrDefault(c => c.CategoryId == categoryId);
            ViewBag.CategoryName = category != null ? category.Name : "Không xác định";
            ViewBag.CategoryId = categoryId;
            ViewBag.SearchTerm = search;

            if (products == null || products.Count == 0)
            {
                TempData["Error"] = "Không có sản phẩm nào trong danh mục này.";
                return RedirectToAction("Index", "Product");
            }

            return View("ByCategory", products);
        }

    }
}