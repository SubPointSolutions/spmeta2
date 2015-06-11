using SPMeta2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

using SPMeta2.Services.Impl;

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

            InitServices();
        }

        private void InitServices()
        {
            RegisterService(typeof(TraceServiceBase), new TraceSourceService());

            RegisterService(typeof(ModelTreeTraverseServiceBase), new DefaultModelTreeTraverseService());
            RegisterService(typeof(ModelWeighServiceBase), new DefaultModelWeighService());

            RegisterService(typeof(DefaultJSONSerializationService), new DefaultJSONSerializationService());
            RegisterService(typeof(DefaultXMLSerializationService), new DefaultXMLSerializationService());

            RegisterService(typeof(WebPartChromeTypesConvertService), new DefaultWebPartChromeTypesConvertService());
        }

        #endregion

        #region properties

        public Dictionary<Type, List<object>> Services { get; set; }

        #endregion

        #region methods

        public void RegisterService(Type type, object service)
        {
            if (!Services.ContainsKey(type))
                Services.Add(type, new List<object>());

            var list = Services[type];

            if (!list.Contains(service))
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
