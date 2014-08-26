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

namespace SPMeta2.Regression.Tests.Impl.Events
{
    [TestClass]
    public class DefinitionEventsTests : SPMeta2RegresionEventsTestBase
    {
        [TestMethod]
        [TestCategory("Regression.Events")]
        public void CanRaiseEvents_FieldDefinitionNew()
        {
            WithProvisionRunners(runner =>
            {
                WithEventHooks(hooks =>
                {
                    var model = SPMeta2Model.NewSiteModel(site =>
                    {
                        site
                            .AddField(RegSiteFields.BooleanField, field =>
                            {
                                var typeName = ResolveSPType(field);
                                AssertEventHooksByStringType(typeName, field, hooks);
                            });
                    });

                    runner.DeploySiteModel(model);

                });
            });
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

            var typeName = CurrentProvisionRunner.ResolveFullTypeName(className, classAssembly);
            return typeName;
        }

        #region hooks

        protected virtual Action<OnCreatingContext<TObjectType, TDefinitionType>> CreateOnProvisioningHook<TObjectType, TDefinitionType>()
          where TDefinitionType : DefinitionBase
        {
            return new Action<OnCreatingContext<TObjectType, TDefinitionType>>(context =>
            {
                DefinitionEventsTests.CurrentHook.OnProvisioning = true;

                Assert.IsNotNull(context.ObjectDefinition);
            });
        }

        protected virtual Action<OnCreatingContext<TObjectType, TDefinitionType>> CreateOnProvisionedHook<TObjectType, TDefinitionType>()
            where TDefinitionType : DefinitionBase
        {
            return new Action<OnCreatingContext<TObjectType, TDefinitionType>>(context =>
            {
                DefinitionEventsTests.CurrentHook.OnProvisioned = true;

                Assert.IsNotNull(context.Object);
                Assert.IsNotNull(context.ObjectDefinition);

                Assert.IsInstanceOfType(context.Object, typeof(TObjectType));
            });
        }

        private static EventHooks CurrentHook;

        protected virtual void AssertEventHooksByStringType(string spType, ModelNode modelNode, EventHooks hooks)
        {
            CurrentHook = hooks;
            var spObjectType = Type.GetType(spType);
            var modelContextType = typeof(OnCreatingContext<,>);

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
    }

    public abstract class DefinitionEventsTestBase : SPMeta2RegresionEventsTestBase
    {
        #region new




        #endregion

        #region tests

        public abstract void CanRaiseEvents_BreakRoleInheritanceDefinition();

        public abstract void CanRaiseEvents_ContentTypeDefinition();
        public abstract void CanRaiseEvents_ContentTypeFieldLinkDefinition();
        public abstract void CanRaiseEvents_ContentTypeLinkDefinition();

        public abstract void CanRaiseEvents_FarmDefinition();

        public abstract void CanRaiseEvents_FeatureDefinition();

        //public virtual void CanRaiseEvents_FieldDefinition()
        //{
        //    WithProvisionRunners(runner =>
        //    {
        //        WithEventHooks(hooks =>
        //        {
        //            var model = SPMeta2Model.NewSiteModel(site =>
        //            {
        //                site
        //                    .AddField(RegSiteFields.BooleanField, field =>
        //                    {
        //                        if (CurrentProvisionRunner.Name == "SSOM")
        //                        {
        //                            AssertEventHooksByStringType("Microsoft.SharePoint.SPField, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c", field, hooks);
        //                        }

        //                        if (CurrentProvisionRunner.Name == "CSOM")
        //                        {
        //                            AssertEventHooksByStringType("Microsoft.SharePoint.Client.Field, Microsoft.SharePoint.Client, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c", field, hooks);
        //                        }
        //                    });
        //            });

        //            runner.DeploySiteModel(model);

        //        });
        //    });
        //}


        public abstract void CanRaiseEvents_FolderDefinition();

        public abstract void CanRaiseEvents_JobDefinition();

        public abstract void CanRaiseEvents_ListDefinition();
        public abstract void CanRaiseEvents_ListFieldLinkDefinition();

        public abstract void CanRaiseEvents_ListItemDefinition();
        public abstract void CanRaiseEvents_ListItemFieldValueDefinition();

        public abstract void CanRaiseEvents_ListViewDefinition();

        public abstract void CanRaiseEvents_ManagedAccountDefinition();

        public abstract void CanRaiseEvents_ModuleFileDefinition();
        public abstract void CanRaiseEvents_PrefixDefinition();

        public abstract void CanRaiseEvents_PropertyDefinition();
        public abstract void CanRaiseEvents_PublishingPageDefinition();

        public abstract void CanRaiseEvents_QuickLunchNavigationNodeDefinition();

        public abstract void CanRaiseEvents_SandboxSolutionDefinition();


        public abstract void CanRaiseEvents_SecurityGroupDefinition();
        public abstract void CanRaiseEvents_SecurityGroupLinkDefinition();
        public abstract void CanRaiseEvents_SecurityRoleDefinition();
        public abstract void CanRaiseEvents_SecurityRoleLinkDefinition();

        public abstract void CanRaiseEvents_SiteDefinition();

        public abstract void CanRaiseEvents_SP2013WorkflowDefinition();
        public abstract void CanRaiseEvents_SP2013WorkflowSubscriptionDefinition();

        public abstract void CanRaiseEvents_TopNavigationNodeDefinition();

        public abstract void CanRaiseEvents_UserCustomActionDefinition();

        public abstract void CanRaiseEvents_WebApplicationDefinition();

        public abstract void CanRaiseEvents_WebDefinition();
        public abstract void CanRaiseEvents_WebPartDefinition();
        public abstract void CanRaiseEvents_WebPartPageDefinition();
        public abstract void CanRaiseEvents_WikiPageDefinition();

        #endregion


    }
}
