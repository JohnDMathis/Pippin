using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace MI.Common.ServiceConfiguration
{
    public class UnityInstanceProvider:IInstanceProvider
    {
        private readonly Type _serviceType;

        public UnityInstanceProvider(Type serviceType)
        {
            _serviceType = serviceType;
        }

        #region Implementation of IInstanceProvider

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return Container.Instance.Resolve(_serviceType,"",null);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }

        #endregion
    }
}