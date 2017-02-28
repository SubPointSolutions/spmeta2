using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers;

using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;

namespace SPMeta2.Regression.Tests.Impl.Scenarios.Fields
{
    [TestClass]
    public class OutcomeChoiceFieldScenariousTest : SPMeta2RegresionScenarioTestBase
    {
        #region internal

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region single select

        protected OutcomeChoiceFieldDefinition GetOutcomeChoiceFieldDefinition()
        {
            return GetOutcomeChoiceFieldDefinition(null);
        }

        protected OutcomeChoiceFieldDefinition GetOutcomeChoiceFieldDefinition(Action<OutcomeChoiceFieldDefinition> action)
        {
            var result = ModelGeneratorService.GetRandomDefinition<OutcomeChoiceFieldDefinition>(def =>
            {
                def.ShowInNewForm = true;
                def.Hidden = false;
                def.Required = false;
            });

            if (action != null)
                action(result);

            return result;
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.OutcomeChoiceField")]
        [SiteCollectionIsolation]
        public void CanDeploy_OutcomeChoiceField_WithOptions()
        {
            var fieldDef = GetOutcomeChoiceFieldDefinition(def =>
            {
                def.Choices.Add(Rnd.String());
                def.Choices.Add(Rnd.String());
                def.Choices.Add(Rnd.String());
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddOutcomeChoiceField(fieldDef);
            });

            TestModel(siteModel);
        }

        #endregion
    }
}
