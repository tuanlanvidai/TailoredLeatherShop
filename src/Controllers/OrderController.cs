using ASPdotNET_CUOIMON.Models.ModelView;
using ASPdotNET_CUOIMON.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPdotNET_CUOIMON.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderRepository orderRepo = new OrderRepository();
        private readonly ProductRepository productRepo = new ProductRepository();

        // GET: /Order/Order
        public ActionResult Order(int productId, int quantity = 1)
        {
            var user = Session["user"] as AccountSessionView;
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Request.Url.PathAndQuery });
            }

            var product = productRepo.GetById(productId);
            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ProductName = product.Name;
            ViewBag.ProductImage = product.ImageUrl;

            var model = new OrderCreateView
            {
                ProductId = product.ProductId,
                Quantity = quantity,
                ShippingAddress = "",
                AdditionalRequest = "",
                CustomNote = "",

                FullName = user.FullName,
                Phone = user.Phone,
                Email = user.Email
            };

            return View(model);
        }



        // POST: /Order/ConfirmOrder
        [HttpPost]
        public ActionResult ConfirmOrder(OrderCreateView model)
        {
            var user = Session["user"] as AccountSessionView;
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orderModel = new OrderView
            {
                UserId = user.UserId,
                OrderItems = new List<OrderItemView>
                {
                    new OrderItemView
                    {
                        ProductId = model.ProductId,
                        Quantity = model.Quantity,
                        CustomNote = model.CustomNote
                    }
                },
                ShippingAddress = model.ShippingAddress,
                AdditionalRequest = model.AdditionalRequest
            };

            var success = orderRepo.CreateOrder(orderModel);
            if (success)
            {
                TempData["Success"] = "Đặt hàng thành công! Đang chờ Admin báo giá.";
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Có lỗi xảy ra khi đặt hàng!";
            return View("Order", model);
        }

        // GET: /Order/MyOrders
        public ActionResult MyOrders()
        {
            var user = Session["user"] as AccountSessionView;
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = orderRepo.GetOrdersByUser(user.UserId);
            ViewBag.UserProfile = user;
            return View(orders);
        }

        // GET: /Order/Details/5
        public ActionResult Details(int id)
        {
            var user = Session["user"] as AccountSessionView;
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = orderRepo.GetById(id);
            if (order == null || order.UserId != user.UserId)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng hoặc bạn không có quyền xem đơn này.";
                return RedirectToAction("MyOrders", "Order");
            }

            ViewBag.UserProfile = user;
            return View(order);
        }


        [HttpPost]
        public ActionResult CancelOrder(int orderId)
        {
            var success = orderRepo.CancelOrder(orderId);
            if (success)
            {
                TempData["Success"] = "Đơn hàng đã được hủy thành công!";
            }
            else
            {
                TempData["Error"] = "Không thể hủy đơn hàng. Đơn hàng đang trong quá trình xử lý hoặc đã hoàn thành.";
            }
            return RedirectToAction("MyOrders");
        }

        [HttpPost]
        public ActionResult ApproveOrder(int orderId)
        {
            var success = orderRepo.ApproveOrder(orderId);
            if (success)
            {
                TempData["Success"] = "Đơn hàng đã được xác nhận và đang xử lý!";
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi xác nhận đơn hàng.";
            }
            return RedirectToAction("Details", new { id = orderId });
        }

        // POST: /Order/MarkAsCompleted
        [HttpPost]
        public ActionResult MarkAsCompleted(int orderId)
        {
            var success = orderRepo.MarkAsCompleted(orderId);
            if (success)
            {
                TempData["Success"] = "Đơn hàng đã hoàn thành. Cảm ơn bạn!";
            }
            else
            {
                TempData["Error"] = "Có lỗi xảy ra khi cập nhật đơn hàng.";
            }
            return RedirectToAction("Details", new { id = orderId });
        }


    }
}