using ASPdotNET_CUOIMON.Models.Entities;
using ASPdotNET_CUOIMON.Models.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ASPdotNET_CUOIMON.Models.DAO
{
    public class AccountDAO
	{
        private readonly handbagShopDBEntities db = new handbagShopDBEntities();

        public AccountSessionView CheckLogin(string username, string password)
        {
            var user = db.tbl_users
                         .FirstOrDefault(u => u.username == username
                                              && u.password_hash == password
                                              && u.is_delete == false);

            if (user != null)
            {
                return new AccountSessionView
                {
                    UserId = user.user_id,
                    Username = user.username,
                    FullName = user.full_name,
                    Phone = user.phone,
                    Email = user.email,
                    Role = user.role
                };
            }
            return null;
        }
        public tbl_users GetById(int userId)
        {
            return db.tbl_users.FirstOrDefault(u => u.user_id == userId && u.is_delete == false);
        }

    }
}