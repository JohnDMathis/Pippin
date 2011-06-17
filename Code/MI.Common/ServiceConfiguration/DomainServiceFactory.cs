using System;
using System.ServiceModel.DomainServices.Server;
using Microsoft.Practices.Unity;

namespace MI.Common.ServiceConfiguration
{
    public class DomainServiceFactory : IDomainServiceFactory
    {
        public DomainService CreateDomainService(Type domainServiceType, DomainServiceContext context)
        {
            var service = Container.Instance.Resolve(domainServiceType) as DomainService;
            if (service != null) service.Initialize(context);
            return service;
        }

        public void ReleaseDomainService(DomainService domainService)
        {
            domainService.Dispose();
        }
    }
}

