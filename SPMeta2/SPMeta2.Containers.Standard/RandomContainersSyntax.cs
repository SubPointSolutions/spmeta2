using System;
using System.Reflection;
using SPMeta2.Containers.DefinitionGenerators;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Containers.Standard
{
    public static class RandomContainersSyntax
    {
        #region constructors

        static RandomContainersSyntax()
        {
            ModelGeneratorService = new ModelGeneratorService();

            ModelGeneratorService.RegisterDefinitionGenerators(Assembly.GetExecutingAssembly());
            ModelGeneratorService.RegisterDefinitionGenerators(typeof(PublishingPageDefinition).Assembly);
        }

        #endregion

        #region properties

        public static ModelGeneratorService ModelGeneratorService { get; set; }

        #endregion

        #region syntax

        #region publishing pages

        public static ModelNode AddRandomPublishingPage(this ModelNode model)
        {
            return AddRandomPublishingPage(model, null);
        }

        public static ModelNode AddRandomPublishingPage(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<PublishingPageDefinition>(action);
        }

        #endregion


        #region taxonomy

        public static ModelNode AddRandomTermStore(this ModelNode model)
        {
            return AddRandomTermStore(model, null);
        }

        public static ModelNode AddRandomTermStore(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<TaxonomyTermStoreDefinition>(action);
        }

        public static ModelNode AddRandomTermGroup(this ModelNode model)
        {
            return AddRandomTermGroup(model, null);
        }

        public static ModelNode AddRandomTermGroup(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<TaxonomyTermGroupDefinition>(action);
        }

        public static ModelNode AddRandomTermSet(this ModelNode model)
        {
            return AddRandomTermSet(model, null);
        }

        public static ModelNode AddRandomTermSet(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<TaxonomyTermSetDefinition>(action);
        }

        public static ModelNode AddRandomTerm(this ModelNode model)
        {
            return AddRandomTerm(model, null);
        }

        public static ModelNode AddRandomTerm(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<TaxonomyTermDefinition>(action);
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
