using System;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class ModelWithNamespaces
    {
        #region methods

        #region features

        public static ModelNode WithSP2013Workflows(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        public static ModelNode WithFarmFeatures(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        public static ModelNode WithWebApplicationFeatures(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        public static ModelNode WithWebFeatures(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        public static ModelNode WithSiteFeatures(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        public static ModelNode WithUserCustomActions(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        #endregion

        #region sites and webs

        public static ModelNode WithSubWebs(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        #endregion

        #region security

        public static ModelNode WithSecurityRoles(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        public static ModelNode WithSecurityGroups(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        #endregion

        #region fields and conent types

        public static ModelNode WithFields(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        public static ModelNode WithContentTypes(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        #endregion

        #region lists

        public static ModelNode WithLists(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        public static ModelNode WithListViews(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        #endregion

        #region pages

        public static ModelNode WithPages(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        public static ModelNode WithWebPartPages(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        public static ModelNode WithWikiPages(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        public static ModelNode WithPublishingPages(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        #endregion

        #region web parts

        public static ModelNode WithWebParts(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        #endregion

        #region navigation

        public static ModelNode WithQuickLaunchNavigation(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        public static ModelNode WithTopNavigation(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }


        #endregion

        #endregion

        #region solutions

        public static ModelNode WithFarmSolutions(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        public static ModelNode WithSandboxSolutions(this ModelNode model, Action<ModelNode> action)
        {
            action(model);

            return model;
        }

        #endregion
    }
}
