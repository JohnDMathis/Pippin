using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.DomainServices.Server;
using System.ServiceModel.DomainServices.Server.ApplicationServices;
using System.ServiceModel.DomainServices.Hosting;
using System.Web.Security;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace MI.Model
{
    public class AppUser : UserBase, IAppUser, IUser, IPrincipal, IIdentity
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual string Email { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual bool CanLogin { get; set; }
        public virtual bool IsAdmin { get; set; }

    }

}

