using ASPdotNET_CUOIMON.Models.ModelView;
using ASPdotNET_CUOIMON.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPdotNET_CUOIMON.Controllers
{
    public class AdminAccountController : Controller
    {
        private readonly AccountRepository accountRepo = new AccountRepository();

        // GET: /AdminAccount/Index
        public ActionResult Index(string search = "")
        {
            var accounts = accountRepo.GetAllAccounts();
            if (!string.IsNullOrEmpty(search))
            {
                accounts = accounts.Where(a =>
                    a.FullName.ToLower().Contains(search.ToLower()) ||
                    a.Email.ToLower().Contains(search.ToLower())
                ).ToList();
                ViewBag.Search = search;
            }

            return View(accounts);
        }

        // GET: /AdminAccount/Details/{id}
        public ActionResult Details(int id)
        {
            var account = accountRepo.GetAllAccounts().FirstOrDefault(a => a.UserId == id);
            if (account == null)
            {
                TempData["Error"] = "Không tìm thấy tài khoản.";
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: /AdminAccount/CreateAdmin
        public ActionResult CreateAdmin()
        {
            return View(new AccountRegisterView());
        }

        // POST: /AdminAccount/CreateAdmin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdmin(AccountRegisterView model)
        {
            if (ModelState.IsValid)
            {
                var result = accountRepo.CreateAdmin(model);
                if (result)
                {
                    TempData["Success"] = "Tạo tài khoản Admin thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản hoặc email đã tồn tại.");
                }
            }
            return View(model);
        }
    }
}