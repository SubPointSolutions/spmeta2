using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Syntax.Default
{
    public static class SPMeta2Model
    {
        #region farm

        #endregion

        #region web applications

        #endregion

        #region sites

        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty site model".
        /// Site model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// </summary>
        /// <returns></returns>
        public static ModelNode NewSiteModel()
        {
            return NewSiteModel((SiteDefinition)null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty site model".
        /// Site model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// Use action to get access to the "site model node" and construct model tree.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ModelNode NewSiteModel(Action<ModelNode> action)
        {
            var definition = new SiteDefinition { RequireSelfProcessing = false };

            return NewSiteModel(definition, action);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding site model provided.
        /// If RequireSelfProcessing set as 'true', then site model is going to be processed and pushed by SPMeta2 API.
        /// </summary>
        /// <param name="siteDefinition"></param>
        /// <returns></returns>
        public static ModelNode NewSiteModel(SiteDefinition siteDefinition)
        {
            return NewSiteModel(siteDefinition, null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding site model provided.
        /// If RequireSelfProcessing set as 'true', then site model is going to be processed and pushed by SPMeta2 API.
        /// Use action to get access to the "site model node" and construct model tree.
        /// </summary>
        /// <param name="siteDefinition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ModelNode NewSiteModel(SiteDefinition siteDefinition, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = siteDefinition ?? new SiteDefinition { RequireSelfProcessing = false } };

            if (action != null)
                action(newModelNode);

            return newModelNode;
        }

        #endregion

        #region webs

        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty web model".
        /// Web model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// </summary>
        /// <returns></returns>
        public static ModelNode NewWebModel()
        {
            return NewWebModel((WebDefinition)null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty web model".
        /// Web model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// Use action to get access to the "site model node" and construct model tree.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ModelNode NewWebModel(Action<ModelNode> action)
        {
            var definition = new WebDefinition { RequireSelfProcessing = false };

            return NewWebModel(definition, action);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding web model provided.
        /// If RequireSelfProcessing set as 'true', then web model is going to be processed and pushed by SPMeta2 API.
        /// </summary>
        /// <param name="webDefinition"></param>
        /// <returns></returns>
        public static ModelNode NewWebModel(WebDefinition webDefinition)
        {
            return NewWebModel(webDefinition, null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding web model provided.
        /// If RequireSelfProcessing set as 'true', then web model is going to be processed and pushed by SPMeta2 API.
        /// Use action to get access to the "web model node" and construct model tree.
        /// </summary>
        /// <param name="webDefinition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ModelNode NewWebModel(WebDefinition webDefinition, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = webDefinition ?? new WebDefinition { RequireSelfProcessing = false } };

            if (action != null)
                action(newModelNode);

            return newModelNode;
        }

        #endregion

        #region Obsolete

        [Obsolete("Please use NewSiteModel()/NewWebModel() methods. NewModel() will be removed in further versions of SPMeta2 API.")]
        public static ModelNode NewModel()
        {
            return new ModelNode();
        }


        [Obsolete("Please use NewSiteModel() methods. DummySite() will be removed in further versions of SPMeta2 API.")]
        public static ModelNode DummySite(this ModelNode node)
        {
            node.Value = new SiteDefinition { RequireSelfProcessing = false };

            return node;
        }

        [Obsolete("Please use NewWebModel() methods. DummyWeb() will be removed in further versions of SPMeta2 API.")]
        public static ModelNode DummyWeb(this ModelNode node)
        {
            node.Value = new WebDefinition { RequireSelfProcessing = false };

            return node;
        }

        #endregion
    }
}
