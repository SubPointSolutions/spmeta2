using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Syntax.Default
{
    /// <summary>
    /// Initial entry to start model build up process.
    /// </summary>
    public static class SPMeta2Model
    {
        #region farm

        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty model".
        /// Model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// </summary>
        /// <returns></returns>
        public static ModelNode NewFarmModel()
        {
            return NewFarmModel((FarmDefinition)null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty model".
        /// Model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// Use action to get access to the "model node" and construct model tree.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ModelNode NewFarmModel(Action<ModelNode> action)
        {
            return NewFarmModel(new FarmDefinition { RequireSelfProcessing = false }, action);
        }


        /// <summary>
        /// Creates a new instance of the ModelNode adding model provided.
        /// If RequireSelfProcessing set as 'true', then model is going to be processed and pushed by SPMeta2 API.
        /// </summary>
        /// <param name="farmDefinition"></param>
        /// <returns></returns>
        public static ModelNode NewFarmModel(FarmDefinition farmDefinition)
        {
            return NewFarmModel(farmDefinition, null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding model provided.
        /// If RequireSelfProcessing set as 'true', then site model is going to be processed and pushed by SPMeta2 API.
        /// Use action to get access to the "model node" and construct model tree.
        /// </summary>
        /// <param name="farmDefinition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ModelNode NewFarmModel(FarmDefinition farmDefinition, Action<ModelNode> action)
        {
            return NewModelNode<FarmDefinition>(farmDefinition, action);

        }

        #endregion

        #region web applications


        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty model".
        /// Model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// </summary>
        /// <returns></returns>
        public static ModelNode NewWebApplicationModel()
        {
            return NewWebApplicationModel((WebApplicationDefinition)null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding "empty model".
        /// Model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// Use action to get access to the "model node" and construct model tree.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ModelNode NewWebApplicationModel(Action<ModelNode> action)
        {
            return NewWebApplicationModel(new WebApplicationDefinition { RequireSelfProcessing = false }, action);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding model provided.
        /// If RequireSelfProcessing set as 'true', then model is going to be processed and pushed by SPMeta2 API.
        /// </summary>
        /// <param name="webApplicationDefinition"></param>
        /// <returns></returns>
        public static ModelNode NewWebApplicationModel(WebApplicationDefinition webApplicationDefinition)
        {
            return NewWebApplicationModel(webApplicationDefinition, null);
        }

        /// <summary>
        /// Creates a new instance of the ModelNode adding model provided.
        /// If RequireSelfProcessing set as 'true', then site model is going to be processed and pushed by SPMeta2 API.
        /// Use action to get access to the "model node" and construct model tree.
        /// </summary>
        /// <param name="webApplicationDefinition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ModelNode NewWebApplicationModel(WebApplicationDefinition webApplicationDefinition, Action<ModelNode> action)
        {
            return NewModelNode<WebApplicationDefinition>(webApplicationDefinition, action);

        }

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
            return NewSiteModel(new SiteDefinition { RequireSelfProcessing = false }, action);
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
            return NewModelNode<SiteDefinition>(siteDefinition, action);
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
        /// Creates a new instance of the ModelNode adding "empty web model".
        /// Web model is not going to be pushes by SPMeta2 API, it just required to be there for model tree processing.
        /// Use action to get access to the "site model node" and construct model tree.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ModelNode NewWebModel(Action<ModelNode> action)
        {
            return NewWebModel(new WebDefinition { RequireSelfProcessing = false }, action);
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
            return NewModelNode<WebDefinition>(webDefinition, action);
        }

        #endregion

        #region utils

        private static ModelNode NewModelNode<TModelDefinition>(TModelDefinition model, Action<ModelNode> action)
          where TModelDefinition : DefinitionBase, new()
        {
            var newModelNode = new ModelNode { Value = model ?? new TModelDefinition { RequireSelfProcessing = false } };

            // levacy
            newModelNode.Options.RequireSelfProcessing = newModelNode.Value.RequireSelfProcessing;

            if (action != null)
                action(newModelNode);

            return newModelNode;
        }

        #endregion

        #region Obsolete

        [Obsolete("Please use NewSiteModel()/NewWebModel()/NewWebApplicationModel()/NewFarmModel() methods. NewModel() will be removed in further versions of SPMeta2 API.")]
        public static ModelNode NewModel()
        {
            return new ModelNode();
        }

        [Obsolete("Please use NewSiteModel()/NewWebModel()/NewWebApplicationModel()/NewFarmModel() methods.. DummySite() will be removed in further versions of SPMeta2 API.")]
        public static ModelNode DummySite(this ModelNode node)
        {
            node.Value = new SiteDefinition { RequireSelfProcessing = false };

            return node;
        }

        [Obsolete("Please use NewSiteModel()/NewWebModel()/NewWebApplicationModel()/NewFarmModel() methods.. DummyWeb() will be removed in further versions of SPMeta2 API.")]
        public static ModelNode DummyWeb(this ModelNode node)
        {
            node.Value = new WebDefinition { RequireSelfProcessing = false };

            return node;
        }

        #endregion
    }
}
