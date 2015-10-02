using System;
using System.Reflection;
using SPMeta2.Containers.DefinitionGenerators;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Standard.DefinitionGenerators;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Containers.Standard
{
    public static class RandomStandardContainersSyntax
    {
        #region constructors

        static RandomStandardContainersSyntax()
        {
            ModelGeneratorService = new ModelGeneratorService();
            ModelGeneratorService.RegisterDefinitionGenerators(Assembly.GetExecutingAssembly());

            RandomContainersSyntax.ModelGeneratorService.RegisterDefinitionGenerators(Assembly.GetExecutingAssembly());
        }

        #endregion

        #region properties

        public static ModelGeneratorService ModelGeneratorService { get; set; }

        #endregion

        #region syntax

        #region publishing pages

        public static TModelNode AddRandomPublishingPage<TModelNode>(this TModelNode model)
              where TModelNode : TypedModelNode, IListItemHostModelNode, new()
        {
            return AddRandomPublishingPage(model, null);
        }

        public static TModelNode AddRandomPublishingPage<TModelNode>(this TModelNode model,
            Action<PublishingPageModelNode> action)
               where TModelNode : TypedModelNode, IListItemHostModelNode, new()
        {
            return model.AddRandomTypedDefinition<PublishingPageDefinition, TModelNode, PublishingPageModelNode>(action);
        }

        #endregion


        #region taxonomy

        public static SiteModelNode AddRandomTermStore(this SiteModelNode model)
        {
            return AddRandomTermStore(model, null);
        }

        public static SiteModelNode AddRandomTermStore(this SiteModelNode model,
            Action<TaxonomyTermStoreModelNode> action)
        {
            return model.AddRandomTypedDefinition<TaxonomyTermStoreDefinition, SiteModelNode, TaxonomyTermStoreModelNode>(action);
        }

        public static TaxonomyTermStoreModelNode AddRandomTermGroup(this TaxonomyTermStoreModelNode model)
        {
            return AddRandomTermGroup(model, null);
        }

        public static TaxonomyTermStoreModelNode AddRandomTermGroup(this TaxonomyTermStoreModelNode model,
            Action<TaxonomyTermGroupModelNode> action)
        {
            return model.AddRandomTypedDefinition<TaxonomyTermGroupDefinition, TaxonomyTermStoreModelNode, TaxonomyTermGroupModelNode>(action);
        }

        public static TaxonomyTermGroupModelNode AddRandomTermSet(this TaxonomyTermGroupModelNode model)
        {
            return AddRandomTermSet(model, null);
        }

        public static TaxonomyTermGroupModelNode AddRandomTermSet(this TaxonomyTermGroupModelNode model,
            Action<TaxonomyTermSetModelNode> action)
        {
            return model.AddRandomTypedDefinition<TaxonomyTermSetDefinition, TaxonomyTermGroupModelNode, TaxonomyTermSetModelNode>(action);
        }

        public static TaxonomyTermSetModelNode AddRandomTerm(this TaxonomyTermSetModelNode model)
        {
            return AddRandomTerm(model, null);
        }

        public static TaxonomyTermSetModelNode AddRandomTerm(this TaxonomyTermSetModelNode model,
            Action<TaxonomyTermModelNode> action)
        {
            return model.AddRandomTypedDefinition<TaxonomyTermDefinition, TaxonomyTermSetModelNode, TaxonomyTermModelNode>(action);
        }


        public static TaxonomyTermModelNode AddRandomTerm(this TaxonomyTermModelNode model)
        {
            return AddRandomTerm(model, null);
        }

        public static TaxonomyTermModelNode AddRandomTerm(this TaxonomyTermModelNode model,
            Action<TaxonomyTermModelNode> action)
        {
            return model.AddRandomTypedDefinition<TaxonomyTermDefinition, TaxonomyTermModelNode, TaxonomyTermModelNode>(action);
        }

        #endregion

        #endregion

        #region internal

        public static ModelNode AddRandomDefinition<TDefinition>(this ModelNode model)
            where TDefinition : DefinitionBase
        {
            return AddRandomDefinition<TDefinition>(model, null);
        }

        public static ModelNode AddRandomDefinition<TDefinition>(this ModelNode model, Action<ModelNode> action)
              where TDefinition : DefinitionBase
        {
            return model.AddDefinitionNode(ModelGeneratorService.GetRandomDefinition<TDefinition>(), action);
        }

        #endregion
    }
}
