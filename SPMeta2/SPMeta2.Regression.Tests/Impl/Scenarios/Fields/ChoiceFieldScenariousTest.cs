using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.CSOM.DefaultSyntax;
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
    public class ChoiceFieldScenariousTest : SPMeta2RegresionScenarioTestBase
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

        protected ChoiceFieldDefinition GetChoiceFieldDefinition()
        {
            return GetChoiceFieldDefinition(null);
        }

        protected ChoiceFieldDefinition GetChoiceFieldDefinition(Action<ChoiceFieldDefinition> action)
        {
            var result = ModelGeneratorService.GetRandomDefinition<ChoiceFieldDefinition>(def =>
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
        [TestCategory("Regression.Scenarios.Fields.ChoiceField")]
        public void CanDeploy_ChoiceField_AsDropdown()
        {
            var field = GetChoiceFieldDefinition(def =>
            {
                def.EditFormat = BuiltInChoiceFormatType.Dropdown;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(field);
            });

            TestModel(siteModel);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields.ChoiceField")]
        public void CanDeploy_ChoiceField_AsRadioButtons()
        {
            var field = GetChoiceFieldDefinition(def =>
            {
                def.EditFormat = BuiltInChoiceFormatType.RadioButtons;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(field);
            });

            TestModel(siteModel);
        }

        #endregion
    }
}
