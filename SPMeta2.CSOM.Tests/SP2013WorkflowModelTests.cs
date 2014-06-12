using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Tests.Base;
using SPMeta2.CSOM.Tests.Models;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.CSOM.Tests
{
    //[TestClass]
    public class SP2013WorkflowModelTests : ClientOMSharePointTestBase
    {
        #region resources

        [TestInitialize]
        public void Setup()
        {
            // it is a good place to change TestSetting
            InitTestSettings();
        }

        [TestCleanup]
        public void Cleanup()
        {
            CleanupResources();
        }

        #endregion

        #region tests


        [TestMethod]
        [TestCategory("CSOM")]
        public void CanPublisWorkflowDefinition()
        {
            var model = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
                .AddSP2013Workflow(SP2013WorkflowModels.WriteToHistoryList);

            WithStaticSharePointClientContext(context =>
            {
                ServiceFactory.DeployModel(WebModelHost.FromClientContext(context), model);
            });
        }

        [TestMethod]
        [TestCategory("CSOM")]
        public void CanPublisWorkflowToList()
        {
            var model = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
                     .WithSP2013Workflows(workflows =>
                     {
                         workflows
                             .AddSP2013Workflow(SP2013WorkflowModels.WriteToHistoryList);

                     })
                     .WithLists(lists =>
                     {
                         lists
                             .AddList(ListModels.TestLibrary, list =>
                             {
                                 list
                                     .AddSP2013WorkflowSubscription(
                                         SP2013WorkflowSubscriptionModels.TestDocumentLibrary.WriteToHistoryList);
                             });
                     });

            WithStaticSharePointClientContext(context =>
            {
                ServiceFactory.DeployModel(WebModelHost.FromClientContext(context), model);
            });
        }


        #endregion
    }
}
