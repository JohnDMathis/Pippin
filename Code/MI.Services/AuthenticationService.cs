using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.DomainServices.Server;
using System.ServiceModel.DomainServices.Server.ApplicationServices;
using System.ServiceModel.DomainServices.Hosting;
using System.Web.Security;
using MI.Model;

namespace MI.Services
{
    //[EnableClientAccess(RequiresSecureEndpoint = ServiceConfiguration.Secure)]
    public class AuthenticationService<T> : DomainService, IAuthentication<T> where T:IAppUser
    {
        private const string ClientKey = "{54B72F19-1A6B-4379-BAD4-72544B3761D9}";

        private IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #region Implementation of IAuthentication<IAppUser>


        public T Login(string userName, string password, bool isPersistent, string clientKey)
        {
            // unauthorized applications are restricted from logging in by use of a ClientKey GUID.
            // any calling application must provide this key.
            T user =default(T);
            if (clientKey == ClientKey)
            {
                user = (T)_userRepository.GetAuthenticatedUserOrNull(userName, password);
                if (user != null)
                    FormsAuthentication.SetAuthCookie(userName, isPersistent);
            }
            return user;
        }

        [Invoke]
        public T InvokeLogin(string userName, string password, bool isPersistent, string clientKey)
        {
            // unauthorized applications are restricted from logging in by use of a ClientKey GUID.
            // any calling application must provide this key.
            T user = default(T);
            if (clientKey == ClientKey)
            {
                user = (T)_userRepository.GetAuthenticatedUserOrNull(userName, password);
                if (user != null)
                    FormsAuthentication.SetAuthCookie(userName, isPersistent);
            }
            return user;
        }


        [Invoke]
        public bool CheckLogin()
        {
            return ServiceContext.User.Identity.IsAuthenticated;
        }

        [Invoke]
        public T GetAuthenticatedUser(string clientKey)
        {
            T user = default(T);
            if (clientKey == ClientKey)
            {
                if (ServiceContext.User.Identity.IsAuthenticated)
                {
                    user = (T)_userRepository.GetUserByName(ServiceContext.User.Identity.Name);
                }
            }
            return user;
        }

        public T Logout()
        {
            FormsAuthentication.SignOut();
            return default(T);
        }

        [Invoke]
        public void InvokeLogout()
        {
            FormsAuthentication.SignOut();
        }

        public T GetUser()
        {
            return default(T);
        }

        public void UpdateUser(T user)
        {
            _userRepository.UpdateUser(user);
        }

        #endregion
    }
}
