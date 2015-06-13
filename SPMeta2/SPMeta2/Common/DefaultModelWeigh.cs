using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    typeof (FeatureDefinition),

                    typeof (SecurityGroupDefinition),

                    typeof (BreakRoleInheritanceDefinition),
                    typeof (ResetRoleInheritanceDefinition),

                    typeof (SecurityRoleLinkDefinition),

                    typeof (PropertyDefinition),
                    
                    typeof (FieldDefinition),
                    typeof (LookupFieldDefinition),
                    typeof (DependentLookupFieldDefinition),

                    typeof (ContentTypeDefinition),
                    
                    typeof (SP2013WorkflowDefinition),
                    
                    typeof (ListDefinition),
                    // goes after list definitions to make sure you get history/task lists 
                    typeof (SP2013WorkflowSubscriptionDefinition),

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
