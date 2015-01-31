using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            InitFolderScope();
        }

        #endregion

        #region properties

        /// <summary>
        /// Default weighs for correct model provision order.
        /// </summary>
        public static List<ModelWeigh> Weighs { get; set; }

        #endregion

        #region methods

        private static void InitFolderScope()
        {
            Weighs.Add(new ModelWeigh(
                typeof(FolderDefinition),
                new[]
                {
                    typeof (BreakRoleInheritanceDefinition),
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
                    typeof (SecurityRoleLinkDefinition),

                    typeof (PropertyDefinition),
                    
                    // list scoped fields should be deployed first as well as content type links
                    typeof (FieldDefinition),
                    typeof (ListFieldLinkDefinition),

                    typeof (ContentTypeDefinition),
                    typeof (ContentTypeLinkDefinition),

                    typeof (SP2013WorkflowSubscriptionDefinition),
                    
                    typeof (FolderDefinition),
                    
                    typeof (ListViewDefinition),
                    typeof (ModuleFileDefinition),

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
                    typeof (SecurityRoleLinkDefinition),

                    typeof (PropertyDefinition),
                    
                    typeof (FieldDefinition),
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
                    typeof (ContentTypeDefinition),
                    
                    typeof (WebDefinition)
                }));
        }

        #endregion
    }
}
