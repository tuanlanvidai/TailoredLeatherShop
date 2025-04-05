using ASPdotNET_CUOIMON.Models.DAO;
using ASPdotNET_CUOIMON.Models.Entities;
using ASPdotNET_CUOIMON.Models.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNET_CUOIMON.Models.Repositories
{
	public class AccountRepository
	{
        private readonly AccountDAO dao = new AccountDAO();
        private readonly handbagShopDBEntities db = new handbagShopDBEntities();
        public AccountSessionView Login(string username, string password)
        {
            return dao.CheckLogin(username, password);
        }

        public AccountSessionView GetById(int userId)
        {
            var user = dao.GetById(userId);
            if (user != null)
            {
                return new AccountSessionView
                {
                    UserId = user.user_id,
                    FullName = user.full_name,
                    Email = user.email,
                    Phone = user.phone,
                    Role = user.role
                };
            }
            return null;
        }
        public List<AccountView> GetAllAccounts()
        {
            using (var db = new handbagShopDBEntities())
            {
                var users = db.tbl_users
                              .Where(u => u.is_delete == false)
                              .Select(u => new AccountView
                              {
                                  UserId = u.user_id,
                                  FullName = u.full_name,
                                  Email = u.email,
                                  Phone = u.phone,
                                  Role = u.role,
                                  CreatedAt = u.created_at.HasValue ? u.created_at.Value : DateTime.Now
                              })
                              .ToList();
                return users;
            }
        }
        public bool CreateAdmin(AccountRegisterView model)
        {
            if (db.tbl_users.Any(u => u.username == model.Username || u.email == model.Email))
            {
                return false;
            }

            var user = new tbl_users
            {
                username = model.Username,
                password_hash = model.Password,
                full_name = model.FullName,
                email = model.Email,
                phone = model.Phone,
                role = "Admin",
                created_at = DateTime.Now,
                is_delete = false
            };

            db.tbl_users.Add(user);
            db.SaveChanges();
            return true;
        }
    }
}