using System;
using System.Collections.Generic;
using System.Linq;
using SPMeta2.Services;
using SPMeta2.Services.Impl;
using SPMeta2.Exceptions;

namespace SPMeta2
{
    public class ServiceContainer
    {
        #region constructors

        static ServiceContainer()
        {
            Instance = new ServiceContainer();
        }

        private ServiceContainer()
        {
            Services = new Dictionary<Type, List<object>>();
            ServiceTypes = new Dictionary<Type, Type>();

            InitServices();
        }

        private void InitServices()
        {
            // types
            RegisterServiceType(typeof(ModelTreeTraverseServiceBase), typeof(DefaultModelTreeTraverseService));

            // service
            RegisterService(typeof(TraceServiceBase), new TraceSourceService());

            RegisterService(typeof(ModelTreeTraverseServiceBase), new DefaultModelTreeTraverseService());
            RegisterService(typeof(ModelWeighServiceBase), new DefaultModelWeighService());

            RegisterService(typeof(DefaultJSONSerializationService), new DefaultJSONSerializationService());
            RegisterService(typeof(DefaultXMLSerializationService), new DefaultXMLSerializationService());

            RegisterService(typeof(WebPartChromeTypesConvertService), new DefaultWebPartChromeTypesConvertService());
            RegisterService(typeof(ListViewScopeTypesConvertService), new DefaultListViewScopeTypesConvertService());

            RegisterService(typeof(DefinitionRelationshipServiceBase), new DefaultDefinitionRelationshipService());

            RegisterService(typeof(ModelCompatibilityServiceBase), new DefaultModelCompatibilityService());

            RegisterService(typeof(DefaultDiagnosticInfoService), new DefaultDiagnosticInfoService());

            RegisterService(typeof(ModelPrettyPrintServiceBase), new DefaultModelPrettyPrintService());
            RegisterService(typeof(ModelDotGraphPrintServiceBase), new DefaultModelDotGraphPrintService());

            RegisterService(typeof(FluentModelValidationServiceBase), new DefaultFluentModelValidationService());

            RegisterService(typeof(ModelStatInfoServiceBase), new DefaultModelStatInfoService());

            RegisterService(typeof(TryRetryServiceBase), new DefaultTryRetryService());

            RegisterService(typeof(WebPartPageTemplatesServiceBase), new DefaultWebPartPageTemplatesService());
            RegisterService(typeof(HashCodeServiceBase), new MD5HashCodeServiceBase());
        }

        #endregion

        #region properties

        public Dictionary<Type, List<object>> Services { get; set; }

        public Dictionary<Type, Type> ServiceTypes { get; set; }

        #endregion

        #region methods


        public void RegisterServiceType(Type serviceType, Type implementationType)
        {
            if (!ServiceTypes.ContainsKey(serviceType))
                ServiceTypes.Add(serviceType, implementationType);

            ServiceTypes[serviceType] = implementationType;
        }

        public TServiceType CreateNewService<TServiceType>()
            where TServiceType : class
        {
            var type = typeof(TServiceType);
            return CreateNewService(type) as TServiceType;
        }

        public object CreateNewService(Type serviceType)
        {
            if (!ServiceTypes.ContainsKey(serviceType))
                throw new SPMeta2Exception(string.Format("Cannot find implementation type for service type:[{0}]", serviceType));

            return CreateNewInstance(ServiceTypes[serviceType]);
        }

        public TInstanceType CreateNewInstance<TInstanceType>()
            where TInstanceType : class
        {
            return CreateNewInstance(typeof(TInstanceType)) as TInstanceType;
        }

        public object CreateNewInstance(Type instanceType)
        {
            var instance = Activator.CreateInstance(instanceType);

            if (instance == null)
                throw new SPMeta2Exception(string.Format("Created instance is null. Service type:[{0}]", instanceType));

            return instance;
        }

        public void RegisterService(Type type, object service)
        {
            if (!Services.ContainsKey(type))
                Services.Add(type, new List<object>());

            var list = Services[type];

            if (!list.Contains(service))
                list.Add(service);
        }

        public void ReplaceService(Type type, object service)
        {
            if (!Services.ContainsKey(type))
                Services.Add(type, new List<object>());

            var list = Services[type];

            list.Clear();
            list.Add(service);
        }

        public void RegisterServices(Type type, List<object> services)
        {
            foreach (var s in services)
                RegisterService(type, s);
        }

        public TService GetService<TService>()
            where TService : class
        {
            List<object> services;

            Services.TryGetValue(typeof(TService), out services);

            if (services != null)
                return services.FirstOrDefault() as TService;

            return null;
        }

        public IEnumerable<TService> GetServices<TService>()
           where TService : class
        {
            List<object> services;

            Services.TryGetValue(typeof(TService), out services);

            if (services != null)
            {
                return services
                    .Where(s => s as TService != null)
                    .Select(s => s as TService);
            }

            return Enumerable.Empty<TService>();
        }

        #endregion

        #region properties

        public static ServiceContainer Instance { get; set; }

        #endregion


    }
}
