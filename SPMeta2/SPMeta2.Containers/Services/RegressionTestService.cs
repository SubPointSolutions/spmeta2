using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using SPMeta2.Attributes;
using SPMeta2.Containers.Assertion;
using SPMeta2.Containers.Common;
using SPMeta2.Containers.Consts;
using SPMeta2.Containers.Exceptions;
using SPMeta2.Containers.Extensions;
using SPMeta2.Containers.Services.Rnd;
using SPMeta2.Containers.Utils;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Extensions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Utils;
using SPMeta2.Syntax.Default;
using SPMeta2.Services;

namespace SPMeta2.Containers.Services
{
    public class RegressionTestService
    {
        public AssertServiceBase AssertService { get; set; }
        public RandomService RndService { get; set; }



        public RegressionTestService()
        {
            RegExcludedDefinitionTypes = new List<Type>();

            ModelGeneratorService = new ModelGeneratorService();
            //Assert = new AssertService();

            RndService = new DefaultRandomService();

            //RegressionService = new RegressionTestService();

            //ModelGeneratorService.RegisterDefinitionGenerators(typeof(WebNavigationSettingsDefinitionGenerator).Assembly);

            ProvisionRunnerAssemblies = new List<string>();
            ProvisionRunners = new List<ProvisionRunnerBase>();

            EnablePropertyValidation = true;
            EnableEventValidation = true;

            EnableDefinitionImmutabilityValidation = false;

            InitConfig();
        }

        #region properties

        protected void InitLazyRunnerConnection()
        {
            InitRunnerImplementations();

            foreach (var runner in ProvisionRunners)
                runner.InitLazyRunnerConnection();
        }

        protected void DisposeLazyRunnerConnection()
        {
            foreach (var runner in ProvisionRunners)
                runner.DisposeLazyRunnerConnection();
        }

        #endregion

        [Serializable]
        public class ContainerValidationResultException : SPMeta2Exception
        {
            public ContainerValidationResultException() { }
            public ContainerValidationResultException(string message) : base(message) { }
            public ContainerValidationResultException(string message, Exception inner) : base(message, inner) { }
            protected ContainerValidationResultException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }

            public DefinitionBase Definition { get; set; }

            public OnPropertyValidatedEventArgs Args { get; set; }
        }

        public void OnModelPropertyValidated(object sender, OnPropertyValidatedEventArgs e)
        {
            // immediate throw
            // temporary solution due to multiple provision of the same model

            //if (!e.Result.IsValid)
            //{
            //    throw new ContainerValidationResultException
            //    {
            //        Args = e,
            //        Definition = e.Result.Tag as DefinitionBase
            //    };
            //}

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

        protected virtual void InitConfig()
        {
            InitRunnerTypes();
            InitRunnerImplementations();
        }

        private bool _hasInit = false;

        public static string CurrentProvisionRunnerAsssmbly { get; set; }

        protected virtual void InitRunnerImplementations()
        {
            if (_hasInit) return;

            foreach (var asmFileName in ProvisionRunnerAssemblies)
            {
                CurrentProvisionRunnerAsssmbly = asmFileName;

                var asmImpl = Assembly.LoadFrom(asmFileName);

                var types = ReflectionUtils.GetTypesFromAssembly<ProvisionRunnerBase>(asmImpl);

                foreach (var type in types)
                {
                    var runnerImpl = Activator.CreateInstance(type) as ProvisionRunnerBase;

                    ProvisionRunners.Add(runnerImpl);
                }
            }

            _hasInit = true;
        }

        protected virtual void InitRunnerTypes()
        {
            var runnerLibraries = RunnerEnvironmentUtils.GetEnvironmentVariable(EnvironmentConsts.RunnerLibraries);

            ContainerTraceUtils.WriteLine(string.Format("Testing with runner libraries: [{0}]", runnerLibraries));

            if (!string.IsNullOrEmpty(runnerLibraries))
            {
                var libs = runnerLibraries.Split(',');

                foreach (var lib in libs)
                    ProvisionRunnerAssemblies.Add(lib);
            }

            if (ProvisionRunnerAssemblies.Count == 0)
                throw new ArgumentException("Cannot find any test runners. Please configure test runners via SPMeta2.Regression.Environment.ps1 script.");

            // Test runners should be managed via SPMeta2.Regression.Environment.ps1
            // Manual adding is for internal use only.

            //  ProvisionRunnerAssemblies.Add("SPMeta2.Regression.Runners.O365.dll");
            //  ProvisionRunnerAssemblies.Add("SPMeta2.Regression.Runners.CSOM.dll");

            //ProvisionRunnerAssemblies.Clear();
            //ProvisionRunnerAssemblies.Add("SPMeta2.Containers.SSOM.dll");
        }

        protected ProvisionRunnerBase CurrentProvisionRunner;

        #region methods

        public void WithExcpectedCSOMnO365RunnerExceptions(Action action)
        {
            WithExcpectedExceptions(new Type[] {
                typeof(SPMeta2UnsupportedCSOMRunnerException),
                typeof(SPMeta2UnsupportedO365RunnerException)
            }, action);
        }

        public void WithExcpectedExceptions(IEnumerable<Type> exceptionTypes, Action action)
        {
            try
            {
                if (action != null)
                    action();
            }
            catch (Exception e)
            {
                var targetExeption = e;
                var isAllowedException = false;

                if (e is SPMeta2ModelDeploymentException)
                {
                    targetExeption = e.InnerException;
                }

                foreach (var allowedType in exceptionTypes)
                {
                    //if (targetExeption.GetType().IsAssignableFrom(allowedType))
                    // we need a specific type matching to avoid missed excpetions

                    // SPMeta2.Exceptions.SPMeta2UnsupportedModelHostException: model host should be ListModelHost/WebModelHost/SiteModelHost on deploy a model to a SiteCollection using the Feature Receiver (SSOM) #1035
                    // https://github.com/SubPointSolutions/spmeta2/issues/1035
                    if (targetExeption.GetType() == allowedType)
                        isAllowedException = true;
                }

                if (isAllowedException)
                {
                    Trace.WriteLine
                        (string.Format("Handled expected exception:[{0}] - [{1}]",
                            targetExeption.GetType().Name, targetExeption));
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        public RegressionTestService RegressionService { get; set; }
        public ModelGeneratorService ModelGeneratorService { get; set; }

        public List<ProvisionRunnerBase> ProvisionRunners { get; set; }
        public List<string> ProvisionRunnerAssemblies { get; set; }

        #region tests

        private SPObjectModelType GetRunnerType(ProvisionRunnerBase runner)
        {
            if (runner.Name == "SSOM")
                return SPObjectModelType.SSOM;

            if (runner.Name == "O365" || runner.Name == "CSOM")
                return SPObjectModelType.CSOM;

            throw new SPMeta2NotSupportedException(string.Format("Cannot find SPObjectModelType type for runer of type:[{0}]", runner.Name));
        }

        private List<EventHooks> GetHooks(ModelNode modeNode)
        {
            var reult = new List<EventHooks>();
            AttachHooks(modeNode, reult);

            return reult;
        }

        protected EventHooks CreateHook()
        {
            return new EventHooks();
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

                if (att == null)
                    throw new SPMeta2UnsupportedCSOMRunnerException();

                className = att.ClassName;
                classAssembly = att.AssemblyName;
            }

            if (CurrentProvisionRunner.Name == "O365")
            {
                var att = attrs.FirstOrDefault(a => a.ObjectModelType == SPObjectModelType.CSOM);

                if (att == null)
                    throw new SPMeta2UnsupportedCSOMRunnerException();

                className = att.ClassName;
                classAssembly = att.AssemblyName;
            }

            var typeName = CurrentProvisionRunner.ResolveFullTypeName(className, classAssembly);
            return typeName;
        }

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

        protected virtual Action<OnCreatingContext<TObjectType, TDefinitionType>> CreateOnProvisioningHook<TObjectType, TDefinitionType>()
          where TDefinitionType : DefinitionBase
        {
            return new Action<OnCreatingContext<TObjectType, TDefinitionType>>(context =>
            {
                var id = context.GetType().GetGenericArguments()[0].AssemblyQualifiedName + ":" + context.ObjectDefinition.GetHashCode();
                var hook = _hookMap[id];

                hook.OnProvisioning = true;

                AssertService.IsNotNull(context.ModelHost);

                AssertService.IsNotNull(context.ObjectDefinition);
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

                var farmDef = context.ObjectDefinition as FarmSolutionDefinition;

                if (farmDef != null && farmDef.ShouldDelete == true)
                {
                    // by passing checking the object within onb provisioed event
                    // farm solution is expected to be deleted
                }
                else
                {
                    if (context.Object == null)
                    {
                        Console.WriteLine("");
                    }

                    AssertService.IsNotNull(context.Object);
                    AssertService.IsNotNull(context.ObjectDefinition);

                    AssertService.IsNotNull(context.ModelHost);
                    AssertService.IsInstanceOfType(context.Object, typeof(TObjectType));
                }
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


        private void AttachHooks(ModelNode modeNode, List<EventHooks> hooks)
        {
            //if (modeNode.Value.RequireSelfProcessing || modeNode.Options.RequireSelfProcessing)
            if (modeNode.Options.RequireSelfProcessing)
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

        public void TestModel(ModelNode model)
        {
            TestModels(new ModelNode[] { model });
        }

        private static HashCodeServiceBase _hasService = ServiceContainer.Instance.GetService<HashCodeServiceBase>();
        private static Dictionary<DefinitionBase, string> _definitionHashes = new Dictionary<DefinitionBase, string>();

        private static void PersistDefinitionHashes(IEnumerable<ModelNode> models)
        {
            var defs = models.SelectMany(s => s.FindNodes(n => { return true; })
                                               .Select(n => n.Value as DefinitionBase));

            foreach (var def in defs)
            {
                var hash = GetDefinitionHash(def);

                if (!_definitionHashes.ContainsKey(def))
                    _definitionHashes.Add(def, hash);
                else
                    _definitionHashes[def] = hash;
            }
        }

        private static string GetDefinitionHash(DefinitionBase def)
        {
            return _hasService.GetHashCode(def);
        }


        private void PleaseMakeSureDefinitionsWereNotChangedByModelHandlers(IEnumerable<ModelNode> models)
        {
            var defs = models.SelectMany(s => s.FindNodes(n => { return true; })
                                            .Select(n => n.Value as DefinitionBase));

            foreach (var def in defs)
            {
                var oldHash = _definitionHashes[def];
                var currentHash = GetDefinitionHash(def);

                if (oldHash != currentHash)
                {
                    throw new SPMeta2Exception(
                        string.Format("Definition was changed by model handler. Avoid changing definitions while provisioning them. [{0}]", def));
                }
            }
        }

        public void TestModels(IEnumerable<ModelNode> models)
        {
            TestModels(models, false);
        }

        public Action<ProvisionRunnerBase> BeforeProvisionRunnerExcecution { get; set; }
        public Action<ProvisionRunnerBase> AfterProvisionRunnerExcecution { get; set; }

        public void TestModels(IEnumerable<ModelNode> models, bool deployOnce)
        {
            // force XML serialiation
            GetSerializedAndRestoredModels(models);

            _hookMap.Clear();

            foreach (var model in models)
            {
                ContainerTraceUtils.WriteLine(string.Format(".ToPrettyPrint() result:"));
                ContainerTraceUtils.WriteLine(model.ToPrettyPrint());

                ContainerTraceUtils.WriteLine(string.Format(".ToDotGraph result:"));
                ContainerTraceUtils.WriteLine(model.ToDotGraph());

                if (EnableDefinitionImmutabilityValidation)
                    PersistDefinitionHashes(new[] { model });

                var allHooks = new List<EventHooks>();

                WithProvisionRunnerContext(runnerContext =>
                {
                    var runner = runnerContext.Runner;

                    if (BeforeProvisionRunnerExcecution != null)
                        BeforeProvisionRunnerExcecution(runner);

                    var omModelType = GetRunnerType(runner);
                    var hooks = new List<EventHooks>();

                    if (!deployOnce)
                    {
                        if (this.EnableDefinitionValidation)
                        {
                            hooks = GetHooks(model);

                            foreach (var hook in hooks)
                                hook.Tag = runner.Name;

                            allHooks.AddRange(hooks);
                        }
                    }

                    if (model.Value.GetType() == typeof(FarmDefinition))
                        runner.DeployFarmModel(model);
                    else if (model.Value.GetType() == typeof(WebApplicationDefinition))
                        runner.DeployWebApplicationModel(model);
                    else if (model.Value.GetType() == typeof(SiteDefinition))
                        runner.DeploySiteModel(model);
                    else if (model.Value.GetType() == typeof(WebDefinition))
                        runner.DeployWebModel(model);
                    else if (model.Value.GetType() == typeof(ListDefinition))
                        runner.DeployListModel(model);
                    else
                    {
                        throw new SPMeta2NotImplementedException(
                            string.Format("Runner does not support model of type: [{0}]", model.Value.GetType()));
                    }

                    if (!deployOnce)
                    {
                        if (this.EnableDefinitionValidation)
                        {
                            var hasMissedOrInvalidProps = ResolveModelValidation(model, hooks);
                            AssertService.IsFalse(hasMissedOrInvalidProps);
                        }
                    }

                    if (AfterProvisionRunnerExcecution != null)
                        AfterProvisionRunnerExcecution(runner);
                });

                if (!deployOnce)
                {
                    if (EnableDefinitionImmutabilityValidation)
                        PleaseMakeSureDefinitionsWereNotChangedByModelHandlers(new[] { model });
                }
            }
        }

        public virtual List<ModelNode> GetSerializedAndRestoredModels(ModelNode model)
        {
            return GetSerializedAndRestoredModels(new[] { model });
        }

        public virtual List<ModelNode> GetSerializedAndRestoredModels(IEnumerable<ModelNode> models)
        {
            var result = new List<ModelNode>();

            foreach (var model in models)
            {
                var xml = SPMeta2Model.ToXML(model);
                var xmlModelInstance = SPMeta2Model.FromXML(xml);

                var json = SPMeta2Model.ToJSON(model);
                var jsonModelInstance = SPMeta2Model.FromJSON(json);

                result.Add(xmlModelInstance);
                result.Add(jsonModelInstance);
            }

            return result;
        }

        public ModelNode TestRandomDefinition<TDefinition>(Action<TDefinition> definitionSetup)
          where TDefinition : DefinitionBase, new()
        {
            _hookMap.Clear();

            ModelNode result = null;

            var allHooks = new List<EventHooks>();

            WithProvisionRunnerContext(runnerContext =>
            {
                var runner = runnerContext.Runner;

                if (BeforeProvisionRunnerExcecution != null)
                    BeforeProvisionRunnerExcecution(runner);

                ValidateDefinitionHostRunnerSupport<TDefinition>(runner);

                var omModelType = GetRunnerType(runner);

                var definitionSandbox = ModelGeneratorService.GenerateModelTreeForDefinition<TDefinition>(omModelType);
                var additionalDefinitions = ModelGeneratorService.GetAdditionalDefinition<TDefinition>();

                ModelGeneratorService.ComposeModelWithAdditionalDefinitions(definitionSandbox, additionalDefinitions, omModelType);

                if (definitionSetup != null)
                {
                    foreach (var def in ModelGeneratorService.CurrentDefinitions)
                        definitionSetup(def as TDefinition);
                }

                var hooks = new List<EventHooks>();

                if (this.EnableDefinitionValidation)
                {
                    hooks = GetHooks(definitionSandbox);

                    foreach (var hook in hooks)
                        hook.Tag = runner.Name;

                    GetSerializedAndRestoredModels(definitionSandbox);
                    allHooks.AddRange(hooks);
                }
                else
                {
                    GetSerializedAndRestoredModels(definitionSandbox);
                }

                if (definitionSandbox.Value.GetType() == typeof(FarmDefinition))
                    runner.DeployFarmModel(definitionSandbox);

                if (definitionSandbox.Value.GetType() == typeof(WebApplicationDefinition))
                    runner.DeployWebApplicationModel(definitionSandbox);

                if (definitionSandbox.Value.GetType() == typeof(SiteDefinition))
                    runner.DeploySiteModel(definitionSandbox);

                if (definitionSandbox.Value.GetType() == typeof(WebDefinition))
                    runner.DeployWebModel(definitionSandbox);

                if (this.EnableDefinitionValidation)
                {
                    var hasMissedOrInvalidProps = ResolveModelValidation(definitionSandbox, hooks);
                    AssertService.IsFalse(hasMissedOrInvalidProps);
                }

                if (AfterProvisionRunnerExcecution != null)
                    AfterProvisionRunnerExcecution(runner);

                result = definitionSandbox;
            });

            return result;
        }

        private bool ResolveModelValidation(ModelNode modelNode, List<EventHooks> hooks)
        {
            return ResolveModelValidation(modelNode, "     ", hooks);
        }

        internal class ModelValidationResult
        {
            public ModelValidationResult()
            {
                Properties = new List<PropertyValidationResult>();
            }

            public DefinitionBase Model { get; set; }
            public List<PropertyValidationResult> Properties { get; set; }
        }

        private static List<ModelValidationResult> ModelValidations = new List<ModelValidationResult>();

        public bool EnablePropertyValidation { get; set; }
        public bool EnableEventValidation { get; set; }

        public List<Type> RegExcludedDefinitionTypes { get; set; }

        private bool ResolveModelValidation(ModelNode modelNode, string start, List<EventHooks> hooks)
        {
            // should be re-written with ModelTreeTraverseService
            ContainerTraceUtils.WriteLine(string.Format(""));

            var hasMissedOrInvalidProps = false;

            var model = modelNode.Value;
            ContainerTraceUtils.WriteLine(string.Format("[INF]{2}MODEL CHECK [{0}] - ( {1} )", model.GetType(), model.ToString(), start));

            //if (model.RequireSelfProcessing || modelNode.Options.RequireSelfProcessing)
            if (modelNode.Options.RequireSelfProcessing)
            {
                var shouldProcessFlag = !modelNode.RegIsExcludedFromValidation();

                if (RegExcludedDefinitionTypes.Contains(modelNode.Value.GetType()))
                    shouldProcessFlag = false;

                if (shouldProcessFlag)
                {

                    var modelValidationResult = ModelValidations.FirstOrDefault(r => r.Model == model);

                    var shouldBeValidatedProperties = model.GetType()
                        .GetProperties()
                        .Where(
                            p =>
                                p.GetCustomAttributes<SPMeta2.Attributes.Regression.ExpectValidationAttribute>().Count() >
                                0)
                        .ToList();


                    if (modelValidationResult == null)
                    {
                        ContainerTraceUtils.WriteLine(string.Format("[ERR]{2} Missing validation for model [{0}] - ( {1} )",
                            model.GetType(), model.ToString(), start));

                        hasMissedOrInvalidProps = true;
                        return hasMissedOrInvalidProps;
                    }

                    foreach (
                        var property in
                            modelValidationResult.Properties.OrderBy(p => p.Src != null ? p.Src.Name : p.ToString()))
                    {
                        if ((!property.IsValid) ||
                            (property.IsValid && !ShowOnlyFalseResults))
                        {
                            ContainerTraceUtils.WriteLine(
                                string.Format(
                                    "[INF]{6} [{4}] - Src prop: [{0}] Src value: [{1}] Dst prop: [{2}] Dst value: [{3}] Message:[{5}]",
                                    new object[]
                                    {
                                        property.Src != null ? property.Src.Name : string.Empty,
                                        property.Src != null ? property.Src.Value : string.Empty,

                                        property.Dst != null ? property.Dst.Name : string.Empty,
                                        property.Dst != null ? property.Dst.Value : string.Empty,

                                        property.IsValid,
                                        property.Message,
                                        start
                                    }));
                        }

                        if (!property.IsValid)
                            hasMissedOrInvalidProps = true;

                    }

                    ContainerTraceUtils.WriteLine(string.Format("[INF]{0}PROPERTY CHECK", start));

                    if (EnablePropertyValidation)
                    {
                        ContainerTraceUtils.WriteLine(string.Format("[INF]{0}EnablePropertyValidation == true. Checking...", start));

                        foreach (var shouldBeValidatedProp in shouldBeValidatedProperties.OrderBy(p => p.Name))
                        {
                            var hasValidation = false;
                            var validationResult =
                                modelValidationResult.Properties.FirstOrDefault(
                                    r => r.Src != null && r.Src.Name == shouldBeValidatedProp.Name);

                            // convert stuff
                            if (validationResult == null)
                            {
                                validationResult = modelValidationResult.Properties.FirstOrDefault(
                                    r => r.Src != null && r.Src.Name.Contains("." + shouldBeValidatedProp.Name + ")"));
                            }

                            // nullables
                            if (validationResult == null)
                            {
                                validationResult = modelValidationResult.Properties.FirstOrDefault(
                                    r => r.Src != null &&
                                         (r.Src.Name.Contains("System.Nullable`1") &&
                                          r.Src.Name.Contains(shouldBeValidatedProp.Name)));
                            }

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
                                if (!ShowOnlyFalseResults)
                                {
                                    ContainerTraceUtils.WriteLine(string.Format("[INF]{2} [{0}] - [{1}]",
                                        "VALIDATED",
                                        shouldBeValidatedProp.Name,
                                        start));
                                }
                            }
                            else
                            {
                                ContainerTraceUtils.WriteLine(string.Format("[ERR]{2} [{0}] - [{1}]",
                                    "MISSED",
                                    shouldBeValidatedProp.Name,
                                    start));
                            }
                        }
                    }
                    else
                    {
                        ContainerTraceUtils.WriteLine(string.Format("[INF]{0}EnablePropertyValidation == false. Skipping...", start));
                    }

                    ContainerTraceUtils.WriteLine(string.Format("[INF]{0}EVENT CHECK", start));

                    if (EnableEventValidation && !modelNode.RegIsExcludeFromEventsValidation())
                    {
                        ContainerTraceUtils.WriteLine(string.Format("[INF]{0}EnableEventValidation == true. Checking...", start));

                        var hook = hooks.FirstOrDefault(h => h.ModelNode == modelNode);

                        if (hook != null)
                        {
                            ResolveHook(hook, start);
                        }
                        else
                        {
                            ContainerTraceUtils.WriteLine(string.Format("[ERR]{2} Missing hook validation for model [{0}] - ( {1} )",
                                model.GetType(), model.ToString(), start));
                        }
                    }
                    else
                    {
                        ContainerTraceUtils.WriteLine(string.Format("[INF]{0}EnableEventValidation == false. Skipping...", start));
                    }
                }
                else
                {
                    ContainerTraceUtils.WriteLine(string.Format("[INF]{0} Skipping due .RegIsExcludedFromValidation ==  TRUE", start));
                }
            }
            else
            {
                ContainerTraceUtils.WriteLine(string.Format("[INF]{0} Skipping due RequireSelfProcessing ==  FALSE", start));
            }

            foreach (var childModel in modelNode.ChildModels)
            {
                var tmp = ResolveModelValidation(childModel, start + start, hooks);

                if (tmp == true)
                    hasMissedOrInvalidProps = true;
            }

            return hasMissedOrInvalidProps;
        }

        protected void ResolveHook(EventHooks eventHooks, string start)
        {
            IndentableTrace.WithScope(traceScope =>
            {
                if (eventHooks.OnProvisioning)
                {
                    if (!ShowOnlyFalseResults)
                        traceScope.WriteLine(string.Format("[INF]{0} [VALIDATED] - [OnProvisioning]", start));
                }
                else
                {

                    traceScope.WriteLine(string.Format("[ERR]{0} [MISSED] - [OnProvisioning]", start));
                }

                if (eventHooks.OnProvisioned)
                {
                    if (!ShowOnlyFalseResults)
                        traceScope.WriteLine(string.Format("[INF]{0} [VALIDATED] - [OnProvisioned]", start));
                }
                else
                {
                    traceScope.WriteLine(string.Format("[IERR]{0} [MISSED] - [OnProvisioned]", start));
                }

                AssertService.AreEqual(true, eventHooks.OnProvisioning);
                AssertService.AreEqual(true, eventHooks.OnProvisioned);
            });
        }

        protected void WithProvisionRunnerContext(Action<ProvisionRunnerContext> action)
        {
            foreach (var provisionRunner in ProvisionRunners)
            {
                var type = provisionRunner.GetType().FullName;

                provisionRunner.ProvisionGenerationCount = ProvisionGenerationCount;

                provisionRunner.EnableDefinitionProvision = EnableDefinitionProvision;
                provisionRunner.EnableDefinitionValidation = EnableDefinitionValidation;

                CurrentProvisionRunner = provisionRunner;

                ContainerTraceUtils.WriteLine(string.Format("[INF]    Testing with runner impl: [{0}]", type));
                ContainerTraceUtils.WriteLine(string.Format("[INF]    Testing with Is64BitProcess flag: [{0}]", Environment.Is64BitProcess));
                ContainerTraceUtils.WriteLine(string.Format(@"[INF]    Testing as user: [{0}\{1}]", Environment.UserDomainName, Environment.UserName));
                ContainerTraceUtils.WriteLine(string.Empty);

                ContainerTraceUtils.WriteLine(string.Format("[INF]        - Current VM: [{0}]", Environment.MachineName));
                ContainerTraceUtils.WriteLine(string.Format("[INF]        - Current VM CPU: [{0}]", Environment.ProcessorCount));
                ContainerTraceUtils.WriteLine(string.Empty);

                ContainerTraceUtils.WriteLine(string.Format("[INF]        - ProvisionGenerationCount: [{0}]", ProvisionGenerationCount));
                ContainerTraceUtils.WriteLine(string.Format("[INF]        - EnableDefinitionValidation: [{0}]", EnableDefinitionValidation));

                action(new ProvisionRunnerContext
                {
                    Runner = provisionRunner
                });
            }
        }

        private void ValidateDefinitionHostRunnerSupport<T1>(ProvisionRunnerBase runner)
        {
            ValidateDefinitionHostRunnerSupport(runner, typeof(T1));
        }

        private void ValidateDefinitionHostRunnerSupport(ProvisionRunnerBase runner, Type targetType)
        {
            var attrs = targetType.GetCustomAttributes()
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

        public int ProvisionGenerationCount { get; set; }

        public bool ShowOnlyFalseResults { get; set; }

        public bool EnableDefinitionProvision { get; set; }
        public bool EnableDefinitionValidation { get; set; }


        #endregion

        public bool EnableDefinitionImmutabilityValidation { get; set; }
    }

    public class ProvisionRunnerContext
    {
        public ProvisionRunnerBase Runner { get; set; }

    }
}
