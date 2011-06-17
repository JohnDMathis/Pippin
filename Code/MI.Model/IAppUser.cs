using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.DomainServices.Server.ApplicationServices;

namespace MI.Model
{
    public interface IAppUser:IUser
    {
        int Id { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        bool CanLogin { get; set; }
        bool IsAdmin { get; set; }
    }
}
