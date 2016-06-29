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
using SPMeta2.Containers.Services.Rnd;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.ModelHandlers;
using SPMeta2.Regression.Tests.Impl.Scenarios.Webparts;
using SPMeta2.Services;

namespace SPMeta2.Regression.Tests.Base
{
    public class SPMeta2RegresionTestCoreBase
    {
        public SPMeta2RegresionTestCoreBase()
        {
            Rnd = new DefaultRandomService();

            ModelGeneratorService = new ModelGeneratorService();
            ModelGeneratorService.RegisterDefinitionGenerators(typeof(ImageRenditionDefinitionGenerator).Assembly);
        }
        protected RandomService Rnd { get; set; }

        protected virtual bool IsCorrectValidationException(Exception e)
        {
            var result = true;

            result = result & (e is SPMeta2Exception);
            result = result & (e.InnerException is SPMeta2AggregateException);
            result = result & ((e.InnerException as AggregateException)
                                    .InnerExceptions.All(ee => ee is SPMeta2ModelValidationException));

            return result;
        }

        public ModelGeneratorService ModelGeneratorService { get; set; }

        protected virtual T RndDef<T>()
            where T : DefinitionBase
        {
            return RndDef<T>(null);
        }

        protected virtual T RndDef<T>(Action<T> action)
            where T : DefinitionBase
        {
            return ModelGeneratorService.GetRandomDefinition<T>(action);
        }
    }

    public class SPMeta2DefinitionRegresionTestBase : SPMeta2RegresionTestCoreBase
    {

    }

    public class SPMeta2ProvisionRegresionTestBase : SPMeta2RegresionTestCoreBase
    {
        #region constructors

        public SPMeta2ProvisionRegresionTestBase()
        {
            ModelServiceBase.OnResolveNullModelHandler = (node => new EmptyModelhandler());

            RegressionService.EnableDefinitionProvision = true;
            RegressionService.ProvisionGenerationCount = 2;

            RegressionService.EnableDefinitionValidation = true;

            RegressionService.ShowOnlyFalseResults = true;

            EnablePropertyUpdateValidation = false;
            PropertyUpdateGenerationCount = 2;

            TestOptions = new RunOptions();

            TestOptions.EnableWebApplicationDefinitionTest = false;
            TestOptions.EnableSerializeDeserializeAndStillDeployTests = false;

            TestOptions.EnableContentTypeHubTests = true;

            TestOptions.EnablWebConfigModificationTest = false;
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

        static SPMeta2ProvisionRegresionTestBase()
        {
            // ensure we aren't in GAC
            var location = typeof(FieldDefinition).Assembly.Location;

            if (location.ToUpper().Contains("GAC_"))
            {
                throw new SPMeta2Exception(string.Format("M2 assemblies are beinfg loaded from the GAC: [{0}].", location));
            }


            // moved to RegressionService lazy load initialization
            //RegressionService = new RegressionTestService();

            //RegressionService.AssertService = new VSAssertService();

            //RegressionService.EnableDefinitionValidation = true;
            //RegressionService.ModelGeneratorService.RegisterDefinitionGenerators(typeof(ImageRenditionDefinitionGenerator).Assembly);
        }

        #endregion

        #region classes

        protected class RunOptions
        {
            public bool EnableWebApplicationDefinitionTest { get; set; }
            public bool EnableSerializeDeserializeAndStillDeployTests { get; set; }

            public bool EnablWebConfigModificationTest { get; set; }

            public bool EnableContentTypeHubTests { get; set; }
        }

        #endregion

        #region properties


        public bool EnablePropertyNullableValidation { get; set; }
        public int PropertyNullableGenerationCount { get; set; }

        protected RunOptions TestOptions { get; set; }
        public bool EnablePropertyUpdateValidation { get; set; }

        private static RegressionTestService _regressionService;

        public static RegressionTestService RegressionService
        {
            get
            {
                if (_regressionService == null)
                {
                    _regressionService = new RegressionTestService();
                    _regressionService.AssertService = new VSAssertService();

                    _regressionService.EnableDefinitionValidation = true;
                    _regressionService.ModelGeneratorService.RegisterDefinitionGenerators(typeof(ImageRenditionDefinitionGenerator).Assembly);
                }

                return _regressionService;
            }
            set
            {
                _regressionService = value;
            }
        }

        //public ModelGeneratorService ModelGeneratorService
        //{
        //    get { return RegressionService.ModelGeneratorService; }
        //}

        #endregion

        #region testing API

        protected virtual void WithDisabledValidationOnTypes(Type type, Action action)
        {
            WithDisabledValidationOnTypes(new[] { type }, action);
        }

        protected virtual void WithDisabledValidationOnTypes(IEnumerable<Type> types, Action action)
        {
            try
            {
                RegressionService.RegExcludedDefinitionTypes.Add(typeof(WebDefinition));
                action();
            }
            finally
            {
                RegressionService.RegExcludedDefinitionTypes.Clear();
            }
        }


        protected void WithDisabledDefinitionImmutabilityValidation(Action action)
        {
            var _oldValue = RegressionService.EnableDefinitionImmutabilityValidation;


            try
            {
                RegressionService.EnableDefinitionImmutabilityValidation = false;
                action();
            }
            finally
            {
                RegressionService.EnableDefinitionImmutabilityValidation = _oldValue;
            }
        }

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


            PleaseMakeSureWeCanSerializeDeserializeAndStillDeploy(new[] { model });
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
            TestModels(new ModelNode[] { model });
        }

        protected void TestModel(ModelNode firstModel, ModelNode secondModel)
        {
            TestModels(new ModelNode[] { firstModel, secondModel });
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
            PleaseMakeSureWeCanSerializeDeserializeAndStillDeploy(models);
        }

        private void PleaseMakeSureWeCanSerializeDeserializeAndStillDeploy(IEnumerable<ModelNode> models)
        {
            if (!TestOptions.EnableSerializeDeserializeAndStillDeployTests)
                return;

            TraceUtils.WithScope(trace =>
            {
                trace.WriteLine("Saving-restoring XML/JSON models. Deployng..");
                var serializedModels = RegressionService.GetSerializedAndRestoredModels(models);

                RegressionService.TestModels(serializedModels);
            });
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
                        var expectUpdateAttr = prop.GetCustomAttributes(typeof(ExpectUpdate), true)
                                                   .FirstOrDefault() as ExpectUpdate;


                        newValue = GetNewPropValue(expectUpdateAttr, def, prop);

                        trace.WriteLine(string.Format("[INF]\t\tChanging property [{0}] from [{1}] to [{2}]", prop.Name, prop.GetValue(def), newValue));
                        prop.SetValue(def, newValue);
                    }
                }

                trace.WriteLine("");
            });
        }

        private static object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            var expectUpdateServices = new List<ExpectUpdateValueServiceBase>();
            expectUpdateServices.AddRange(ReflectionUtils.GetTypesFromAssembly<ExpectUpdateValueServiceBase>(typeof(ExpectUpdateValueServiceBase).Assembly)
                                                         .Select(t => Activator.CreateInstance(t) as ExpectUpdateValueServiceBase));

            var targetServices = expectUpdateServices.FirstOrDefault(s => s.TargetType == attr.GetType());

            if (targetServices == null)
                throw new SPMeta2NotImplementedException(string.Format("Can't find ExpectUpdateValueServiceBase impl for type: [{0}]", attr.GetType()));

            return targetServices.GetNewPropValue(attr, obj, prop);
        }

        #endregion
    }
}

