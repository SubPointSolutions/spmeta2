using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class DesignPackageModelNode : TypedModelNode
    {

    }

    public static class DesignPackageDefinitionSyntax
    {
        #region methods

        public static TModelNode AddDesignPackage<TModelNode>(this TModelNode model, DesignPackageDefinition definition)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return AddDesignPackage(model, definition, null);
        }

        public static TModelNode AddDesignPackage<TModelNode>(this TModelNode model, DesignPackageDefinition definition,
            Action<DesignPackageModelNode> action)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddDesignPackages<TModelNode>(this TModelNode model, IEnumerable<DesignPackageDefinition> definitions)
           where TModelNode : ModelNode, ISiteModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
