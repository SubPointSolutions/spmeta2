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

namespace SPMeta2.Regression.Tests.Impl.Events
{
    public abstract class DefinitionEventsTestBase : SPMeta2RegresionEventsTestBase
    {
        #region tests

        public abstract void CanRaiseEvents_BreakRoleInheritanceDefinition();

        public abstract void CanRaiseEvents_ContentTypeDefinition();
        public abstract void CanRaiseEvents_ContentTypeFieldLinkDefinition();
        public abstract void CanRaiseEvents_ContentTypeLinkDefinition();

        public abstract void CanRaiseEvents_FarmDefinition();

        public abstract void CanRaiseEvents_FeatureDefinition();
        public abstract void CanRaiseEvents_FieldDefinition();
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
