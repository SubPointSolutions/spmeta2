using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Model.Definitions;
using SPMeta2.Regression.Tests.Impl.Events;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.O365.Impl
{
    [TestClass]
    public class DefinitionEventsTest : DefinitionEventsTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_FieldDefinition()
        {
            ValidateSiteModelEvents<Field>(SPMeta2Model.NewSiteModel(), RegSiteFields.BooleanField);
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ContentTypeDefinition()
        {
            ValidateSiteModelEvents<ContentType>(SPMeta2Model.NewSiteModel(), RegContentTypes.CustomItem);
        }

        #endregion

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ContentTypeFieldLinkDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ContentTypeLinkDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_FarmDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_FeatureDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_FolderDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ListDefinition()
        {
            ValidateWebModelEvents<List>(SPMeta2Model.NewWebModel(), RegLists.GenericList);
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ListItemDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ListItemFieldValueDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ListViewDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_ModuleFileDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_PropertyDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_PublishingPageDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_QuickLunchNavigationNodeDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_SecurityGroupDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_SecurityGroupLinkDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_SecurityRoleDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_SecurityRoleLinkDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_SiteDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_SP2013WorkflowDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_SP2013WorkflowSubscriptionDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_UserCustomActionDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_WebDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_WebPartDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_WebPartPageDefinition()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Events.O365")]
        public override void CanRaiseEvents_WikiPageDefinition()
        {
            throw new NotImplementedException();
        }
    }
}
