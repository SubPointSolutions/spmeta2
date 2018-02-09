using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Services;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Regression.ModelHandlers;

namespace SPMeta2.Regression.Tests.Impl.Services
{

    public class TestModelServiceBase : ModelServiceBase
    {

    }

    [TestClass]
    public class ModelServiceBaseTests
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.Services.ModelServiceBase")]
        [TestCategory("CI.Core")]
        public void OnModelProcessing_ProcessedModelNodeCount()
        {
            // ProcessedModelNodeCount is different for single provision operation #1066
            // https://github.com/SubPointSolutions/spmeta2/issues/1066

            var provisionService = new TestModelServiceBase();

            var hasProcessingEventHit = false;
            var hasProcessedEventHit = false;

            var expectedProcessingReults = new Dictionary<int, int>() {
                { 1,3 },
                { 2,3 },
                { 3,3 }
            };

            var expectedProcessedReults = new Dictionary<int, int>() {
                { 1,3 },
                { 2,3 },
                { 3,3 }
            };

            var processingResults = new Dictionary<int, int>();
            var processedResults = new Dictionary<int, int>();

            provisionService.OnModelNodeProcessing += (s, e) =>
            {
                processingResults.Add(e.ProcessedModelNodeCount, e.TotalModelNodeCount);
                hasProcessingEventHit = true;
            };

            provisionService.OnModelNodeProcessed += (s, e) =>
            {
                processedResults.Add(e.ProcessedModelNodeCount, e.TotalModelNodeCount);
                hasProcessedEventHit = true;
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomField();
                site.AddRandomField();
            });

            provisionService.ModelHandlers.Add(typeof(SiteDefinition), new EmptyModelhandler());
            provisionService.ModelHandlers.Add(typeof(FieldDefinition), new EmptyModelhandler());

            provisionService.DeployModel(null, model);

            // hits
            Assert.IsTrue(hasProcessingEventHit);
            Assert.IsTrue(hasProcessedEventHit);

            // processing / processed results
            foreach (var result in processingResults.Keys)
            {
                var resultTotal = processingResults[result];

                Assert.IsTrue(expectedProcessedReults.ContainsKey(result));
                Assert.IsTrue(expectedProcessedReults[result] == resultTotal);
            }

            foreach (var result in processedResults.Keys)
            {
                var resultTotal = processedResults[result];

                Assert.IsTrue(processedResults.ContainsKey(result));
                Assert.IsTrue(processedResults[result] == resultTotal);
            }
        }

        #endregion
    }
}
