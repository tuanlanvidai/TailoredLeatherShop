using ASPdotNET_CUOIMON.Models.Entities;
using ASPdotNET_CUOIMON.Models.ModelView;
using ASPdotNET_CUOIMON.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPdotNET_CUOIMON.Controllers
{
    public class AccountController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Categories = new CategoryRepository().GetAll();

            base.OnActionExecuting(filterContext);
        }

        private readonly AccountRepository repo = new AccountRepository();
        private readonly handbagShopDBEntities db = new handbagShopDBEntities();

        // GET: /Account/Login
        public ActionResult Login(string returnUrl)
        {
            var session = Session["user"] as AccountSessionView;
            if (session != null)
            {
                if (session.Role == "Admin")
                    return RedirectToAction("Index", "AdminDashboard");
                else
                    return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = string.IsNullOrEmpty(returnUrl) ? Url.Action("Index", "Home") : returnUrl;
            return View(new AccountLoginView());
        }

        // POST: /Account/Login
        [HttpPost]
        public ActionResult Login(AccountLoginView model)
        {
            if (ModelState.IsValid)
            {
                var session = repo.Login(model.Username, model.Password);
                if (session != null)
                {
                    Session["user"] = new AccountSessionView
                    {
                        UserId = session.UserId,
                        Username = session.Username,
                        FullName = session.FullName,
                        Phone = session.Phone,
                        Email = session.Email, 
                        Role = session.Role
                    };
                    if (session.Role == "Admin")
                        return RedirectToAction("Index", "AdminDashboard");
                    else
                        return RedirectToAction("Index", "Home");
                }
                ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng.";
            }

            return View(model);
        }
        // GET: /Account/Register
        public ActionResult Register()
        {
            var session = Session["user"] as AccountSessionView;
            if (session != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new AccountRegisterView());
        }

        // POST: /Account/Register
        [HttpPost]
        public ActionResult Register(AccountRegisterView model)
        {
            if (ModelState.IsValid)
            {
                var existUsername = db.tbl_users
                                      .Any(u => u.username == model.Username && u.is_delete == false);
                if (existUsername)
                {
                    ModelState.AddModelError("Username", "Tài khoản đã tồn tại.");
                    return View(model);
                }
                var existEmail = db.tbl_users
                                   .Any(u => u.email == model.Email && u.is_delete == false);
                if (existEmail)
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                    return View(model);
                }

                var user = new tbl_users
                {
                    username = model.Username,
                    password_hash = model.Password,
                    email = model.Email,
                    full_name = model.FullName,
                    phone = model.Phone,
                    role = "Customer",
                    created_at = System.DateTime.Now,
                    is_delete = false
                };

                db.tbl_users.Add(user);
                db.SaveChanges();

                TempData["Success"] = "Tạo tài khoản thành công. Mời bạn đăng nhập!";
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        // GET: /Account/Profile
        public ActionResult Profile()
        {
            var session = Session["user"] as AccountSessionView;
            if (session == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = db.tbl_users.FirstOrDefault(u => u.user_id == session.UserId);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new AccountProfileView
            {
                UserId = user.user_id,
                Username = user.username,
                FullName = user.full_name,
                Email = user.email,
                Phone = user.phone,
                Role = user.role
            };

            return View(model);
        }

        // GET: /Account/EditProfile
        public ActionResult EditProfile()
        {
            var session = Session["user"] as AccountSessionView;
            if (session == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = db.tbl_users.FirstOrDefault(u => u.user_id == session.UserId);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new AccountProfileView
            {
                UserId = user.user_id,
                Username = user.username,
                FullName = user.full_name,
                Email = user.email,
                Phone = user.phone,
                Role = user.role
            };

            return View(model);
        }

        // POST: /Account/UpdateProfile
        [HttpPost]
        public ActionResult UpdateProfile(AccountProfileView model)
        {
            if (ModelState.IsValid)
            {
                var session = Session["user"] as AccountSessionView;
                if (session == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var user = db.tbl_users.FirstOrDefault(u => u.user_id == session.UserId);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                user.full_name = model.FullName;
                user.email = model.Email;
                user.phone = model.Phone;

                db.SaveChanges();
                TempData["Success"] = "Cập nhật thông tin thành công!";
                return RedirectToAction("Profile");
            }

            return View("EditProfile", model);
        }

        // GET: /Account/AdminProfile
        public ActionResult AdminProfile()
        {
            var session = Session["user"] as AccountSessionView;
            if (session == null || session.Role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var user = db.tbl_users.FirstOrDefault(u => u.user_id == session.UserId);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new AccountProfileView
            {
                UserId = user.user_id,
                Username = user.username,
                FullName = user.full_name,
                Email = user.email,
                Phone = user.phone,
                Role = user.role
            };

            return View(model);
        }

        // GET: /Account/EditAdminProfile
        public ActionResult EditAdminProfile()
        {
            var session = Session["user"] as AccountSessionView;
            if (session == null || session.Role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var user = db.tbl_users.FirstOrDefault(u => u.user_id == session.UserId);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new AccountProfileView
            {
                UserId = user.user_id,
                Username = user.username,
                FullName = user.full_name,
                Email = user.email,
                Phone = user.phone,
                Role = user.role
            };

            return View(model);
        }

        // POST: /Account/UpdateAdminProfile
        [HttpPost]
        public ActionResult UpdateAdminProfile(AccountProfileView model)
        {
            if (ModelState.IsValid)
            {
                var session = Session["user"] as AccountSessionView;
                if (session == null || session.Role != "Admin")
                {
                    return RedirectToAction("Login", "Account");
                }

                var user = db.tbl_users.FirstOrDefault(u => u.user_id == session.UserId);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Cập nhật thông tin Admin
                user.full_name = model.FullName;
                user.email = model.Email;
                user.phone = model.Phone;

                db.SaveChanges();
                TempData["Success"] = "Cập nhật thông tin Admin thành công!";
                return RedirectToAction("AdminProfile");
            }

            return View("EditAdminProfile", model);
        }


        // GET: /Account/Logout
        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Login", "Account");
        }
        // GET: AdminAccount/CreateAdmin
        public ActionResult CreateAdmin()
        {
            return View(new AccountRegisterView());
        }

        // POST: AdminAccount/CreateAdmin
        [HttpPost]
        public ActionResult CreateAdmin(AccountRegisterView model)
        {
            if (ModelState.IsValid)
            {
                var existUsername = db.tbl_users
                                      .Any(u => u.username == model.Username && u.is_delete == false);
                if (existUsername)
                {
                    ModelState.AddModelError("Username", "Tài khoản đã tồn tại.");
                    return View(model);
                }

                var existEmail = db.tbl_users
                                   .Any(u => u.email == model.Email && u.is_delete == false);
                if (existEmail)
                {
                    ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                    return View(model);
                }

                var user = new tbl_users
                {
                    username = model.Username,
                    password_hash = model.Password,
                    email = model.Email,
                    full_name = model.FullName,
                    phone = model.Phone,
                    role = "Admin",
                    created_at = DateTime.Now,
                    is_delete = false
                };

                db.tbl_users.Add(user);
                db.SaveChanges();

                TempData["Success"] = "Tạo tài khoản Admin thành công!";
                return RedirectToAction("Index");
            }

            return View(model);
        }


    }
}