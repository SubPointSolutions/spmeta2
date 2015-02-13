using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class EventReceiverScenariosTest : SPMeta2RegresionScenarioTestBase
    {
        #region constructors

        public EventReceiverScenariosTest()
        {
            RegressionService.ProvisionGenerationCount = 2;
        }

        #endregion

        #region internal

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanupAttribute]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region test

        [TestMethod]
        [TestCategory("Regression.Scenarios.EventReceivers")]
        public void CanDeploy_ListItemEventReceiver()
        {
            var eventReceiver = ModelGeneratorService.GetRandomDefinition<EventReceiverDefinition>();

            eventReceiver.Assembly = "Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c";
            eventReceiver.Class = "Microsoft.SharePoint.Help.HelpLibraryEventReceiver";

            eventReceiver.Synchronization = BuiltInEventReceiverSynchronization.Synchronous;
            eventReceiver.Type = BuiltInEventReceiverType.ItemAdded;

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomList(list =>
                    {
                        list.AddEventReceiver(eventReceiver);
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.EventReceivers")]
        public void CanDeploy_ContentTypeEventReceiver()
        {
            WithExcpectedExceptions(new[] { typeof(SPMeta2UnsupportedModelHostException) }, () =>
            {
                var eventReceiver = ModelGeneratorService.GetRandomDefinition<EventReceiverDefinition>();

                eventReceiver.Assembly = "Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c";
                eventReceiver.Class = "Microsoft.SharePoint.Help.HelpLibraryEventReceiver";

                eventReceiver.Synchronization = BuiltInEventReceiverSynchronization.Synchronous;
                eventReceiver.Type = BuiltInEventReceiverType.ItemAdded;

                var model = SPMeta2Model
                    .NewSiteModel(site =>
                    {
                        site.AddRandomContentType(contentType =>
                        {
                            contentType.AddEventReceiver(eventReceiver);
                        });
                    });

                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.EventReceivers")]
        public void CanDeploy_ListEventReceiver()
        {
            var eventReceiver = ModelGeneratorService.GetRandomDefinition<EventReceiverDefinition>();

            eventReceiver.Assembly = "Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c";
            eventReceiver.Class = "Microsoft.SharePoint.Publishing.Internal.VariationsRelationshipListEventReceiver";

            eventReceiver.Synchronization = BuiltInEventReceiverSynchronization.Synchronous;
            eventReceiver.Type = BuiltInEventReceiverType.ListDeleting;

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddEventReceiver(eventReceiver);
                });

            TestModel(model);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.EventReceivers")]
        public void CanDeploy_WebEventReceiver()
        {
            var eventReceiver = ModelGeneratorService.GetRandomDefinition<EventReceiverDefinition>();

            eventReceiver.Assembly = "Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c";
            eventReceiver.Class = "Microsoft.SharePoint.Publishing.CPVAreaEventReceiver";

            eventReceiver.Synchronization = BuiltInEventReceiverSynchronization.Synchronous;
            eventReceiver.Type = BuiltInEventReceiverType.WebAdding;

            var model = SPMeta2Model
                .NewSiteModel(site =>
                {
                    site.AddEventReceiver(eventReceiver);
                });

            TestModel(model);
        }

        #endregion
    }
}
