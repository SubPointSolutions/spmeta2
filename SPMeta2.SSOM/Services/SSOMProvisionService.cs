using SPDevLab.SPMeta2.Definitions;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHandlers;

namespace SPMeta2.SSOM.Services
{
    public class SSOMProvisionService : ModelServiceBase
    {
        #region contructors

        public SSOMProvisionService()
        {
            RegisterModelHandlers();
        }

        private void RegisterModelHandlers()
        {
            ModelHandlers.Add(typeof(FieldDefinition), new FieldModelHandler());
            ModelHandlers.Add(typeof(ContentTypeFieldLinkDefinition), new ContentTypeFieldLinkModelHandler());

            ModelHandlers.Add(typeof(ContentTypeDefinition), new ContentTypeModelHandler());
            ModelHandlers.Add(typeof(ContentTypeLinkDefinition), new ContentTypeLinkModelHandler());

            ModelHandlers.Add(typeof(WebPartPageDefinition), new WebPartPageModelHandler());
            ModelHandlers.Add(typeof(WikiPageDefinition), new WikiPageModelHandler());

            ModelHandlers.Add(typeof(WebPartDefinition), new WebPartModelHandler());

            ModelHandlers.Add(typeof(ListDefinition), new ListModelHandler());
            ModelHandlers.Add(typeof(ListViewDefinition), new ListViewModelHandler());

            ModelHandlers.Add(typeof(SecurityGroupDefinition), new SecurityGroupModelHandler());
            ModelHandlers.Add(typeof(SecurityGroupLinkDefinition), new SecurityGroupLinkModelHandler());

            ModelHandlers.Add(typeof(SecurityRoleDefinition), new SecurityRoleModelHandler());
            ModelHandlers.Add(typeof(SecurityRoleLinkDefinition), new SecurityRoleLinkModelHandler());

            ModelHandlers.Add(typeof(WebDefinition), new WebModelHandler());
            ModelHandlers.Add(typeof(SiteDefinition), new SiteModelHandler());

            ModelHandlers.Add(typeof(FeatureDefinition), new FeatureModelHandler());
            ModelHandlers.Add(typeof(UserCustomActionDefinition), new UserCustomActionModelHandler());

            ModelHandlers.Add(typeof(FarmDefinition), new FarmModelHandler());

            ModelHandlers.Add(typeof(ModuleFileDefinition), new ModuleFileModelHandler());
            ModelHandlers.Add(typeof(FolderDefinition), new FolderModelHandler());

            ModelHandlers.Add(typeof(QuickLaunchNavigationNodeDefinition), new QuickLaunchNavigationNodeModelHandler());
        }

        #endregion
    }
}
