using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class CalculatedFieldScenariousTest : SPMeta2RegresionScenarioTestBase
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

        #region utils

        protected CalculatedFieldDefinition GetCalculatedFieldDefinition()
        {
            return GetCalculatedFieldDefinition(null);
        }

        protected CalculatedFieldDefinition GetCalculatedFieldDefinition(Action<CalculatedFieldDefinition> action)
        {
            var result = ModelGeneratorService.GetRandomDefinition<CalculatedFieldDefinition>(def =>
            {
                def.ShowInNewForm = true;
                def.ShowInEditForm = true;
                def.ShowInDisplayForm = true;

                def.ShowInListSettings = true;

                def.Hidden = false;
                def.Required = false;
            });

            if (action != null)
                action(result);

            return result;
        }

        protected FieldDefinition GetRandomFieldWithType(string type)
        {
            return ModelGeneratorService.GetRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = type;

                def.ShowInNewForm = true;
                def.ShowInEditForm = true;
                def.ShowInDisplayForm = true;

                def.ShowInListSettings = true;

                def.Hidden = false;
                def.Required = false;
            });
        }

        #endregion

        #region formula changes


        //[TestMethod]
        //[TestCategory("Regression.Scenarios.Fields.CalculatedField")]
        //public void CanDeploy_CalculatedSiteField_AndChangeFormula()
        //{
        //    //RegressionService.EnableDefinitionValidation = false;
        //    //RegressionService.EnableEventValidation = false;

        //    var field = GetCalculatedFieldDefinition(def =>
        //    {
        //        def.OutputType = BuiltInFieldTypes.Text;
        //    });

        //    var firstIntField = GetRandomFieldWithType(BuiltInFieldTypes.Integer);
        //    var firstTextField = GetRandomFieldWithType(BuiltInFieldTypes.Text);
        //    var firstNumberField = GetRandomFieldWithType(BuiltInFieldTypes.Number);

        //    var secondIntField = GetRandomFieldWithType(BuiltInFieldTypes.Integer);
        //    var secondTextField = GetRandomFieldWithType(BuiltInFieldTypes.Text);
        //    var secondNumberField = GetRandomFieldWithType(BuiltInFieldTypes.Number);

        //    field.FieldReferences = new Collection<string>()
        //    {
        //        firstTextField.InternalName,
        //            firstTextField.InternalName,
        //            firstTextField.InternalName,

        //            secondIntField.InternalName,
        //            secondTextField.InternalName,
        //            secondNumberField.InternalName

        //    };

        //    var siteModel = SPMeta2Model.NewSiteModel(site =>
        //    {
        //        site.AddField(firstIntField);
        //        site.AddField(firstTextField);
        //        site.AddField(firstNumberField);

        //        site.AddField(secondIntField);
        //        site.AddField(secondTextField);
        //        site.AddField(secondNumberField);

        //        site.AddField(field);
        //    });

        //    field.Formula = string.Format("=CONCATENATE([{0}],[{1}],[{2}])",
        //        new String[]
        //        {
        //            firstTextField.InternalName,
        //            firstTextField.InternalName,
        //            firstTextField.InternalName,
        //        });

        //    RegressionService.ProvisionGenerationCount = 1;

        //    TestModel(siteModel);
        //    TestModel(siteModel);

        //    field.Formula = string.Format("=CONCATENATE([{0}],[{1}],[{2}])",
        //       new String[]
        //        {
        //            secondIntField.InternalName,
        //            secondTextField.InternalName,
        //            secondNumberField.InternalName,
        //        });

        //    TestModel(siteModel);
        //    TestModel(siteModel);
        //}


        //[TestMethod]
        //[TestCategory("Regression.Scenarios.Fields.CalculatedField")]
        //public void CanDeploy_CalculatedFieldToList_AndChangeFormula()
        //{
        //    RegressionService.ProvisionGenerationCount = 1;

        //    var calcField = GetCalculatedFieldDefinition(def =>
        //    {
        //        def.OutputType = BuiltInFieldTypes.Text;
        //    });

        //    var firstIntField = GetRandomFieldWithType(BuiltInFieldTypes.Integer);
        //    var firstTextField = GetRandomFieldWithType(BuiltInFieldTypes.Text);
        //    var firstNumberField = GetRandomFieldWithType(BuiltInFieldTypes.Number);

        //    var secondIntField = GetRandomFieldWithType(BuiltInFieldTypes.Integer);
        //    var secondTextField = GetRandomFieldWithType(BuiltInFieldTypes.Text);
        //    var secondNumberField = GetRandomFieldWithType(BuiltInFieldTypes.Number);

        //    var contentTypeWithCalcField = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
        //    {
        //        def.Hidden = false;
        //        def.ParentContentTypeId = BuiltInContentTypeId.Item;
        //    });

        //    var listWithCalcContentType = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
        //    {
        //        def.ContentTypesEnabled = true;
        //        def.Hidden = false;
        //        def.TemplateType = BuiltInListTemplateTypeId.GenericList;
        //    });

        //    calcField.FieldReferences = new Collection<string>()
        //    {
        //            firstIntField.InternalName,
        //            firstTextField.InternalName,
        //            firstNumberField.InternalName,

        //            secondIntField.InternalName,
        //            secondTextField.InternalName,
        //            secondNumberField.InternalName,
        //            "Title"
        //    };

        //    var siteModel = SPMeta2Model.NewSiteModel(site =>
        //    {
        //        site.AddField(firstIntField);
        //        site.AddField(firstTextField);
        //        site.AddField(firstNumberField);

        //        site.AddField(secondIntField);
        //        site.AddField(secondTextField);
        //        site.AddField(secondNumberField);

        //        site.AddField(calcField);

        //        site.AddContentType(contentTypeWithCalcField, contentType =>
        //        {
        //            contentType
        //                .AddContentTypeFieldLink(firstIntField)
        //                .AddContentTypeFieldLink(firstTextField)
        //                .AddContentTypeFieldLink(firstNumberField)

        //                .AddContentTypeFieldLink(secondIntField)
        //                .AddContentTypeFieldLink(secondTextField)
        //                .AddContentTypeFieldLink(secondNumberField)

        //                .AddContentTypeFieldLink(calcField);
        //        });
        //    });

        //    calcField.Formula = string.Format("=[Title]");

        //    //calcField.Formula = string.Format("=CONCATENATE([{0}],[{1}],[{2}])",
        //    //    new String[]
        //    //    {
        //    //        firstIntField.InternalName,
        //    //        firstNumberField.InternalName,
        //    //        firstTextField.InternalName,
        //    //    });

        //    TestModel(siteModel);
        //    TestModel(siteModel);

        //    var webModel = SPMeta2Model.NewWebModel(web =>
        //    {
        //        web.AddList(listWithCalcContentType, list =>
        //        {
        //            list.AddContentTypeLink(contentTypeWithCalcField);
        //        });
        //    });

        //    TestModel(webModel);
        //    TestModel(webModel);

        //    calcField.Formula = string.Format("=CONCATENATE([{0}],[{1}],[{2}])",
        //        new String[]
        //        {
        //            firstIntField.InternalName,
        //            firstNumberField.InternalName,
        //            firstTextField.InternalName,
        //        });

        //    var subSiteModel = SPMeta2Model.NewSiteModel(site =>
        //    {
        //        site.AddField(calcField);

        //        //site.AddContentType(contentTypeWithCalcField, contentType =>
        //        //{
        //        //    contentType
        //        //        .AddContentTypeFieldLink(calcField);
        //        //});
        //    });

        //    TestModel(subSiteModel);

        //    calcField.Formula = string.Format("=CONCATENATE([{0}],[{1}],[{2}])",
        //       new String[]
        //        {
        //            firstIntField.InternalName,
        //            firstNumberField.InternalName,
        //            firstTextField.InternalName,
        //        });

        //    TestModel(siteModel);
        //    TestModel(siteModel);
        //}

        #endregion
    }
}
