using SPMeta2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
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
            Services = new Dictionary<Type, object>();

            InitServices();
        }

        private void InitServices()
        {
            Services.Add(typeof(TraceServiceBase), new TraceSourceService());

            Services.Add(typeof(ModelTreeTraverseServiceBase), new DefaultModelTreeTraverseService());
            Services.Add(typeof(ModelWeighServiceBase), new DefaultModelWeighService());
        }

        #endregion

        #region properties

        public Dictionary<Type, object> Services { get; set; }

        #endregion

        #region methods

        public TService GetService<TService>()
           where TService : class
        {
            object service;

            Services.TryGetValue(typeof(TService), out service);

            return service as TService;
        }

        #endregion

        #region properties

        public static ServiceContainer Instance { get; set; }

        #endregion
    }
}
