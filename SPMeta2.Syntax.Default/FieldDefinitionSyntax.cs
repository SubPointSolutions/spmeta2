using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class FieldDefinitionSyntax
    {
        #region methods

        public static ModelNode AddField(this ModelNode siteModel, DefinitionBase fielDefinition)
        {
            return AddFields(siteModel, new[] { fielDefinition });
        }

        public static ModelNode AddField(this ModelNode siteModel, DefinitionBase fielDefinition, Action<ModelNode> action)
        {
            var fieldNode = new ModelNode { Value = fielDefinition };

            siteModel.ChildModels.Add(fieldNode);

            if (action != null)
                action(fieldNode);

            return siteModel;
        }

        public static ModelNode AddFields(this ModelNode siteModel, params DefinitionBase[] fielDefinition)
        {
            return AddFields(siteModel, (IEnumerable<DefinitionBase>)fielDefinition);
        }

        public static ModelNode AddFields(this ModelNode siteModel, IEnumerable<DefinitionBase> fieldDefinitions)
        {
            foreach (var fieldDefinition in fieldDefinitions)
                siteModel.ChildModels.Add(new ModelNode { Value = fieldDefinition });

            return siteModel;
        }

        #endregion

        #region model



        #endregion
    }
}
