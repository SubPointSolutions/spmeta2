using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Assertion;
using SPMeta2.Containers.Exceptions;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Standard.DefinitionGenerators;
using SPMeta2.Containers.Utils;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Exceptions;
using SPMeta2.Extensions;
using SPMeta2.Models;

using SPMeta2.Regression.Tests.Services;
using SPMeta2.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Enumerations;
using SPMeta2.Validation.Services;
using System.Collections.ObjectModel;
using SPMeta2.Standard.Enumerations;
using System.Text;

namespace SPMeta2.Regression.Tests.Base
{
    public class SPMeta2RegresionTestBase
    {
        #region constructors

        public SPMeta2RegresionTestBase()
        {
            RegressionService.EnableDefinitionProvision = true;
            RegressionService.ProvisionGenerationCount = 2;

            RegressionService.EnableDefinitionValidation = true;

            RegressionService.ShowOnlyFalseResults = true;

            EnablePropertyUpdateValidation = false;
            PropertyUpdateGenerationCount = 2;

            TestOptions = new RunOptions();

            TestOptions.EnableWebApplicationDefinitionTest = false;

        }

        #endregion

        #region static

        public int PropertyUpdateGenerationCount { get; set; }

        protected static void InternalCleanup()
        {

        }

        protected static void InternalInit()
        {
            RegressionAssertService.OnPropertyValidated += OnModelPropertyValidated;
        }

        protected static void OnModelPropertyValidated(object sender, OnPropertyValidatedEventArgs e)
        {
            RegressionService.OnModelPropertyValidated(sender, e);
        }

        static SPMeta2RegresionTestBase()
        {
            RegressionService = new RegressionTestService();

            RegressionService.AssertService = new VSAssertService();

            RegressionService.EnableDefinitionValidation = true;
            RegressionService.ModelGeneratorService.RegisterDefinitionGenerators(typeof(ImageRenditionDefinitionGenerator).Assembly);
        }

        #endregion

        #region classes

        protected class RunOptions
        {
            public bool EnableWebApplicationDefinitionTest { get; set; }
        }

        #endregion

        #region properties

        public bool EnablePropertyNullableValidation { get; set; }
        public int PropertyNullableGenerationCount { get; set; }

        protected RunOptions TestOptions { get; set; }
        public bool EnablePropertyUpdateValidation { get; set; }

        public static RegressionTestService RegressionService { get; set; }

        public ModelGeneratorService ModelGeneratorService
        {
            get { return RegressionService.ModelGeneratorService; }
        }

        #endregion

        #region testing API

        protected void WithDisabledPropertyUpdateValidation(Action action)
        {
            var _oldEnablePropertyUpdateValidation = EnablePropertyUpdateValidation;


            try
            {
                EnablePropertyUpdateValidation = false;
                action();
            }
            finally
            {
                EnablePropertyUpdateValidation = _oldEnablePropertyUpdateValidation;
            }
        }

        protected void TestRandomDefinition<TDefinition>()
           where TDefinition : DefinitionBase, new()
        {
            TestRandomDefinition<TDefinition>(null);
        }

        protected void TestRandomDefinition<TDefinition>(Action<TDefinition> definitionSetup)
            where TDefinition : DefinitionBase, new()
        {
            var model = RegressionService.TestRandomDefinition(definitionSetup);

            PleaseMakeSureWeCanUpdatePropertiesForTheSharePointSake(new[] { model });
        }

        protected void WithSPMeta2NotSupportedExceptions(Action action)
        {
            WithExcpectedExceptions(new Type[] {
                typeof(SPMeta2NotSupportedException)
             
            }, action);
        }

        protected void WithExpectedUnsupportedCSOMnO365RunnerExceptions(Action action)
        {
            WithExcpectedExceptions(new Type[] {
                typeof(SPMeta2UnsupportedCSOMRunnerException),
                typeof(SPMeta2UnsupportedO365RunnerException)
            }, action);
        }

        protected void WithExcpectedException(Type exceptionType, Action action)
        {
            WithExcpectedExceptions(new[] { exceptionType }, action);
        }

        protected void WithExcpectedExceptions(IEnumerable<Type> exceptionTypes, Action action)
        {
            RegressionService.WithExcpectedExceptions(exceptionTypes, action);
        }

        protected void TestModel(ModelNode model)
        {
            TestModels(new[] { model });
        }

        protected void TestModel(ModelNode firstModel, ModelNode secondModel)
        {
            TestModels(new[] { firstModel, secondModel });
        }



        protected void PleaseMakeSureWeCanUpdatePropertiesForTheSharePointSake(IEnumerable<ModelNode> models)
        {
            if (EnablePropertyUpdateValidation)
            {
                for (int index = 0; index < PropertyUpdateGenerationCount; index++)
                {
                    ProcessPropertyUpdateValidation(models);
                    RegressionService.TestModels(models);
                }
            }

            if (EnablePropertyNullableValidation)
            {
                for (int index = 0; index < PropertyNullableGenerationCount; index++)
                {
                    ProcessPropertyNullableValidation(models);
                    RegressionService.TestModels(models);
                }
            }
        }

        private void ProcessPropertyNullableValidation(IEnumerable<ModelNode> models)
        {
            foreach (var model in models)
            {
                model.WithNodesOfType<DefinitionBase>(node =>
                {
                    var def = node.Value;
                    ProcessDefinitionsPropertyNulableValidation(def);
                });
            }
        }



        protected void TestModels(IEnumerable<ModelNode> models)
        {
            RegressionService.TestModels(models);
            PleaseMakeSureWeCanUpdatePropertiesForTheSharePointSake(models);
        }

        private void ProcessPropertyUpdateValidation(IEnumerable<ModelNode> models)
        {
            foreach (var model in models)
            {
                model.WithNodesOfType<DefinitionBase>(node =>
                {
                    var def = node.Value;
                    ProcessDefinitionsPropertyUpdateValidation(def);
                });
            }
        }


        private void ProcessDefinitionsPropertyNulableValidation(DefinitionBase def)
        {
            var nullableProps = def.GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(ExpectNullable), true).Count() > 0);


            TraceUtils.WithScope(trace =>
            {
                trace.WriteLine("");

                trace.WriteLine(string.Format("[INF]\tPROPERTY NULLABLE VALIDATION"));
                trace.WriteLine(string.Format("[INF]\tModel of type: [{0}] - [{1}]", def.GetType(), def));

                if (nullableProps.Count() == 0)
                {
                    trace.WriteLine(string.Format("[INF]\tNo properties to be validated. Skipping."));
                }
                else
                {
                    foreach (var prop in nullableProps)
                    {
                        trace.WriteLine(string.Format("[INF]\tSetting NULLABLE property: [" + prop.Name + "]"));
                        prop.SetValue(def, null);
                    }
                }

            });
        }

        private void ProcessDefinitionsPropertyUpdateValidation(DefinitionBase def)
        {
            var updatableProps = def.GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(ExpectUpdate), true).Count() > 0);


            TraceUtils.WithScope(trace =>
            {
                trace.WriteLine("");

                trace.WriteLine(string.Format("[INF]\tPROPERTY UPDATE VALIDATION"));
                trace.WriteLine(string.Format("[INF]\tModel of type: [{0}] - [{1}]", def.GetType(), def));

                if (updatableProps.Count() == 0)
                {
                    trace.WriteLine(string.Format("[INF]\tNo properties to be validated. Skipping."));
                }
                else
                {
                    foreach (var prop in updatableProps)
                    {
                        object newValue = null;

                        var attrs = prop.GetCustomAttributes(typeof(ExpectUpdate), true);

                        if (attrs.Count(a => a is ExpectUpdateAsLCID) > 0)
                        {
                            var newLocaleIdValue = 1033 + RegressionService.RndService.Int(5);

                            if (prop.PropertyType == typeof(int))
                                newValue = newLocaleIdValue;
                            else if (prop.PropertyType == typeof(int?))
                                newValue = RegressionService.RndService.Bool() ? (int?)null : newLocaleIdValue;
                            else if (prop.PropertyType == typeof(uint))
                                newValue = (uint)newLocaleIdValue;
                            else if (prop.PropertyType == typeof(uint?))
                                newValue = (uint?)(RegressionService.RndService.Bool() ? (uint?)null : (uint?)newLocaleIdValue);
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsUser) > 0)
                        {
                            var newUserValue = RegressionService.RndService.UserLogin();
                            newValue = newUserValue;
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsStandalone) > 0)
                        {
                            var values = new List<string>();

                            values.Add("Override");
                            values.Add("Standalone");

                            if (prop.PropertyType == typeof(string))
                                newValue = values[RegressionService.RndService.Int(values.Count - 1)];
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsCompatibleSearchDataTypes) > 0)
                        {
                            var values = new List<string>();

                            values.Add(BuiltInCompatibleSearchDataTypes.Integer);
                            values.Add(BuiltInCompatibleSearchDataTypes.DateTime);
                            values.Add(BuiltInCompatibleSearchDataTypes.Decimal);
                            values.Add(BuiltInCompatibleSearchDataTypes.Text);
                            values.Add(BuiltInCompatibleSearchDataTypes.YesNo);

                            if (prop.PropertyType == typeof(string))
                                newValue = values[RegressionService.RndService.Int(values.Count - 1)];

                            if (prop.PropertyType == typeof(List<string>))
                            {
                                var result = new List<string>();
                                var resultLength = RegressionService.RndService.Int(values.Count - 1);

                                for (var index = 0; index < resultLength; index++)
                                    result.Add(values[index]);

                                newValue = result;
                            }
                        }

                        else if (attrs.Count(a => a is ExpectUpdateAsTargetControlType) > 0)
                        {
                            var values = new List<string>();

                            values.Add(BuiltInTargetControlType.ContentWebParts);
                            values.Add(BuiltInTargetControlType.Custom);
                            values.Add(BuiltInTargetControlType.Refinement);
                            values.Add(BuiltInTargetControlType.SearchBox);
                            values.Add(BuiltInTargetControlType.SearchHoverPanel);
                            values.Add(BuiltInTargetControlType.SearchResults);

                            if (prop.PropertyType == typeof(string))
                                newValue = values[RegressionService.RndService.Int(values.Count - 1)];

                            if (prop.PropertyType == typeof(List<string>))
                            {
                                var result = new List<string>();
                                var resultLength = RegressionService.RndService.Int(values.Count - 1);

                                for (var index = 0; index < resultLength; index++)
                                    result.Add(values[index]);

                                newValue = result;
                            }
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsWebPartPageLayoutTemplate) > 0)
                        {
                            var values = new List<int>();

                            values.Add(BuiltInWebpartPageTemplateId.spstd1);
                            values.Add(BuiltInWebpartPageTemplateId.spstd2);
                            values.Add(BuiltInWebpartPageTemplateId.spstd3);
                            values.Add(BuiltInWebpartPageTemplateId.spstd4);
                            values.Add(BuiltInWebpartPageTemplateId.spstd5);
                            values.Add(BuiltInWebpartPageTemplateId.spstd6);
                            values.Add(BuiltInWebpartPageTemplateId.spstd7);

                            if (prop.PropertyType == typeof(int))
                                newValue = values[RegressionService.RndService.Int(values.Count - 1)];
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsDateTimeFieldCalendarType) > 0)
                        {
                            var values = new List<string>();

                            values.Add(BuiltInCalendarType.Gregorian);
                            values.Add(BuiltInCalendarType.Korea);
                            values.Add(BuiltInCalendarType.Hebrew);
                            values.Add(BuiltInCalendarType.GregorianArabic);
                            values.Add(BuiltInCalendarType.SakaEra);

                            if (prop.PropertyType == typeof(string))
                                newValue = values[RegressionService.RndService.Int(values.Count - 1)];
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsFieldUserSelectionMode) > 0)
                        {
                            var curentValue = prop.GetValue(def) as string;

                            if (curentValue == BuiltInFieldUserSelectionMode.PeopleAndGroups)
                                newValue = BuiltInFieldUserSelectionMode.PeopleOnly;
                            else
                                newValue = BuiltInFieldUserSelectionMode.PeopleAndGroups;
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsDateTimeFieldCalendarType) > 0)
                        {
                            var values = new List<string>();

                            values.Add(BuiltInCalendarType.Gregorian);
                            values.Add(BuiltInCalendarType.Korea);
                            values.Add(BuiltInCalendarType.Hebrew);
                            values.Add(BuiltInCalendarType.GregorianArabic);
                            values.Add(BuiltInCalendarType.SakaEra);

                            if (prop.PropertyType == typeof(string))
                                newValue = values[RegressionService.RndService.Int(values.Count - 1)];
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsChromeState) > 0)
                        {
                            var values = new List<string>();

                            values.Add(BuiltInPartChromeState.Minimized);
                            values.Add(BuiltInPartChromeState.Normal);

                            if (prop.PropertyType == typeof(string))
                                newValue = values[RegressionService.RndService.Int(values.Count - 1)];
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsChromeType) > 0)
                        {
                            var values = new List<string>();

                            //values.Add(BuiltInPartChromeType.BorderOnly);
                            values.Add(BuiltInPartChromeType.Default);
                            values.Add(BuiltInPartChromeType.None);
                            //values.Add(BuiltInPartChromeType.TitleAndBorder);
                            //values.Add(BuiltInPartChromeType.TitleOnly);

                            if (prop.PropertyType == typeof(string))
                                newValue = values[RegressionService.RndService.Int(values.Count - 1)];
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsDateTimeFieldDisplayFormat) > 0)
                        {
                            var curentValue = prop.GetValue(def) as string;

                            if (curentValue == BuiltInDateTimeFieldFormatType.DateOnly)
                                newValue = BuiltInDateTimeFieldFormatType.DateTime;
                            else
                                newValue = BuiltInDateTimeFieldFormatType.DateOnly;
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsDateTimeFieldFriendlyDisplayFormat) > 0)
                        {
                            var curentValue = prop.GetValue(def) as string;

                            if (curentValue == BuiltInDateTimeFieldFriendlyFormatType.Disabled)
                                newValue = BuiltInDateTimeFieldFriendlyFormatType.Relative;
                            else if (curentValue == BuiltInDateTimeFieldFriendlyFormatType.Relative)
                                newValue = BuiltInDateTimeFieldFriendlyFormatType.Unspecified;
                            else if (curentValue == BuiltInDateTimeFieldFriendlyFormatType.Unspecified)
                                newValue = BuiltInDateTimeFieldFriendlyFormatType.Disabled;
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsByte) > 0)
                        {
                            if (prop.PropertyType == typeof(int?) ||
                                prop.PropertyType == typeof(int?))
                                newValue = Convert.ToInt32(RegressionService.RndService.Byte().ToString());
                            else
                            {
                                // TODO, as per case
                                newValue = RegressionService.RndService.Byte();
                            }
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsCamlQuery) > 0)
                        {
                            newValue =
                                string.Format(
                                    "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where>",
                                    RegressionService.RndService.String());
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsIntRange) > 0)
                        {
                            var attr =
                                attrs.FirstOrDefault(a => a is ExpectUpdateAsIntRange) as ExpectUpdateAsIntRange;

                            var minValue = attr.MinValue;
                            var maxValue = attr.MaxValue;

                            var tmpValue = minValue + RegressionService.RndService.Int(maxValue - minValue);

                            if (prop.PropertyType == typeof(double?) ||
                                prop.PropertyType == typeof(double))
                                newValue = Convert.ToDouble(tmpValue);
                            else
                            {
                                // TODO, as per case
                                newValue = tmpValue;
                            }

                        }

                        else if (attrs.Count(a => a is ExpectUpdateAsUrl) > 0)
                        {
                            var attr =
                                attrs.FirstOrDefault(a => a is ExpectUpdateAsUrl) as ExpectUpdateAsUrl;
                            var fileExtension = attr.Extension;

                            if (!fileExtension.StartsWith("."))
                                fileExtension = "." + fileExtension;

                            newValue = string.Format("http://regression-ci.com/{0}{1}",
                                RegressionService.RndService.String(), fileExtension);
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsFileName) > 0)
                        {
                            var attr =
                                attrs.FirstOrDefault(a => a is ExpectUpdateAsFileName) as ExpectUpdateAsFileName;
                            var fileExtension = attr.Extension;

                            if (!fileExtension.StartsWith("."))
                                fileExtension = "." + fileExtension;

                            newValue = string.Format("{0}{1}", RegressionService.RndService.String(), fileExtension);
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsInternalFieldName) > 0)
                        {
                            var values = new List<string>();

                            values.Add(BuiltInInternalFieldNames.ID);
                            values.Add(BuiltInInternalFieldNames.Edit);
                            values.Add(BuiltInInternalFieldNames.Created);
                            values.Add(BuiltInInternalFieldNames._Author);

                            if (prop.PropertyType == typeof(string))
                                newValue = values[RegressionService.RndService.Int(values.Count - 1)];

                            if (prop.PropertyType == typeof(Collection<string>))
                            {
                                var result = new Collection<string>();
                                var resultLength = RegressionService.RndService.Int(values.Count - 1);

                                for (var index = 0; index < resultLength; index++)
                                    result.Add(values[index]);

                                newValue = result;
                            }
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsPageLayoutFileName) > 0)
                        {
                            var values = new List<string>();

                            values.Add(BuiltInPublishingPageLayoutNames.ArticleLeft);
                            values.Add(BuiltInPublishingPageLayoutNames.ArticleLinks);
                            values.Add(BuiltInPublishingPageLayoutNames.ArticleRight);
                            values.Add(BuiltInPublishingPageLayoutNames.BlankWebPartPage);
                            values.Add(BuiltInPublishingPageLayoutNames.CatalogArticle);
                            values.Add(BuiltInPublishingPageLayoutNames.CatalogWelcome);
                            values.Add(BuiltInPublishingPageLayoutNames.EnterpriseWiki);

                            if (prop.PropertyType == typeof(string))
                                newValue = values[RegressionService.RndService.Int(values.Count - 1)];

                            if (prop.PropertyType == typeof(Collection<string>))
                            {
                                var result = new Collection<string>();
                                var resultLength = RegressionService.RndService.Int(values.Count - 1);

                                for (var index = 0; index < resultLength; index++)
                                    result.Add(values[index]);

                                newValue = result;
                            }
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsUrlFieldFormat) > 0)
                        {
                            var curentValue = prop.GetValue(def) as string;

                            if (curentValue == BuiltInUrlFieldFormatType.Hyperlink)
                                newValue = BuiltInUrlFieldFormatType.Image;
                            else
                                newValue = BuiltInUrlFieldFormatType.Hyperlink;
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsCalculatedFieldFormula) > 0)
                        {
                            newValue = string.Format("=ID*{0}", RegressionService.RndService.Int(100));
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAssCalculatedFieldOutputType) > 0)
                        {
                            var curentValue = prop.GetValue(def) as string;

                            if (curentValue == BuiltInFieldTypes.Number)
                                newValue = BuiltInFieldTypes.Text;
                            else
                                newValue = BuiltInFieldTypes.Number;
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAssCalculatedFieldReferences) > 0)
                        {
                            var values = new List<string>();

                            values.Add(BuiltInInternalFieldNames.ID);
                            values.Add(BuiltInInternalFieldNames.FileRef);
                            values.Add(BuiltInInternalFieldNames.FileType);
                            values.Add(BuiltInInternalFieldNames.File_x0020_Size);
                            values.Add(BuiltInInternalFieldNames.FirstName);

                            if (prop.PropertyType == typeof(string))
                                newValue = values[RegressionService.RndService.Int(values.Count - 1)];

                            if (prop.PropertyType == typeof(Collection<string>))
                            {
                                var result = new Collection<string>();
                                var resultLength = RegressionService.RndService.Int(values.Count - 1);

                                for (var index = 0; index < resultLength; index++)
                                    result.Add(values[index]);

                                newValue = result;
                            }
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsBasePermission) > 0)
                        {
                            var values = new List<string>();

                            values.Add(BuiltInBasePermissions.AddAndCustomizePages);
                            values.Add(BuiltInBasePermissions.AnonymousSearchAccessWebLists);
                            values.Add(BuiltInBasePermissions.ApproveItems);
                            values.Add(BuiltInBasePermissions.CancelCheckout);
                            values.Add(BuiltInBasePermissions.CreateSSCSite);
                            values.Add(BuiltInBasePermissions.EditMyUserInfo);

                            if (prop.PropertyType == typeof(string))
                                newValue = values[RegressionService.RndService.Int(values.Count - 1)];

                            if (prop.PropertyType == typeof(Collection<string>))
                            {
                                var result = new Collection<string>();
                                var resultLength = RegressionService.RndService.Int(values.Count - 1);

                                for (var index = 0; index < resultLength; index++)
                                    result.Add(values[index]);

                                newValue = result;
                            }
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsPublishingPageContentType) > 0)
                        {
                            var values = new List<string>();

                            values.Add(BuiltInPublishingContentTypeId.ArticlePage);
                            values.Add(BuiltInPublishingContentTypeId.EnterpriseWikiPage);
                            values.Add(BuiltInPublishingContentTypeId.ErrorPage);
                            values.Add(BuiltInPublishingContentTypeId.RedirectPage);

                            if (prop.PropertyType == typeof(string))
                                newValue = values[RegressionService.RndService.Int(values.Count - 1)];

                            if (prop.PropertyType == typeof(Collection<string>))
                            {
                                var result = new Collection<string>();
                                var resultLength = RegressionService.RndService.Int(values.Count - 1);

                                for (var index = 0; index < resultLength; index++)
                                    result.Add(values[index]);

                                newValue = result;
                            }
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsUIVersion) > 0)
                        {
                            var values = new List<string>();

                            values.Add("4");
                            values.Add("15");

                            if (prop.PropertyType == typeof(string))
                                newValue = values[RegressionService.RndService.Int(values.Count - 1)];

                            if (prop.PropertyType == typeof(Collection<string>))
                            {
                                var result = new Collection<string>();
                                var resultLength = RegressionService.RndService.Int(values.Count - 1);

                                for (var index = 0; index < resultLength; index++)
                                    result.Add(values[index]);

                                newValue = result;
                            }

                            if (prop.PropertyType == typeof(List<string>))
                            {
                                var result = new List<string>();
                                var resultLength = RegressionService.RndService.Int(values.Count - 1);

                                for (var index = 0; index < resultLength; index++)
                                    result.Add(values[index]);

                                newValue = result;
                            }
                        }
                        else
                        {
                            // all this needs to be refactored, we know

                            if (prop.PropertyType == typeof(byte[]))
                            {
                                if (def is WebPartGalleryFileDefinition
                                    && prop.Name == "Content")
                                {
                                    // change web part
                                    var webPartXmlString = Encoding.UTF8.GetString(prop.GetValue(def) as byte[]);
                                    var webPartXml = WebpartXmlExtensions.LoadWebpartXmlDocument(webPartXmlString);

                                    webPartXml.SetTitleUrl(RegressionService.RndService.HttpUrl());

                                    newValue = Encoding.UTF8.GetBytes(webPartXml.ToString());
                                }
                                else
                                {
                                    newValue = RegressionService.RndService.Content();
                                }
                            }
                            else if (prop.PropertyType == typeof(string))
                                newValue = RegressionService.RndService.String();
                            else if (prop.PropertyType == typeof(bool))
                                newValue = RegressionService.RndService.Bool();
                            else if (prop.PropertyType == typeof(bool?))
                            {
                                var oldValue = prop.GetValue(def) as bool?;

                                if (oldValue == null || !oldValue.HasValue)
                                {
                                    newValue = RegressionService.RndService.Bool();
                                }
                                else
                                {
                                    newValue = !oldValue.Value;
                                }
                            }
                            else if (prop.PropertyType == typeof(int))
                                newValue = RegressionService.RndService.Int();
                            else if (prop.PropertyType == typeof(int?))
                                newValue = RegressionService.RndService.Bool()
                                    ? (int?)null
                                    : RegressionService.RndService.Int();
                            else if (prop.PropertyType == typeof(uint))
                                newValue = (uint)RegressionService.RndService.Int();
                            else if (prop.PropertyType == typeof(uint?))
                                newValue =
                                    (uint?)
                                        (RegressionService.RndService.Bool()
                                            ? (uint?)null
                                            : (uint?)RegressionService.RndService.Int());
                            else if (prop.PropertyType == typeof(Collection<string>))
                            {
                                var resultLength = RegressionService.RndService.Int(10);
                                var values = new List<string>();

                                var result = new Collection<string>();

                                for (var index = 0; index < resultLength; index++)
                                    result.Add(RegressionService.RndService.String());

                                newValue = result;
                            }
                            else if (prop.PropertyType == typeof(List<string>))
                            {
                                var resultLength = RegressionService.RndService.Int(10);
                                var values = new List<string>();

                                var result = new List<string>();

                                for (var index = 0; index < resultLength; index++)
                                    result.Add(RegressionService.RndService.String());

                                newValue = result;
                            }


                            else if (prop.PropertyType == typeof(double?)
                                     || prop.PropertyType == typeof(double))
                            {
                                newValue = (double)RegressionService.RndService.Int();
                            }
                            else
                            {
                                throw new NotImplementedException(
                                    string.Format("Update validation for type: [{0}] is not supported yet",
                                        prop.PropertyType));
                            }
                        }

                        trace.WriteLine(string.Format("[INF]\t\tChanging property [{0}] from [{1}] to [{2}]", prop.Name, prop.GetValue(def), newValue));
                        prop.SetValue(def, newValue);
                    }
                }

                trace.WriteLine("");
            });
        }

        #endregion
    }
}

