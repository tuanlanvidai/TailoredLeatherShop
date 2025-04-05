using ASPdotNET_CUOIMON.Models.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPdotNET_CUOIMON.Models.Utils
{
	public class SessionUtils
	{
        public static AccountSessionView GetUser()
        {
            return HttpContext.Current.Session["user"] as AccountSessionView;
        }

        public static bool IsLoggedIn()
        {
            return GetUser() != null;
        }

        public static bool IsAdmin()
        {
            return GetUser()?.Role == "Admin";
        }

        public static bool IsCustomer()
        {
            return GetUser()?.Role == "Customer";
        }
    }
}