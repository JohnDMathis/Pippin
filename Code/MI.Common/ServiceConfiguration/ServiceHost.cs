using System;
using System.ServiceModel.Description;
using System.ServiceModel.DomainServices.Hosting;

namespace MI.Common.ServiceConfiguration
{
    public class ServiceHost:DomainServiceHost
    {
        public ServiceHost(Type domainServiceType, params Uri[] baseAddresses)
            : base(domainServiceType, baseAddresses)
        {
        }

        protected override void  AddDefaultBehaviors()
        {
            // turn off the WSDL
            base.AddDefaultBehaviors();
            Description.Behaviors.Find<ServiceMetadataBehavior>().HttpGetEnabled = false;
        }
     
        protected override void  OnOpening()
        {
            // add behavior for Unity
            Description.Behaviors.Add(new UnityServiceBehavior());
            base.OnOpening();
        }
    }

}

