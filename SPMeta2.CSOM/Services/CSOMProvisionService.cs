using System;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Services;

namespace SPMeta2.CSOM.Services
{
    public class CSOMProvisionService : ModelServiceBase
    {
        #region contructors

        public CSOMProvisionService()
        {
            RegisterModelHandlers();
        }

        private void RegisterModelHandlers()
        {
            ModelHandlers.Add(typeof(FieldDefinition), new FieldModelHandler());
            ModelHandlers.Add(typeof(ContentTypeFieldLinkDefinition), new ContentTypeFieldLinkModelHandler());

            ModelHandlers.Add(typeof(ContentTypeDefinition), new ContentTypeModelHandler());
            ModelHandlers.Add(typeof(ContentTypeLinkDefinition), new ContentTypeLinkModelHandler());

            ModelHandlers.Add(typeof(SecurityGroupDefinition), new SecurityGroupModelHandler());
            ModelHandlers.Add(typeof(SecurityGroupLinkDefinition), new SecurityGroupLinkModelHandler());

            ModelHandlers.Add(typeof(SecurityRoleDefinition), new SecurityRoleModelHandler());
            ModelHandlers.Add(typeof(SecurityRoleLinkDefinition), new SecurityRoleLinkModelHandler());

            ModelHandlers.Add(typeof(ListDefinition), new ListModelHandler());
            ModelHandlers.Add(typeof(ListViewDefinition), new ListViewModelHandler());

            ModelHandlers.Add(typeof(WebPartPageDefinition), new WebPartPageModelHandler());
            ModelHandlers.Add(typeof(WikiPageDefinition), new WikiPageModelHandler());

            ModelHandlers.Add(typeof(WebPartDefinition), new WebPartModelHandler());

            ModelHandlers.Add(typeof(WebDefinition), new WebModelHandler());
            ModelHandlers.Add(typeof(SiteDefinition), new SiteModelHandler());

            ModelHandlers.Add(typeof(FeatureDefinition), new FeatureModelHandler());
            ModelHandlers.Add(typeof(UserCustomActionDefinition), new UserCustomActionModelHandler());

            ModelHandlers.Add(typeof(ModuleFileDefinition), new ModuleFileModelHandler());
            ModelHandlers.Add(typeof(FolderDefinition), new FolderModelHandler());

            ModelHandlers.Add(typeof(SP2013WorkflowSubscriptionDefinition), new SP2013WorkflowSubscriptionDefinitionModelHandler());
            ModelHandlers.Add(typeof(SP2013WorkflowDefinition), new SP2013WorkflowDefinitionHandler());

            ModelHandlers.Add(typeof(QuickLaunchNavigationNodeDefinition), new QuickLaunchNavigationNodeModelHandler());

            ModelHandlers.Add(typeof(PropertyDefinition), new PropertyModelHandler());
            ModelHandlers.Add(typeof(ListItemDefinition), new ListItemModelHandler());
            ModelHandlers.Add(typeof(ListItemFieldValueDefinition), new ListItemFieldValueModelHandler());
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, ModelNode model)
        {
            if (!(modelHost is CSOMModelHostBase))
                throw new ArgumentException("model host for CSOM needs to be inherited from CSOMModelHostBase");

            base.DeployModel(modelHost, model);
        }

        #endregion
    }
}
