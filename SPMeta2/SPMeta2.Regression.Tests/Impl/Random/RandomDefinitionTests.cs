using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Regression.Model.Definitions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Common;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Regression.Common.Utils;
using System.Reflection;
using SPMeta2.Attributes;
using SPMeta2.Utils;
using SPMeta2.Attributes.Regression;

using SPMeta2.Syntax.Default.Extensions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Services;
using SPMeta2.Definitions.Fields;
using SPMeta2.Regression.Exceptions;


namespace SPMeta2.Regression.Tests.Impl.Random
{
    [TestClass]
    public class RandomDefinitionTests : SPMeta2RegresionEventsTestBase
    {
        #region test

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void SelfDiagnostic_TestShouldHaveAllDefinitions()
        {
            var methods = GetType().GetMethods();

            var spMetaAssembly = typeof(FieldDefinition).Assembly;
            var allDefinitions = ReflectionUtils.GetTypesFromAssembly<DefinitionBase>(spMetaAssembly);

            foreach (var def in allDefinitions)
            {
                Trace.WriteLine(def.Name);
            }

            foreach (var definition in allDefinitions)
            {
                var hasTestMethod = HasTestMethod(definition, methods);

                Assert.IsTrue(hasTestMethod);
            }
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_BreakRoleInheritanceDefinition()
        {
            TestRandomDefinition<BreakRoleInheritanceDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_FieldDefinition()
        {
            TestRandomDefinition<FieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_BusinessDataFieldDefinition()
        {
            TestRandomDefinition<BusinessDataFieldDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_JobDefinition()
        {
            WithExcpectedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<JobDefinition>();
            });
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_ManagedAccountDefinition()
        {
            WithExcpectedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<ManagedAccountDefinition>();
            });
        }

        private static void WithExcpectedCSOMnO365RunnerExceptions(Action action)
        {
            WithExcpectedExceptions(new Type[] {
                typeof(SPMeta2UnsupportedCSOMRunnerException),
                typeof(SPMeta2UnsupportedO365RunnerException)
            }, action);
        }

        private static void WithExcpectedExceptions(IEnumerable<Type> exceptionTypes, Action action)
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

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_SandboxSolutionDefinition()
        {
            TestRandomDefinition<SandboxSolutionDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_ContentTypeDefinition()
        {
            TestRandomDefinition<ContentTypeDefinition>();
        }


        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_ContentTypeFieldLinkDefinition()
        {
            TestRandomDefinition<ContentTypeFieldLinkDefinition>();
        }


        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_ContentTypeLinkDefinition()
        {
            TestRandomDefinition<ContentTypeLinkDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_FolderDefinition()
        {
            TestRandomDefinition<FolderDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_ListFieldLinkDefinition()
        {
            TestRandomDefinition<ListFieldLinkDefinition>();
        }


        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_ModuleFileDefinition()
        {
            TestRandomDefinition<ModuleFileDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_PrefixDefinition()
        {
            WithExcpectedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<PrefixDefinition>();
            });
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_QuickLaunchNavigationNodeDefinition()
        {
            TestRandomDefinition<QuickLaunchNavigationNodeDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_SP2013WorkflowDefinition()
        {
            TestRandomDefinition<SP2013WorkflowDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_SP2013WorkflowSubscriptionDefinition()
        {
            TestRandomDefinition<SP2013WorkflowSubscriptionDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_TopNavigationNodeDefinition()
        {
            TestRandomDefinition<TopNavigationNodeDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_UserCustomActionDefinition()
        {
            TestRandomDefinition<UserCustomActionDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_FarmDefinition()
        {
            WithExcpectedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<FarmDefinition>();
            });
        }


        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_PropertyDefinition()
        {
            TestRandomDefinition<PropertyDefinition>();

        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_FeatureDefinition()
        {
            TestRandomDefinition<FeatureDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_ListItemDefinition()
        {
            TestRandomDefinition<ListItemDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_ListDefinition()
        {
            TestRandomDefinition<ListDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_ListViewDefinition()
        {
            TestRandomDefinition<ListViewDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_PublishingPageDefinition()
        {
            TestRandomDefinition<PublishingPageDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_SecurityGroupDefinition()
        {
            TestRandomDefinition<SecurityGroupDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_SecurityGroupLinkDefinition()
        {
            TestRandomDefinition<SecurityGroupLinkDefinition>();
        }


        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_SecurityRoleDefinition()
        {
            TestRandomDefinition<SecurityRoleDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_SecurityRoleLinkDefinition()
        {
            TestRandomDefinition<SecurityRoleLinkDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_SiteDefinition()
        {
            WithExcpectedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<SiteDefinition>();
            });
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_WebApplicationDefinition()
        {
            WithExcpectedCSOMnO365RunnerExceptions(() =>
            {
                TestRandomDefinition<WebApplicationDefinition>();
            });
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_WebDefinition()
        {
            TestRandomDefinition<WebDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_WebPartDefinition()
        {
            TestRandomDefinition<WebPartDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_WebPartPageDefinition()
        {
            TestRandomDefinition<WebPartPageDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_WikiPageDefinition()
        {
            TestRandomDefinition<WikiPageDefinition>();
        }

        [TestMethod]
        [TestCategory("Regression.RandomDefinition")]
        public void CanRaiseEvents_ListItemFieldValueDefinition()
        {
            TestRandomDefinition<ListItemFieldValueDefinition>();
        }

        #endregion

        #region utils

        private bool HasTestMethod(Type definition, MethodInfo[] methods)
        {
            var methodPrefix = "CanRaiseEvents";
            var methodName = string.Format("{0}_{1}", methodPrefix, definition.Name);

            Trace.WriteLine(string.Format("Asserting method:[{0}]", methodName));

            var targetMethod = methods.FirstOrDefault(m => m.Name == methodName);

            return targetMethod != null;
        }

        private void TestRandomDefinition<TDefinition>()
            where TDefinition : DefinitionBase, new()
        {
            var allHooks = new List<EventHooks>();

            WithProvisionRunners(runner =>
            {
                ValidateDefinitionHostRunnerSupport<TDefinition>(runner);

                var sandboxService = new ModelGeneratorService();
                var definitionSandbox = sandboxService.GenerateModelTreeForDefinition<TDefinition>();

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

                foreach (var hook in hooks)
                    ResolveHook(hook);
            });

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
            if (modeNode.Value.RequireSelfProcessing)
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
