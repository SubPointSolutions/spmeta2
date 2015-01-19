using System;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class ModelWithNamespaces
    {
        #region methods

        public static ModelNode WithAddAudiences(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithRootWeb(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithDocumentParsers(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithTargetApplications(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithDiagnosticsServices(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithAlternateUrls(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithSP2013Workflows(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithFarmFeatures(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithWebConfigModifications(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithWebApplicationFeatures(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithWebFeatures(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithEventReceivers(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithJobs(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithManagedAccounts(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithContentTypeFieldLinks(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithContentTypeLinks(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithFolders(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithListFieldLinks(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }


        public static ModelNode WithModuleFiles(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithQuickLaunchNavigationNodes(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithSP2013WorkflowSubscriptions(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithTopNavigationNodes(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithProperties(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithListItems(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }


        public static ModelNode WithSecurityRoleLinks(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithSecurityGroupLinks(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithFeatures(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }


        public static ModelNode WithWebApplications(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithWebs(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithListItemFieldValues(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithAudiences(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithImageRenditions(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithSearchResults(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithPrefixes(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithSiteFeatures(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithUserCustomActions(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithContentDatabases(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithSubWebs(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithSecurityRoles(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithSecurityGroups(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithFields(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithContentTypes(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithApps(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithLists(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithListViews(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithPages(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithWebPartPages(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithWikiPages(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithPublishingPages(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithPublishingPageLayouts(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithWebParts(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithQuickLaunchNavigation(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithTopNavigation(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithFarmSolutions(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        public static ModelNode WithSandboxSolutions(this ModelNode model, Action<ModelNode> action)
        {
            return MakeScopeCall(model, action);
        }

        #endregion

        #region internal

        private static ModelNode MakeScopeCall(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        #endregion
    }
}
