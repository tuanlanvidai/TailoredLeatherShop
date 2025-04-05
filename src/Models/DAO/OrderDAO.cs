using ASPdotNET_CUOIMON.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNET_CUOIMON.Models.DAO
{
    public class OrderDAO
    {
        private handbagShopDBEntities db = new handbagShopDBEntities();

        public List<tbl_orders> GetOrdersByUser(int userId)
        {
            return db.tbl_orders
                .Where(o => o.user_id == userId && o.is_delete == false)
                .OrderByDescending(o => o.created_at)
                .ToList();
        }

        public tbl_orders GetById(int id)
        {
            return db.tbl_orders.FirstOrDefault(o => o.order_id == id && o.is_delete == false);
        }

        public bool Create(tbl_orders order, tbl_order_details detail)
        {
            try
            {
                db.tbl_orders.Add(order);
                db.SaveChanges();

                detail.order_id = order.order_id;
                db.tbl_order_details.Add(detail);
                db.SaveChanges();

                return true;
            }
            catch { return false; }
        }

        public bool Update(tbl_orders order)
        {
            try
            {
                var existing = db.tbl_orders.Find(order.order_id);
                if (existing == null) return false;

                existing.status = order.status;
                existing.quote_price = order.quote_price;
                existing.deposit_amount = order.deposit_amount;
                existing.estimated_delivery_date = order.estimated_delivery_date;
                existing.updated_at = DateTime.Now;

                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public bool SoftDelete(int id)
        {
            try
            {
                var order = db.tbl_orders.Find(id);
                if (order == null) return false;

                order.is_delete = true;

                var details = db.tbl_order_details.Where(x => x.order_id == id);
                foreach (var item in details)
                {
                    item.is_delete = true;
                }

                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }


    }
}