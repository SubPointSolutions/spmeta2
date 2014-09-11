using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ListScenariosTest : SPMeta2RegresionScenarioTestBase
    {
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

        #region fields

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_GenericList()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_DocumentLibrary()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_CalendarList()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.Events;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_LinksList()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.Links;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_TasksList()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.Tasks;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Lists")]
        public void CanDeploy_TasksWithTimelineAndHierarchyList()
        {
            TestRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.TasksWithTimelineAndHierarchy;
            });
        }

        #endregion
    }
}
