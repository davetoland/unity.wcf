using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Unity.Wcf
{
    public class UnityInstanceProvider : IInstanceProvider
    {
        private readonly IUnityContainer _container;
        private readonly Type _contractType;
        private readonly string _registrationName;

        public UnityInstanceProvider(IUnityContainer container, Type contractType, string registrationName)
        {
            _container = container ?? throw new ArgumentNullException("container");
            _contractType = contractType ?? throw new ArgumentNullException("contractType");

            _container = container;
            _contractType = contractType;
            _registrationName = registrationName;
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            var result = new object();
            IUnityContainer childContainer = instanceContext.Extensions.Find<UnityInstanceContextExtension>().GetChildContainer(_container);
            if (!string.IsNullOrEmpty(_registrationName))
                result = childContainer.Resolve(_contractType, _registrationName, new Resolution.ResolverOverride[0]);
            else
                result = childContainer.Resolve(_contractType);

            return result;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            instanceContext.Extensions.Find<UnityInstanceContextExtension>().DisposeOfChildContainer();
        }
    }
}