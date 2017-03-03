using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Common;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Standard.Services;
using SPMeta2.Interfaces;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.Services;
using SPMeta2.SSOM.Standard.Services;
using SPMeta2.Utils;
using SPMeta2.Containers.CSOM;
using SPMeta2.Services.Impl;
using SPMeta2.Syntax.Default;
using SPMeta2.Containers.O365;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using System;
using SPMeta2.CSOM.Services.Impl;
using SPMeta2.Extensions;

namespace SPMeta2.Regression.Impl.Tests.Impl.Services
{
    [TestClass]
    public class ModelHandlersTest
    {
        #region init

        [ClassInitialize]
        public static void Init(TestContext context)
        {

        }

        [ClassCleanup]
        public static void Cleanup()
        {

        }

        #endregion

        #region ProvisionServices

        [TestMethod]
        [TestCategory("Regression.Impl.ProvisionServices")]
        [TestCategory("CI.Core")]
        public void Can_CreateFoundationProvisionService()
        {
            Assert.IsNotNull(new CSOMProvisionService());
            Assert.IsNotNull(new SSOMProvisionService());

        }

        [TestMethod]
        [TestCategory("Regression.Impl.ProvisionServices")]
        [TestCategory("CI.Core")]
        public void Can_CreateStandardProvisionService()
        {
            Assert.IsNotNull(new StandardCSOMProvisionService());
            Assert.IsNotNull(new StandardSSOMProvisionService());

        }

        [TestMethod]
        [TestCategory("Regression.Impl.ProvisionServices.CSOM")]
        [TestCategory("CI.Core")]
        public void EnsureCSOMModelHandlers()
        {
            var service = new CSOMProvisionService();
            var modelHandlers = ReflectionUtils.GetTypesFromAssembly<ModelHandlerBase>(typeof(SPMeta2.CSOM.ModelHandlers.FieldModelHandler).Assembly);

            foreach (var srcHandlerImplType in modelHandlers)
            {
                var dstHandlerImpl = service.ModelHandlers.Values.FirstOrDefault(h => h.GetType() == srcHandlerImplType);
                Assert.IsNotNull(dstHandlerImpl);
            }
        }

        [TestMethod]
        [TestCategory("Regression.Impl.ProvisionServices.CSOM")]
        [TestCategory("CI.Core")]
        public void EnsureStandardCSOMModelHandlers()
        {
            var service = new StandardCSOMProvisionService();

            var modelHandlers = ReflectionUtils.GetTypesFromAssembly<ModelHandlerBase>(typeof(SPMeta2.CSOM.ModelHandlers.FieldModelHandler).Assembly);
            var standardModelHandlers = ReflectionUtils.GetTypesFromAssembly<ModelHandlerBase>(typeof(SPMeta2.CSOM.Standard.ModelHandlers.Fields.TaxonomyFieldModelHandler).Assembly);

            foreach (var srcHandlerImplType in modelHandlers)
            {
                var dstHandlerImpl = service.ModelHandlers.Values.FirstOrDefault(h => h.GetType() == srcHandlerImplType);
                Assert.IsNotNull(dstHandlerImpl);
            }

            foreach (var srcHandlerImplType in standardModelHandlers)
            {
                var dstHandlerImpl = service.ModelHandlers.Values.FirstOrDefault(h => h.GetType() == srcHandlerImplType);
                Assert.IsNotNull(dstHandlerImpl);
            }
        }


        [TestMethod]
        [TestCategory("Regression.Impl.ProvisionServices.SSOM")]
        [TestCategory("CI.Core")]
        public void EnsureSSOMModelHandlers()
        {
            var service = new SSOMProvisionService();
            var modelHandlers = ReflectionUtils.GetTypesFromAssembly<ModelHandlerBase>(typeof(SPMeta2.SSOM.ModelHandlers.FieldModelHandler).Assembly);

            foreach (var srcHandlerImplType in modelHandlers)
            {
                var dstHandlerImpl = service.ModelHandlers.Values.FirstOrDefault(h => h.GetType() == srcHandlerImplType);
                Assert.IsNotNull(dstHandlerImpl);
            }
        }

        [TestMethod]
        [TestCategory("Regression.Impl.ProvisionServices.SSOM")]
        [TestCategory("CI.Core")]
        public void EnsureStandardSSOMModelHandlers()
        {
            var service = new StandardSSOMProvisionService();

            var modelHandlers = ReflectionUtils.GetTypesFromAssembly<ModelHandlerBase>(typeof(SPMeta2.SSOM.ModelHandlers.FieldModelHandler).Assembly);
            var standardModelHandlers = ReflectionUtils.GetTypesFromAssembly<ModelHandlerBase>(typeof(SPMeta2.SSOM.Standard.ModelHandlers.Fields.TaxonomyFieldModelHandler).Assembly);

            foreach (var srcHandlerImplType in modelHandlers)
            {
                var dstHandlerImpl = service.ModelHandlers.Values.FirstOrDefault(h => h.GetType() == srcHandlerImplType);
                Assert.IsNotNull(dstHandlerImpl);
            }

            foreach (var srcHandlerImplType in standardModelHandlers)
            {
                var dstHandlerImpl = service.ModelHandlers.Values.FirstOrDefault(h => h.GetType() == srcHandlerImplType);
                Assert.IsNotNull(dstHandlerImpl);
            }
        }

        #endregion

        #region incremental proivision services

        [TestMethod]
        [TestCategory("Regression.Impl.IncrementalProvisionService")]
        [TestCategory("CI.Core")]
        public void Can_Create_IncrementalProvisionServices()
        {
            var services = new List<ProvisionServiceBase>();

            services.Add(new CSOMIncrementalProvisionService());
            services.Add(new StandardCSOMIncrementalProvisionService());

            services.Add(new SSOMIncrementalProvisionService());
            services.Add(new StandardSSOMIncrementalProvisionService());

            foreach (var service in services)
            {
                var incrementalService = service as IIncrementalProvisionService;

                Assert.IsNotNull(incrementalService.PreviousModelHash);
                Assert.IsNotNull(incrementalService.CurrentModelHash);
            }
        }

        [TestMethod]
        [TestCategory("Regression.Impl.IncrementalProvisionService")]
        [TestCategory("CI.Core")]
        public void Can_Create_IncrementalProvisionServices_With_FluentAPI()
        {
            var services = new List<ProvisionServiceBase>();

            services.Add(new CSOMProvisionService());
            services.Add(new StandardCSOMProvisionService());

            services.Add(new SSOMProvisionService());
            services.Add(new StandardSSOMProvisionService());

            foreach (var service in services)
            {
                var incrementalService = service.SetIncrementalProvisionMode();

                var currentModelHash = incrementalService.GetIncrementalProvisionModelHash();

                Assert.IsNotNull(currentModelHash);
            }
        }



        #endregion

        #region persistence storage


        [TestMethod]
        [TestCategory("Regression.Impl.IncrementalProvisionService.PersistenceStorage")]
        [TestCategory("CI.Core")]
        [ExpectedException(typeof(SPMeta2Exception))]
        public void Can_Provision_Incrementally_With_NoIncrementalModelId()
        {
            var provisionRunner = new O365ProvisionRunner();
            var provisionService = provisionRunner.ProvisionService;

            var incrementalProvisionConfig = new IncrementalProvisionConfig();
            incrementalProvisionConfig.PersistenceStorages.Add(new DefaultFileSystemPersistenceStorage());

            provisionService.SetIncrementalProvisionMode(incrementalProvisionConfig);

            provisionRunner.SiteUrls.ForEach(siteUrl =>
            {
                var model = SPMeta2Model.NewSiteModel(site => { });

                provisionRunner.WithO365Context(siteUrl, context =>
                {
                    provisionService.DeployModel(SiteModelHost.FromClientContext(context), model);
                });
            });
        }

        [TestMethod]
        [TestCategory("Regression.Impl.IncrementalProvisionService.PersistenceStorage")]
        [TestCategory("CI.Core")]
        public void Can_Provision_Incrementally_With_FileSystemStorage()
        {
            var provisionRunner = new O365ProvisionRunner();
            var provisionService = provisionRunner.ProvisionService;

            var incrementalProvisionConfig = new IncrementalProvisionConfig();
            incrementalProvisionConfig.PersistenceStorages.Add(new DefaultFileSystemPersistenceStorage());

            provisionService.SetIncrementalProvisionMode(incrementalProvisionConfig);

            provisionRunner.SiteUrls.ForEach(siteUrl =>
            {
                var incrementalModelId = "m2.regression." + Guid.NewGuid().ToString("N");

                var model = SPMeta2Model.NewSiteModel(site =>
                {

                });

                model.SetIncrementalProvisionModelId(incrementalModelId);

                provisionRunner.WithO365Context(siteUrl, context =>
                {
                    provisionService.DeployModel(SiteModelHost.FromClientContext(context), model);
                });
            });
        }

        [TestMethod]
        [TestCategory("Regression.Impl.IncrementalProvisionService.PersistenceStorage")]
        [TestCategory("CI.Core.O365")]
        public void Can_Provision_Incrementally_With_CSOMWebPropertyBagStorage()
        {
            var provisionRunner = new O365ProvisionRunner();
            var provisionService = provisionRunner.ProvisionService;

            var incrementalModelId = "m2.regression." + Guid.NewGuid().ToString("N");

            provisionRunner.SiteUrls.ForEach(siteUrl =>
            {
                provisionRunner.WithO365Context(siteUrl, context =>
                {
                    for (var i = 0; i < 3; i++)
                    {
                        var incrementalProvisionConfig = new IncrementalProvisionConfig();
                        incrementalProvisionConfig.PersistenceStorages.Add(
                            new DefaultCSOMWebPropertyBagStorage(context.Web));

                        provisionService.SetIncrementalProvisionMode(incrementalProvisionConfig);

                        var model = SPMeta2Model.NewSiteModel(site =>
                        {

                        });

                        model.SetIncrementalProvisionModelId(incrementalModelId);


                        provisionService.DeployModel(SiteModelHost.FromClientContext(context), model);
                    }
                });
            });
        }

        #endregion
    }
}
