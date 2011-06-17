using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MI.Model
{
    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
        bool UserIsAuthenticated { get; }
        string UserName { get; }
    }
}
