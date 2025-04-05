using ASPdotNET_CUOIMON.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNET_CUOIMON.Models.DAO
{
    public class CategoryDAO
    {
        private handbagShopDBEntities db = new handbagShopDBEntities();

        public List<tbl_categories> GetAll()
        {
            return db.tbl_categories.Where(c => c.is_delete == false).ToList();
        }
        public tbl_categories GetById(int id)
        {
            return db.tbl_categories.FirstOrDefault(c => c.category_id == id && c.is_delete == false);
        }

    }
}