using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

using Microsoft.Practices.ServiceLocation;

namespace Torshify.Origo.Extensions
{
    public class ServiceLocatorServiceInstanceProvider : IInstanceProvider
    {
        #region Fields

        private readonly Type _serviceType;

        #endregion Fields

        #region Constructors

        public ServiceLocatorServiceInstanceProvider(Type serviceType)
        {
            _serviceType = serviceType;
        }

        #endregion Constructors

        #region Public Methods

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return GetInstance(instanceContext);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return ServiceLocator.Current.GetInstance(_serviceType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            var disposable = instance as IDisposable;

            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        #endregion Public Methods
    }
}