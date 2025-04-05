using ASPdotNET_CUOIMON.Models.DAO;
using ASPdotNET_CUOIMON.Models.Entities;
using ASPdotNET_CUOIMON.Models.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNET_CUOIMON.Models.Repositories
{
    public class OrderRepository
    {
        private handbagShopDBEntities db = new handbagShopDBEntities();
        private OrderDAO dao = new OrderDAO();

        public bool CreateOrder(OrderView model)
        {
            try
            {
                var order = new tbl_orders
                {
                    user_id = model.UserId,
                    status = "New",
                    quote_price = 0,
                    quote_note = "",
                    deposit_amount = 0,
                    estimated_delivery_date = null,
                    shipping_address = model.ShippingAddress,
                    additional_request = model.AdditionalRequest,
                    admin_note = "",
                    created_at = DateTime.Now,
                    is_delete = false
                };

                db.tbl_orders.Add(order);
                db.SaveChanges();

                foreach (var item in model.OrderItems)
                {
                    var orderDetail = new tbl_order_details
                    {
                        order_id = order.order_id,
                        product_id = item.ProductId,
                        quantity = item.Quantity,
                        custom_note = item.CustomNote,
                        is_delete = false
                    };
                    db.tbl_order_details.Add(orderDetail);
                }

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public OrderView GetById(int orderId)
        {
            var orderData = db.tbl_orders
                .Where(o => o.order_id == orderId && o.is_delete == false)
                .FirstOrDefault();

            if (orderData == null)
            {
                return null;
            }
            var userData = db.tbl_users.FirstOrDefault(u => u.user_id == orderData.user_id);
            var orderItems = db.tbl_order_details
                .Where(d => d.order_id == orderData.order_id && d.is_delete == false)
                .Select(d => new OrderItemView
                {
                    ProductId = d.product_id ?? 0,
                    ProductName = d.tbl_products.name,
                    ImageUrl = d.tbl_products.image_url,
                    Quantity = d.quantity ?? 1,
                    CustomNote = d.custom_note
                })
                .ToList();

            var order = new OrderView
            {
                OrderId = orderData.order_id,
                UserId = orderData.user_id.HasValue ? orderData.user_id.Value : 0,
                CustomerName = userData?.full_name ?? "Khách hàng",
                CustomerEmail = userData?.email ?? "N/A",
                CustomerPhone = userData?.phone ?? "N/A",
                Status = orderData.status,
                QuotePrice = orderData.quote_price.HasValue ? (float)orderData.quote_price.Value : 0,
                DepositAmount = orderData.deposit_amount.HasValue ? (float)orderData.deposit_amount.Value : 0,
                EstimatedDeliveryDate = orderData.estimated_delivery_date.HasValue ? orderData.estimated_delivery_date.Value : (DateTime?)null,
                ShippingAddress = orderData.shipping_address,
                AdditionalRequest = orderData.additional_request,
                AdminNote = orderData.admin_note,
                CreatedAt = orderData.created_at.HasValue ? orderData.created_at.Value : DateTime.Now,
                OrderItems = orderItems
            };

            return order;
        }
        public bool UpdateQuote(AdminQuoteView model)
        {
            var order = db.tbl_orders.FirstOrDefault(o => o.order_id == model.OrderId && o.is_delete == false);
            if (order != null)
            {
                order.quote_price = model.QuotePrice;
                order.admin_note = model.AdminNote;
                order.estimated_delivery_date = model.EstimatedDeliveryDate;
                order.status = "Quoted";
                order.updated_at = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public List<OrderView> GetOrdersByUser(int userId)
        {
            var orders = dao.GetOrdersByUser(userId);
            return orders.Select(o => new OrderView
            {
                OrderId = o.order_id,
                Status = o.status,
                QuotePrice = (float)(o.quote_price ?? 0),
                DepositAmount = (float)(o.deposit_amount ?? 0),
                CreatedAt = o.created_at ?? DateTime.Now,
                EstimatedDeliveryDate = o.estimated_delivery_date,
                ShippingAddress = o.shipping_address,
                AdditionalRequest = o.additional_request,
                AdminNote = o.admin_note
            }).ToList();
        }
        public bool CancelOrder(int orderId)
        {
            var order = db.tbl_orders.FirstOrDefault(o => o.order_id == orderId && o.is_delete == false);
            if (order != null)
            {
                if (order.status == "InProgress" || order.status == "Completed")
                {
                    return false;
                }
                order.status = "Cancelled";
                order.updated_at = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public List<OrderView> GetAllOrders()
        {
            var orders = db.tbl_orders
                .Where(o => o.is_delete == false)
                .OrderByDescending(o => o.created_at)
                .Select(o => new OrderView
                {
                    OrderId = o.order_id,
                    UserId = o.user_id.HasValue ? o.user_id.Value : 0,
                    CustomerName = o.tbl_users.full_name,
                    Status = o.status,
                    QuotePrice = o.quote_price.HasValue ? (float)o.quote_price.Value : 0,
                    CreatedAt = o.created_at ?? DateTime.Now
                }).ToList();

            return orders;
        }
        public bool ApproveOrder(int orderId)
        {
            var order = db.tbl_orders.FirstOrDefault(o => o.order_id == orderId && o.is_delete == false);
            if (order != null && order.status == "Quoted")
            {
                order.status = "InProgress";
                order.updated_at = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool MarkAsCompleted(int orderId)
        {
            var order = db.tbl_orders.FirstOrDefault(o => o.order_id == orderId && o.is_delete == false);
            if (order != null && order.status == "InProgress")
            {
                order.status = "Completed";
                order.updated_at = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            return false;
        }
        public bool CancelOrderByAdmin(int orderId)
        {
            var order = db.tbl_orders.FirstOrDefault(o => o.order_id == orderId && o.is_delete == false);
            if (order != null && order.status == "Quoted")
            {
                order.status = "Cancelled";
                order.updated_at = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
