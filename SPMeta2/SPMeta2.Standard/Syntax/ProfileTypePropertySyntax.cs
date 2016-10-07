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
    public class ProfileTypePropertyModelNode : TypedModelNode
    {

    }

    public static class ProfileTypePropertySyntax
    {
        #region methods

        public static TModelNode AddProfileTypeProperty<TModelNode>(this TModelNode model, ProfileTypePropertyDefinition definition)
            where TModelNode : ModelNode, IProfileTypePropertyHostModelNode, new()
        {
            return AddProfileTypeProperty(model, definition, null);
        }

        public static TModelNode AddProfileTypeProperty<TModelNode>(this TModelNode model, ProfileTypePropertyDefinition definition,
            Action<ProfileTypePropertyModelNode> action)
            where TModelNode : ModelNode, IProfileTypePropertyHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        //public static TModelNode AddCoreProperties<TModelNode>(this TModelNode model, IEnumerable<DocumentSetDefinition> definitions)
        //   where TModelNode : ModelNode, ISiteModelNode, new()
        //{
        //    foreach (var definition in definitions)
        //        model.AddDefinitionNode(definition);

        //    return model;
        //}

        #endregion
    }
}
