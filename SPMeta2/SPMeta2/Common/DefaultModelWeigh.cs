using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Common
{
    /// <summary>
    /// Indicates default model ordering while provision process.
    /// For instance, fields are to be provisioned before content types, workflow definitions before list definitions, etc.
    /// </summary>
    public static class DefaultModelWeigh
    {
        #region static

        static DefaultModelWeigh()
        {
            Weighs = new List<ModelWeigh>();

            InitSiteScope();
            InitWebScope();
            InitListScope();

            InitListItemScope();
            InitModuleFileScope();

            InitFolderScope();
        }

        private static void InitModuleFileScope()
        {
            Weighs.Add(new ModelWeigh(
                 typeof(ModuleFileDefinition),
                 new[]
                {
                    typeof (BreakRoleInheritanceDefinition),
                    typeof (ResetRoleInheritanceDefinition),
                }));
        }

        #endregion

        #region properties

        /// <summary>
        /// Default weighs for correct model provision order.
        /// </summary>
        public static List<ModelWeigh> Weighs { get; set; }

        #endregion

        #region methods

        private static void InitListItemScope()
        {
            Weighs.Add(new ModelWeigh(
                typeof(ListItemDefinition),
                new[]
                {
                    typeof (BreakRoleInheritanceDefinition),
                    typeof (ResetRoleInheritanceDefinition),
                    typeof (SecurityRoleLinkDefinition),

                    typeof (PropertyDefinition)
                }));
        }

        private static void InitFolderScope()
        {
            Weighs.Add(new ModelWeigh(
                typeof(FolderDefinition),
                new[]
                {
                    typeof (BreakRoleInheritanceDefinition),
                    typeof (ResetRoleInheritanceDefinition),
                    typeof (SecurityRoleLinkDefinition),

                    typeof (PropertyDefinition)
                }));
        }

        private static void InitListScope()
        {
            Weighs.Add(new ModelWeigh(
                typeof(ListDefinition),
                new[]
                {
                    typeof (BreakRoleInheritanceDefinition),
                    typeof (ResetRoleInheritanceDefinition),
                    typeof (SecurityRoleLinkDefinition),

                    typeof (PropertyDefinition),
                    
                    typeof (ContentTypeDefinition),
                    typeof (ContentTypeLinkDefinition),
                    
                    // Content type related provision should be done before list items provision #636
                    // https://github.com/SubPointSolutions/spmeta2/issues/636
                    typeof (RemoveContentTypeLinksDefinition),
                    typeof (HideContentTypeLinksDefinition),
                    typeof (UniqueContentTypeOrderDefinition),

                    // field and field links could be added with 'AddToAllContentTypes' options
                    // we need content types deployed first
                    typeof (FieldDefinition),
                    typeof (LookupFieldDefinition),
                    typeof (DependentLookupFieldDefinition),

                    typeof (ListFieldLinkDefinition),

                    typeof (SP2013WorkflowSubscriptionDefinition),
                    
                    typeof (FolderDefinition),
                    
                    typeof (ListViewDefinition),
                    typeof (ModuleFileDefinition),

                    typeof (ListItemDefinition),
                    typeof (ListItemFieldValueDefinition),
                }));
        }

        private static void InitWebScope()
        {
            Weighs.Add(new ModelWeigh(
                typeof(WebDefinition),
                new[]
                {
                    typeof(ClearRecycleBinDefinition),

                    // AppDefinition should be deployed before pages #628
                    // https://github.com/SubPointSolutions/spmeta2/issues/628
                    typeof (AppDefinition),
                    
                    typeof (FeatureDefinition),

                    typeof (SecurityGroupDefinition),

                    typeof (BreakRoleInheritanceDefinition),
                    typeof (ResetRoleInheritanceDefinition),

                    typeof (SecurityRoleLinkDefinition),
                    typeof (AnonymousAccessSettingsDefinition),
                    
                    typeof (PropertyDefinition),
                    
                    typeof (FieldDefinition),
                    typeof (LookupFieldDefinition),
                    typeof (DependentLookupFieldDefinition),

                    typeof (ContentTypeDefinition),
                    typeof (SP2013WorkflowDefinition),
                    
                    typeof (ListDefinition),
                    
                    // moved navigation provision after lists
                    // cause adding libraries would trigger 'Recent' link
                    // https://github.com/SubPointSolutions/spmeta2/issues/865

                    // removing navigation first, then add
                    typeof (DeleteQuickLaunchNavigationNodesDefinition),
                    typeof (DeleteTopNavigationNodesDefinition),
                    
                    typeof (QuickLaunchNavigationNodeDefinition),
                    typeof (TopNavigationNodeDefinition),

                    // goes after list definitions to make sure you get history/task lists 
                    typeof (SP2013WorkflowSubscriptionDefinition),
                    typeof (WorkflowAssociationDefinition),

                    typeof (MasterPageSettingsDefinition),
                    typeof (WelcomePageDefinition)
                }));
        }

        private static void InitSiteScope()
        {
            // site scope
            Weighs.Add(new ModelWeigh(
                typeof(SiteDefinition),
                new[]
                {
                    typeof (SandboxSolutionDefinition),
                    typeof (FeatureDefinition),
                    
                    typeof (PropertyDefinition),

                    typeof (SecurityGroupDefinition),
                    typeof (SecurityRoleDefinition),

                    typeof (UserCustomActionDefinition),

                    typeof (FieldDefinition),
                    typeof (LookupFieldDefinition),
                    typeof (DependentLookupFieldDefinition),

                    typeof (ContentTypeDefinition),
                    
                    typeof (WebDefinition)
                }));
        }

        #endregion
    }
}
