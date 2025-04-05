using ASPdotNET_CUOIMON.Models.Entities;
using ASPdotNET_CUOIMON.Models.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNET_CUOIMON.Models.Repositories
{
    public class ProductRepository
    {
        private handbagShopDBEntities db = new handbagShopDBEntities();

        public List<ProductView> GetAll()
        {
            var data = from p in db.tbl_products
                       join c in db.tbl_categories on p.category_id equals c.category_id
                       where p.is_delete == false
                       select new ProductView
                       {
                           ProductId = p.product_id,
                           Name = p.name,
                           Description = p.description,
                           ImageUrl = p.image_url,
                           CategoryName = c.name,
                           CategoryId = c.category_id,
                           IsAvailable = p.is_available ?? false
                       };

            return data.ToList();
        }

        public ProductView GetById(int id)
        {
            var p = db.tbl_products.FirstOrDefault(x => x.product_id == id && x.is_delete == false);
            if (p == null) return null;

            var c = db.tbl_categories.FirstOrDefault(x => x.category_id == p.category_id);
            var images = db.tbl_product_images
                           .Where(i => i.product_id == id && i.is_delete == false)
                           .Select(i => i.image_url)
                           .ToList();

            return new ProductView
            {
                ProductId = p.product_id,
                Name = p.name,
                Description = p.description,
                ImageUrl = p.image_url,
                CategoryName = c != null ? c.name : "",
                CategoryId = c != null ? c.category_id : 0,
                ImageGallery = images,
                IsAvailable = p.is_available ?? false
            };
        }

        public List<ProductView> GetByCategory(int categoryId, string search = "")
        {
            var data = from p in db.tbl_products
                       join c in db.tbl_categories on p.category_id equals c.category_id
                       where p.is_delete == false && p.category_id == categoryId
                       select new ProductView
                       {
                           ProductId = p.product_id,
                           Name = p.name,
                           Description = p.description,
                           ImageUrl = p.image_url,
                           CategoryName = c.name,
                           CategoryId = c.category_id,
                           IsAvailable = p.is_available ?? false
                       };
            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(p => p.Name.Contains(search));
            }

            return data.ToList();
        }


        public int CreateWithReturnId(ProductView model)
        {
            try
            {
                if (IsProductNameExists(model.Name))
                {
                    return -1;
                }

                tbl_products p = new tbl_products
                {
                    name = model.Name,
                    description = model.Description,
                    category_id = model.CategoryId,
                    image_url = model.ImageUrl ?? "",
                    is_available = model.IsAvailable,
                    is_delete = false,
                    created_at = DateTime.Now
                };
                db.tbl_products.Add(p);
                db.SaveChanges();
                return p.product_id;
            }
            catch
            {
                return -1;
            }
        }


        public bool Update(ProductView model)
        {
            try
            {
                var p = db.tbl_products.Find(model.ProductId);
                if (p == null) return false;

                p.name = model.Name;
                p.description = model.Description;
                p.category_id = model.CategoryId;
                p.is_available = model.IsAvailable;
                p.image_url = model.ImageUrl;

                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public bool Delete(int id)
        {
            try
            {
                var p = db.tbl_products.Find(id);
                if (p == null) return false;
                p.is_delete = true;
                var images = db.tbl_product_images.Where(i => i.product_id == id).ToList();
                foreach (var img in images)
                {
                    img.is_delete = true;
                }

                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public List<ProductView> GetHotProducts(int take = 3)
        {
            return db.tbl_products
                .Where(p => p.is_delete == false && p.is_available == true)
                .OrderByDescending(p => p.created_at)
                .Take(take)
                .Join(db.tbl_categories, p => p.category_id, c => c.category_id, (p, c) => new ProductView
                {
                    ProductId = p.product_id,
                    Name = p.name,
                    Description = p.description,
                    ImageUrl = p.image_url,
                    CategoryName = c.name,
                    CategoryId = c.category_id,
                    IsAvailable = p.is_available ?? false
                }).ToList();
        }

        public List<ProductView> GetRelatedProducts(int categoryId, int productId)
        {
            return db.tbl_products
                .Where(p => p.category_id == categoryId && p.product_id != productId && p.is_delete == false)
                .Select(p => new ProductView
                {
                    ProductId = p.product_id,
                    Name = p.name,
                    ImageUrl = p.image_url
                })
                .ToList();
        }

        public bool IsProductNameExists(string productName)
        {
            return db.tbl_products.Any(p => p.name == productName && p.is_delete == false);
        }

    }
}