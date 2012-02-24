using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

using Microsoft.Practices.ServiceLocation;

namespace Torshify.Origo.Extensions
{
    public class ServiceLocatorServiceBehaviorAttribute : Attribute, IServiceBehavior
    {
        #region Public Methods

        public void AddBindingParameters(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
            //nothing to do
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase cdb in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher cd = cdb as ChannelDispatcher;
                if (cd != null)
                {
                    foreach (EndpointDispatcher endpointDispatcher in cd.Endpoints)
                    {
                        endpointDispatcher.DispatchRuntime.InstanceProvider =
                                         new ServiceLocatorServiceInstanceProvider(serviceDescription.ServiceType);

                        foreach (IDispatchMessageInspector inspector in
                                     ServiceLocator.Current.GetAllInstances<IDispatchMessageInspector>())
                        {
                            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
                        }
                    }
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            //nothing to do
        }

        #endregion Public Methods
    }
}