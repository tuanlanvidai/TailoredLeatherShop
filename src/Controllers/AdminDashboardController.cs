using ASPdotNET_CUOIMON.Models.ModelView;
using ASPdotNET_CUOIMON.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPdotNET_CUOIMON.Controllers
{
    public class AdminDashboardController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Categories = new CategoryRepository().GetAll();

            base.OnActionExecuting(filterContext);
        }

        private readonly ProductRepository productRepo = new ProductRepository();
        private readonly OrderRepository orderRepo = new OrderRepository();
        private readonly AccountRepository accountRepo = new AccountRepository();


        // GET: AdminDashboard/Index
        public ActionResult Index(string search = "")
        {
            var products = productRepo.GetAll();

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.ToLower().Contains(search.ToLower())).ToList();
                ViewBag.Search = search;
            }

            return View(products);
        }

        public ActionResult Orders(string search = "", string statusFilter = "")
        {
            var orders = orderRepo.GetAllOrders();

            if (!string.IsNullOrEmpty(search))
            {
                orders = orders.Where(o => o.OrderId.ToString().Contains(search)).ToList();
                ViewBag.Search = search;
            }

            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
            {
                orders = orders.Where(o => o.Status == statusFilter).ToList();
                ViewBag.SelectedStatus = statusFilter;
            }

            ViewBag.StatusList = new List<SelectListItem>
            {
                new SelectListItem { Text = "Tất cả", Value = "All" },
                new SelectListItem { Text = "Mới", Value = "New" },
                new SelectListItem { Text = "Đã báo giá", Value = "Quoted" },
                new SelectListItem { Text = "Đã xác nhận", Value = "Confirmed" },
                new SelectListItem { Text = "Đã hủy", Value = "Cancelled" }
            };

            return View(orders);
        }
        // GET: AdminDashboard/OrderDetails/{id}
        public ActionResult OrderDetails(int id)
        {
            var order = orderRepo.GetById(id);
            if (order == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction("Orders");
            }

            var user = accountRepo.GetById(order.UserId);
            if (user != null)
            {
                order.CustomerName = user.FullName;
                order.CustomerEmail = user.Email;
                order.CustomerPhone = user.Phone;
            }
            else
            {
                order.CustomerName = "Không xác định";
                order.CustomerEmail = "Không xác định";
                order.CustomerPhone = "Không xác định";
            }

            ViewBag.UserProfile = user;
            return View(order);
        }

        [HttpPost]
        public ActionResult UpdateQuote(AdminQuoteView model)
        {
            if (ModelState.IsValid)
            {
                var success = orderRepo.UpdateQuote(model);
                if (success)
                {
                    TempData["Success"] = "Cập nhật thông tin đơn hàng thành công!";
                }
                else
                {
                    TempData["Error"] = "Có lỗi xảy ra khi cập nhật đơn hàng.";
                }
            }
            return RedirectToAction("OrderDetails", new { id = model.OrderId });
        }

        [HttpPost]
        public ActionResult CancelOrderByAdmin(int orderId)
        {
            var success = orderRepo.CancelOrderByAdmin(orderId);
            if (success)
            {
                TempData["Success"] = "Đơn hàng đã được hủy thành công!";
            }
            else
            {
                TempData["Error"] = "Không thể hủy đơn hàng. Chỉ có thể hủy khi đơn hàng ở trạng thái Đã báo giá.";
            }
            return RedirectToAction("OrderDetails", new { id = orderId });
        }

    }
}