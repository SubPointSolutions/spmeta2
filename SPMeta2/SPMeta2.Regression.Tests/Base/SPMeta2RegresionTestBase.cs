using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Standard.DefinitionGenerators;
using SPMeta2.Containers.Utils;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Regression.Assertion;
using SPMeta2.Regression.Exceptions;
using SPMeta2.Regression.Runners;
using SPMeta2.Regression.Runners.Consts;
using SPMeta2.Regression.Services;
using SPMeta2.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Validation.Services;

namespace SPMeta2.Regression.Tests.Base
{
    public class SPMeta2RegresionEventsTestBase
    {
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

        static SPMeta2RegresionEventsTestBase()
        {
            RegressionService = new RegressionTestService();

            RegressionService.EnableDefinitionValidation = true;
            RegressionService.ModelGeneratorService.RegisterDefinitionGenerators(typeof(ImageRenditionDefinitionGenerator).Assembly);
        }

        #endregion

        #region properties

        public static RegressionTestService RegressionService { get; set; }

        #endregion

        #region properties

        public ModelGeneratorService ModelGeneratorService
        {
            get { return RegressionService.ModelGeneratorService; }
        }

        protected void TestRandomDefinition<TDefinition>()
           where TDefinition : DefinitionBase, new()
        {
            TestRandomDefinition<TDefinition>(null);
        }

        protected void TestRandomDefinition<TDefinition>(Action<TDefinition> definitionSetup)
            where TDefinition : DefinitionBase, new()
        {
            RegressionService.TestRandomDefinition(definitionSetup);
        }

        protected void WithExcpectedCSOMnO365RunnerExceptions(Action action)
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

        }

        protected void TestModels(IEnumerable<ModelNode> models)
        {
            RegressionService.TestModels(models);
        }

        #endregion
    }
}
