using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Unity.Wcf
{
    public abstract class UnityServiceHostFactory : ServiceHostFactory
    {
        private string _registrationName;

        protected abstract void ConfigureContainer(IUnityContainer container);

        public UnityServiceHostFactory(string registrationName)
        {
            _registrationName = registrationName;
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return CreateServiceHost("", serviceType, baseAddresses);
        }

        public ServiceHost CreateServiceHost(string registrationName, Type serviceType, Uri[] baseAddresses)
        {
            var container = new UnityContainer();
            ConfigureContainer(container);

            return new UnityServiceHost(container, registrationName, serviceType, baseAddresses);
        }
    }
}