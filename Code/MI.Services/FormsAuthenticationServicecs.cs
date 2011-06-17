using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using MI.Model;
using System.Web;

namespace MI.Services
{

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public bool UserIsAuthenticated
        {
            get { return HttpContext.Current.User.Identity.IsAuthenticated; }
        }


        public string UserName
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
        }

        
    }
}
