using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using SPMeta2.Validation.Services;

namespace SPMeta2.Regression.Tests.Base
{
    public class SPMeta2RegresionTestBase
    {
        #region constructors

        public SPMeta2RegresionTestBase()
        {
            RegressionService.EnableDefinitionProvision = true;
            RegressionService.EnableDefinitionValidation = true;

            //EnablePropertyUpdateValidation = false;
        }

        #endregion

        #region static

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

            if (EnablePropertyUpdateValidation)
            {
                ProcessPropertyUpdateValidation(new[] { model });
                RegressionService.TestModels(new[] { model });
            }
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

        protected void TestModels(IEnumerable<ModelNode> models)
        {
            RegressionService.TestModels(models);

            if (EnablePropertyUpdateValidation)
            {
                ProcessPropertyUpdateValidation(models);
                RegressionService.TestModels(models);
            }
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

                        if (prop.PropertyType == typeof(string))
                            newValue = RegressionService.RndService.String();
                        else if (prop.PropertyType == typeof(bool))
                            newValue = RegressionService.RndService.Bool();
                        else if (prop.PropertyType == typeof(bool?))
                            newValue = RegressionService.RndService.Bool() ? (bool?)null : RegressionService.RndService.Bool();
                        else if (prop.PropertyType == typeof(int))
                            newValue = RegressionService.RndService.Int();
                        else if (prop.PropertyType == typeof(int?))
                            newValue = RegressionService.RndService.Bool() ? (int?)null : RegressionService.RndService.Int();
                        else if (prop.PropertyType == typeof(uint))
                            newValue = (uint)RegressionService.RndService.Int();
                        else if (prop.PropertyType == typeof(uint?))
                            newValue = (uint?)(RegressionService.RndService.Bool() ? (uint?)null : (uint?)RegressionService.RndService.Int());
                        else
                        {
                            throw new NotImplementedException(string.Format("Update validation for type: [{0}] is not supported yet", prop.PropertyType));
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

