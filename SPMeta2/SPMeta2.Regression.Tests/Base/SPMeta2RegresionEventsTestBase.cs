using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Containers.Standard.DefinitionGenerators;
using SPMeta2.Models;
using SPMeta2.Regression.Utils;
using SPMeta2.Regression.Tests.Common;
using SPMeta2.Regression.Tests.Services;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Definitions;
using System.Reflection;
using System.Diagnostics;
using SPMeta2.Regression.Exceptions;
using SPMeta2.Attributes;
using SPMeta2.Regression.Runners;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Assertion;
using SPMeta2.Utils;
using SPMeta2.Validation.Services;

namespace SPMeta2.Regression.Tests.Base
{
    public class SPMeta2RegresionEventsTestBase : SPMeta2RegresionTestBase
    {
        #region constructors

        public SPMeta2RegresionEventsTestBase()
        {
            InitModelGeneratorService();

            //EnableDefinitionValidation = false;
        }

        private void InitModelGeneratorService()
        {
            ModelGeneratorService = new ModelGeneratorService();

            ModelGeneratorService.RegisterDefinitionGenerators(typeof(WebNavigationSettingsDefinitionGenerator).Assembly);
        }


        #endregion

        #region properties

        protected ModelGeneratorService ModelGeneratorService { get; set; }

        #endregion

        internal class ModelValidationResult
        {
            public ModelValidationResult()
            {
                Properties = new List<PropertyValidationResult>();
            }

            public DefinitionBase Model { get; set; }
            public List<PropertyValidationResult> Properties { get; set; }
        }


        protected static void InternalInit()
        {
            RegressionAssertService.OnPropertyValidated += OnModelPropertyValidated;
        }

        protected static void InternalCleanup()
        {

        }

        private static List<ModelValidationResult> ModelValidations = new List<ModelValidationResult>();

        protected static void OnModelPropertyValidated(object sender, OnPropertyValidatedEventArgs e)
        {
            var existingModelResult = ModelValidations.FirstOrDefault(r => r.Model == e.Result.Tag);

            if (existingModelResult == null)
            {
                existingModelResult = new ModelValidationResult
                {
                    Model = e.Result.Tag as DefinitionBase
                };

                ModelValidations.Add(existingModelResult);
            }

            existingModelResult.Properties.Add(e.Result);
        }

        protected static void WithExcpectedCSOMnO365RunnerExceptions(Action action)
        {
            WithExcpectedExceptions(new Type[] {
                typeof(SPMeta2UnsupportedCSOMRunnerException),
                typeof(SPMeta2UnsupportedO365RunnerException)
            }, action);
        }

        protected static void WithExcpectedExceptions(IEnumerable<Type> exceptionTypes, Action action)
        {
            try
            {
                if (action != null)
                    action();
            }
            catch (Exception e)
            {
                var isAllowedException = false;

                foreach (var allowedType in exceptionTypes)
                    if (e.GetType().IsAssignableFrom(allowedType))
                        isAllowedException = true;

                if (isAllowedException)
                {
                    // TODO, Trace utils to report
                }
                else
                {
                    // TOOD, trace utils to report

                    throw;
                }

            }
        }

        protected virtual void AssertEvenktHooks<TObj>(ModelNode modelNode, EventHooks hooks)
        {
            modelNode.OnProvisioning<TObj>(context =>
            {
                hooks.OnProvisioning = true;

                Assert.IsNotNull(context.ObjectDefinition);
            });

            modelNode.OnProvisioned<TObj>(context =>
            {
                hooks.OnProvisioned = true;

                Assert.IsNotNull(context.Object);
                Assert.IsNotNull(context.ObjectDefinition);

                Assert.IsInstanceOfType(context.Object, typeof(TObj));
            });
        }

        protected EventHooks CreateHook()
        {
            return new EventHooks();
        }

        protected void ResolveHook(EventHooks eventHooks, string start)
        {
            TraceUtils.WithScope(traceScope =>
            {
                if (eventHooks.OnProvisioning)
                    traceScope.WriteLine(string.Format("[INF]{0} [VALIDATED] - [OnProvisioning]", start));
                else
                    traceScope.WriteLine(string.Format("[ERR]{0} [MISSED] - [OnProvisioning]", start));

                if (eventHooks.OnProvisioned)
                    traceScope.WriteLine(string.Format("[INF]{0} [VALIDATED] - [OnProvisioned]", start));
                else
                    traceScope.WriteLine(string.Format("[IERR]{0} [MISSED] - [OnProvisioned]", start));

                Assert.AreEqual(true, eventHooks.OnProvisioning);
                Assert.AreEqual(true, eventHooks.OnProvisioned);
            });
        }

        protected virtual void WithEventHooks(Action<EventHooks> hooks)
        {
            TraceUtils.WithScope(traceScope =>
            {
                traceScope.WriteLine(string.Format("Validating OnProvisioning/OnProvisioned events."));

                var eventHooks = new EventHooks();

                hooks(eventHooks);

                traceScope.WriteLine(string.Format("Validating OnProvisioning event hit."));
                Assert.AreEqual(true, eventHooks.OnProvisioning);
                traceScope.WriteLine(string.Format("    - done"));

                traceScope.WriteLine(string.Format("Validating OnProvisioned event hit."));
                Assert.AreEqual(true, eventHooks.OnProvisioned);
                traceScope.WriteLine(string.Format("    - done"));
            });
        }

        #region utils

        protected bool HasTestMethod(string methodPrefix, Type definition, MethodInfo[] methods)
        {
            var methodName = string.Format("{0}{1}", methodPrefix, definition.Name);

            Trace.WriteLine(string.Format("Asserting method:[{0}]", methodName));

            var targetMethod = methods.FirstOrDefault(m => m.Name == methodName);

            return targetMethod != null;
        }



        private SPObjectModelType GetRunnerType(ProvisionRunnerBase runner)
        {
            if (runner.Name == "SSOM")
                return SPObjectModelType.SSOM;

            if (runner.Name == "O365" || runner.Name == "CSOM")
                return SPObjectModelType.CSOM;

            throw new SPMeta2NotSupportedException(string.Format("Cannot find SPObjectModelType type for runer of type:[{0}]", runner.Name));
        }

        protected void TestRandomDefinition<TDefinition>()
           where TDefinition : DefinitionBase, new()
        {
            TestRandomDefinition<TDefinition>(null);
        }



        protected void TestRandomDefinition<TDefinition>(Action<TDefinition> definitionSetup)
            where TDefinition : DefinitionBase, new()
        {
            var allHooks = new List<EventHooks>();

            WithProvisionRunnerContext(runnerContext =>
            {
                var runner = runnerContext.Runner;

                ValidateDefinitionHostRunnerSupport<TDefinition>(runner);

                var omModelType = GetRunnerType(runner);

                var definitionSandbox = ModelGeneratorService.GenerateModelTreeForDefinition<TDefinition>(omModelType);
                var additionalDefinitions = ModelGeneratorService.GetAdditionalDefinition<TDefinition>();

                ModelGeneratorService.ComposeModelWithAdditionalDefinitions(definitionSandbox, additionalDefinitions, omModelType);

                if (definitionSetup != null)
                    definitionSetup(ModelGeneratorService.CurrentDefinition as TDefinition);

                var hooks = GetHooks(definitionSandbox);

                foreach (var hook in hooks)
                    hook.Tag = runner.Name;

                allHooks.AddRange(hooks);

                if (definitionSandbox.Value.GetType() == typeof(FarmDefinition))
                    runner.DeployFarmModel(definitionSandbox);

                if (definitionSandbox.Value.GetType() == typeof(WebApplicationDefinition))
                    runner.DeployWebApplicationModel(definitionSandbox);

                if (definitionSandbox.Value.GetType() == typeof(SiteDefinition))
                    runner.DeploySiteModel(definitionSandbox);

                if (definitionSandbox.Value.GetType() == typeof(WebDefinition))
                    runner.DeployWebModel(definitionSandbox);

                var hasMissedOrInvalidProps = ResolveModelValidation(definitionSandbox, hooks);

                Assert.IsFalse(hasMissedOrInvalidProps);
            });

        }

        protected void TestModel(ModelNode model)
        {
            var allHooks = new List<EventHooks>();

            WithProvisionRunnerContext(runnerContext =>
            {
                var runner = runnerContext.Runner;

                //ValidateDefinitionHostRunnerSupport<TDefinition>(runner);

                var omModelType = GetRunnerType(runner);
                var hooks = GetHooks(model);

                foreach (var hook in hooks)
                    hook.Tag = runner.Name;

                allHooks.AddRange(hooks);

                if (model.Value.GetType() == typeof(FarmDefinition))
                    runner.DeployFarmModel(model);

                if (model.Value.GetType() == typeof(WebApplicationDefinition))
                    runner.DeployWebApplicationModel(model);

                if (model.Value.GetType() == typeof(SiteDefinition))
                    runner.DeploySiteModel(model);

                if (model.Value.GetType() == typeof(WebDefinition))
                    runner.DeployWebModel(model);

                var hasMissedOrInvalidProps = ResolveModelValidation(model, hooks);
                Assert.IsFalse(hasMissedOrInvalidProps);
            });
        }

        private bool ResolveModelValidation(ModelNode modelNode, List<EventHooks> hooks)
        {
            return ResolveModelValidation(modelNode, "     ", hooks);
        }

        private bool ResolveModelValidation(ModelNode modelNode, string start, List<EventHooks> hooks)
        {
            Trace.WriteLine(string.Format(""));

            var hasMissedOrInvalidProps = false;

            var model = modelNode.Value;
            Trace.WriteLine(string.Format("[INF]{2}MODEL CHECK [{0}] - ( {1} )", model.GetType(), model.ToString(), start));

            if (model.RequireSelfProcessing || modelNode.Options.RequireSelfProcessing)
            {
                var modelValidationResult = ModelValidations.FirstOrDefault(r => r.Model == model);

                var shouldBeValidatedProperties = model.GetType()
                                                       .GetProperties()
                                                       .Where(p => p.GetCustomAttributes<SPMeta2.Attributes.Regression.ExpectValidationAttribute>().Count() > 0)
                                                       .ToList();


                if (modelValidationResult == null)
                {
                    Trace.WriteLine(string.Format("[ERR]{2} Missing validation for model [{0}] - ( {1} )", model.GetType(), model.ToString(), start));

                    hasMissedOrInvalidProps = true;
                    return hasMissedOrInvalidProps;
                }

                foreach (var property in modelValidationResult.Properties)
                {
                    Trace.WriteLine(string.Format("[INF]{6} [{4}] - Src prop: [{0}] Src value: [{1}] Dst prop: [{2}] Dst value: [{3}] Message:[{5}]",
                        new object[]{
                            property.Src != null ? property.Src.Name : string.Empty,
                            property.Src != null ? property.Src.Value : string.Empty,

                            property.Dst != null ? property.Dst.Name : string.Empty,
                            property.Dst != null ? property.Dst.Value : string.Empty,

                            property.IsValid,
                            property.Message,
                            start
                    }));

                    if (!property.IsValid)
                        hasMissedOrInvalidProps = true;
                }

                Trace.WriteLine(string.Format("[INF]{0}PROPERTY CHECK", start));

                foreach (var shouldBeValidatedProp in shouldBeValidatedProperties)
                {
                    var hasValidation = false;
                    var validationResult = modelValidationResult.Properties.FirstOrDefault(r => r.Src != null && r.Src.Name == shouldBeValidatedProp.Name);

                    if (validationResult != null)
                    {
                        hasValidation = true;
                    }
                    else
                    {
                        hasMissedOrInvalidProps = true;
                        hasValidation = false;
                    }

                    if (hasValidation)
                    {
                        Trace.WriteLine(string.Format("[INF]{2} [{0}] - [{1}]",
                            "VALIDATED",
                            shouldBeValidatedProp.Name,
                            start));
                    }
                    else
                    {
                        Trace.WriteLine(string.Format("[ERR]{2} [{0}] - [{1}]",
                          "MISSED",
                          shouldBeValidatedProp.Name,
                          start));
                    }
                }


                Trace.WriteLine(string.Format("[INF]{0}EVENT CHECK", start));

                var hook = hooks.FirstOrDefault(h => h.ModelNode == modelNode);

                if (hook != null)
                {
                    ResolveHook(hook, start);
                }
                else
                {
                    Trace.WriteLine(string.Format("[ERR]{2} Missing hook validation for model [{0}] - ( {1} )", model.GetType(), model.ToString(), start));
                }
            }
            else
            {
                Trace.WriteLine(string.Format("[INF]{0} Skipping due RequireSelfProcessing ==  FALSE", start));
            }

            foreach (var childModel in modelNode.ChildModels)
            {
                var tmp = ResolveModelValidation(childModel, start + start, hooks);

                if (tmp == true)
                    hasMissedOrInvalidProps = true;
            }

            return hasMissedOrInvalidProps;
        }

        private void ValidateDefinitionHostRunnerSupport<T1>(Runners.ProvisionRunnerBase runner)
        {
            var attrs = typeof(T1).GetCustomAttributes()
                                             .OfType<SPObjectTypeAttribute>();

            if (CurrentProvisionRunner.Name == "SSOM")
            {
                var att = attrs.FirstOrDefault(a => a.ObjectModelType == SPObjectModelType.SSOM);

                if (att == null)
                    throw new SPMeta2UnsupportedCSOMRunnerException();

            }
            if (CurrentProvisionRunner.Name == "CSOM")
            {
                var att = attrs.FirstOrDefault(a => a.ObjectModelType == SPObjectModelType.CSOM);

                if (att == null)
                    throw new SPMeta2UnsupportedCSOMRunnerException();
            }
            if (CurrentProvisionRunner.Name == "O365")
            {
                var att = attrs.FirstOrDefault(a => a.ObjectModelType == SPObjectModelType.CSOM);

                if (att == null)
                    throw new SPMeta2UnsupportedO365RunnerException();
            }
        }

        private List<EventHooks> GetHooks(ModelNode modeNode)
        {
            var reult = new List<EventHooks>();
            AttachHooks(modeNode, reult);

            return reult;
        }

        private void AttachHooks(ModelNode modeNode, List<EventHooks> hooks)
        {
            if (modeNode.Value.RequireSelfProcessing || modeNode.Options.RequireSelfProcessing)
            {
                var hook = CreateHook();

                hook.ModelNode = modeNode;

                hooks.Add(hook);

                var typeName = ResolveSPType(modeNode);
                AssertEventHooksByStringType(typeName, modeNode, hook);
            }

            foreach (var childModel in modeNode.ChildModels)
            {
                AttachHooks(childModel, hooks);
            }
        }

        private string ResolveSPType(ModelNode field)
        {
            var attrs = field.Value.GetType().GetCustomAttributes()
                                             .OfType<SPObjectTypeAttribute>();

            var className = string.Empty;
            var classAssembly = string.Empty;

            if (CurrentProvisionRunner.Name == "SSOM")
            {
                var att = attrs.FirstOrDefault(a => a.ObjectModelType == SPObjectModelType.SSOM);

                className = att.ClassName;
                classAssembly = att.AssemblyName;
            }
            if (CurrentProvisionRunner.Name == "CSOM")
            {
                var att = attrs.FirstOrDefault(a => a.ObjectModelType == SPObjectModelType.CSOM);

                className = att.ClassName;
                classAssembly = att.AssemblyName;
            }
            if (CurrentProvisionRunner.Name == "O365")
            {
                var att = attrs.FirstOrDefault(a => a.ObjectModelType == SPObjectModelType.CSOM);

                className = att.ClassName;
                classAssembly = att.AssemblyName;
            }

            var typeName = CurrentProvisionRunner.ResolveFullTypeName(className, classAssembly);
            return typeName;
        }

        #region hooks

        protected virtual Action<OnCreatingContext<TObjectType, TDefinitionType>> CreateOnProvisioningHook<TObjectType, TDefinitionType>()
          where TDefinitionType : DefinitionBase
        {
            return new Action<OnCreatingContext<TObjectType, TDefinitionType>>(context =>
            {
                var id = context.GetType().GetGenericArguments()[0].AssemblyQualifiedName + ":" + context.ObjectDefinition.GetHashCode();
                var hook = _hookMap[id];

                hook.OnProvisioning = true;

                Assert.IsNotNull(context.ObjectDefinition);
            });
        }

        protected virtual Action<OnCreatingContext<TObjectType, TDefinitionType>> CreateOnProvisionedHook<TObjectType, TDefinitionType>()
            where TDefinitionType : DefinitionBase
        {
            return new Action<OnCreatingContext<TObjectType, TDefinitionType>>(context =>
            {
                var id = context.GetType().GetGenericArguments()[0].AssemblyQualifiedName + ":" + context.ObjectDefinition.GetHashCode();
                var hook = _hookMap[id];

                hook.OnProvisioned = true;

                Assert.IsNotNull(context.Object);
                Assert.IsNotNull(context.ObjectDefinition);

                Assert.IsInstanceOfType(context.Object, typeof(TObjectType));
            });
        }

        //private static EventHooks CurrentHook;

        private static Dictionary<string, EventHooks> _hookMap = new Dictionary<string, EventHooks>();

        public void AddHook(string id, EventHooks hooks)
        {
            if (!_hookMap.ContainsKey(id))
                _hookMap.Add(id, hooks);
        }

        protected virtual void AssertEventHooksByStringType(string spType, ModelNode modelNode, EventHooks hooks)
        {


            var spObjectType = Type.GetType(spType);
            var modelContextType = typeof(OnCreatingContext<,>);

            AddHook(spObjectType.AssemblyQualifiedName + ":" + modelNode.Value.GetHashCode().ToString(), hooks);

            var nonDefinition = new Type[] { spObjectType, typeof(DefinitionBase) };
            var withDefinition = new Type[] { spObjectType, modelNode.Value.GetType() };

            var modelNonDefInstanceType = modelContextType.MakeGenericType(nonDefinition);
            var modelWithDefInstanceType = modelContextType.MakeGenericType(withDefinition);

            var genericAction = typeof(Action<>);

            HandlerOnProvisioningHook(modelNode, spObjectType, nonDefinition);
            HandlerOnProvisionedHook(modelNode, spObjectType, nonDefinition);
        }

        private void HandlerOnProvisionedHook(ModelNode modelNode, Type spObjectType, Type[] nonDefinition)
        {
            var onProvisioningCalls = typeof(ModernSyntax)
                                                   .GetMethods()
                                                   .Where(m => m.Name == "OnProvisioned")
                                                   .ToList();

            var defaultCall = onProvisioningCalls.FirstOrDefault(m => m.GetGenericArguments().Count() == 1);
            var typedCall = onProvisioningCalls.FirstOrDefault(m => m.GetGenericArguments().Count() == 2);

            var defaultCallInstance = defaultCall.MakeGenericMethod(new[] { spObjectType });

            var methodType = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(x => x.Name == "CreateOnProvisionedHook").MakeGenericMethod(nonDefinition);
            var method = methodType.Invoke(this, null);

            defaultCallInstance.Invoke(modelNode, new object[]{
                modelNode,
                method
            });
        }

        private void HandlerOnProvisioningHook(ModelNode modelNode, Type spObjectType, Type[] nonDefinition)
        {
            var onProvisioningCalls = typeof(ModernSyntax)
                                                    .GetMethods()
                                                    .Where(m => m.Name == "OnProvisioning")
                                                    .ToList();

            var defaultCall = onProvisioningCalls.FirstOrDefault(m => m.GetGenericArguments().Count() == 1);
            var typedCall = onProvisioningCalls.FirstOrDefault(m => m.GetGenericArguments().Count() == 2);

            var defaultCallInstance = defaultCall.MakeGenericMethod(new[] { spObjectType });

            var methodType = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(x => x.Name == "CreateOnProvisioningHook").MakeGenericMethod(nonDefinition);
            var method = methodType.Invoke(this, null);

            defaultCallInstance.Invoke(modelNode, new object[]{
                modelNode,
                method
            });
        }

        protected virtual void AssertEventHooks1<TObj>(ModelNode modelNode, EventHooks hooks)
        {
            modelNode.OnProvisioning<TObj>(context =>
            {
                hooks.OnProvisioning = true;

                Assert.IsNotNull(context.ObjectDefinition);
            });

            modelNode.OnProvisioned<TObj>(context =>
            {
                hooks.OnProvisioned = true;

                Assert.IsNotNull(context.Object);
                Assert.IsNotNull(context.ObjectDefinition);

                Assert.IsInstanceOfType(context.Object, typeof(TObj));
            });
        }


        #endregion

        #endregion
    }
}
