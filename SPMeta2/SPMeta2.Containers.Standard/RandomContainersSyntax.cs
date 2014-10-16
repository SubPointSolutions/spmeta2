using System;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Containers.Standard
{
    public static class RandomContainersSyntax
    {
        #region constructors

        static RandomContainersSyntax()
        {
            ModelGeneratorService = new ModelGeneratorService();
        }

        #endregion

        #region properties

        public static ModelGeneratorService ModelGeneratorService { get; set; }

        #endregion

        #region syntax


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
