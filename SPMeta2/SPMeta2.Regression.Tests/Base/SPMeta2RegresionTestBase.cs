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

namespace SPMeta2.Regression.Tests.Base
{
    public class SPMeta2RegresionTestBase
    {
        #region constructors

        public SPMeta2RegresionTestBase()
        {
            RegressionService.EnableDefinitionProvision = true;
            RegressionService.EnableDefinitionValidation = true;

            RegressionService.ShowOnlyFalseResults = true;

            EnablePropertyUpdateValidation = true;
            PropertyUpdateGenerationCount = 2;
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

        #region properties

        public bool EnablePropertyUpdateValidation { get; set; }

        public static RegressionTestService RegressionService { get; set; }

        public ModelGeneratorService ModelGeneratorService
        {
            get { return RegressionService.ModelGeneratorService; }
        }

        #endregion

        #region testing API

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
                        else if (attrs.Count(a => a is ExpectUpdateAsBasePermission) > 0)
                        {
                            var values = new List<string>();

                            values.Add(BuiltInBasePermissions.AddListItems);
                            values.Add(BuiltInBasePermissions.ApproveItems);
                            values.Add(BuiltInBasePermissions.CancelCheckout);
                            values.Add(BuiltInBasePermissions.CreateGroups);
                            values.Add(BuiltInBasePermissions.DeleteListItems);
                            values.Add(BuiltInBasePermissions.EditListItems);
                            values.Add(BuiltInBasePermissions.EditMyUserInfo);
                            values.Add(BuiltInBasePermissions.EnumeratePermissions);
                            values.Add(BuiltInBasePermissions.ManagePersonalViews);
                            values.Add(BuiltInBasePermissions.ManageLists);

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
                        else if (attrs.Count(a => a is ExpectUpdateAsCamlQuery) > 0)
                        {
                            newValue = string.Format("<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where>",
                                    RegressionService.RndService.String());
                        }
                        else if (attrs.Count(a => a is ExpectUpdateAsFileName) > 0)
                        {
                            var attr = attrs.FirstOrDefault(a => a is ExpectUpdateAsFileName) as ExpectUpdateAsFileName;
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
                            if (prop.PropertyType == typeof(string))
                                newValue = RegressionService.RndService.String();
                            else if (prop.PropertyType == typeof(bool))
                                newValue = RegressionService.RndService.Bool();
                            else if (prop.PropertyType == typeof(bool?))
                                newValue = RegressionService.RndService.Bool()
                                    ? (bool?)null
                                    : RegressionService.RndService.Bool();
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

